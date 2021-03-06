﻿using Domain.Tenats;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ServiceForWorkingWithApartmentBuilding.Models.Tenat;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceForWorkingWithApartmentBuilding.Controllers
{
    public class TenantController : Controller
    {
        [HttpPost("/Login")]
        public async Task<IActionResult> Login(CancellationToken cancellationToken, 
            string userName,
            string password,
            [FromServices] ITenantRepository repository)
        {
            var tenant = await repository.Get(userName, password, cancellationToken);
            if (tenant == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            return await GetToken(tenant.TenatId);
        }


        [HttpPost("/Register")]
        public async Task<IActionResult> Register(CancellationToken cancellationToken, 
           [System.Web.Http.FromBody] CreateTenantBinding binding,
           [FromServices] TenantManager manager,
           [FromServices] ITenantRepository repository)
        {
            var tenant = await repository.Get(binding.Name, binding.Surname, binding.Password, cancellationToken);
            if (tenant != null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            try
            {
                tenant = await manager.Create(binding.Name,
                    binding.Surname,
                    binding.Password,
                    binding.DateOfBirth,
                    binding.Address,
                    binding.EntranceNumber,
                    binding.FlatNumber,
                    cancellationToken);
            }
            catch(BuildingWithSuchAddressNotRegisteredException e)
            {
                return NotFound();
            }

            if (binding.Avatar != null)
            {
                byte[] avatarData = null;

                using (var binaryReader = new BinaryReader(binding.Avatar.OpenReadStream()))
                {
                    avatarData = binaryReader.ReadBytes((int)binding.Avatar.Length);
                }

                tenant.PutAvatar(avatarData);
            }

            await repository.Save(tenant);

            return await GetToken(tenant.TenatId);
        }

        [HttpDelete("tenants/{tenantId}")]
        public async Task<IActionResult> DeleteTenat(CancellationToken cancellationToken,
        [FromRoute] Guid tenatId,
        [FromServices] ITenantRepository repository)
        {
            var tenant = await repository.Get(tenatId, cancellationToken);

            if (tenant == null)
                return NoContent();

            await repository.Delete(tenant);

            return NoContent();
        }

        private async Task<IActionResult> GetToken(Guid userId)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, userId.ToString())
                };
            var identity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), 
                    SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            return Json(response);
        }
    }
}
