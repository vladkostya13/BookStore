using BookStore.API.Dtos.User;
using BookStore.API.Managers.Interfaces;
using BookStore.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.API.Managers.Implementations
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        private User _user;
        private IConfigurationSection _jwtSettings;

        public AuthenticationManager(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
            _jwtSettings = _configuration.GetSection("JwtSettings");
        }

        public async Task<bool> ValidateUser(UserAuthenticationDto userAuthenticationDto)
        {
            _user = await _userManager.FindByNameAsync(userAuthenticationDto.UserName);
            var isPasswordValid = await _userManager.CheckPasswordAsync(_user, userAuthenticationDto.Password);

            return (_user != null && isPasswordValid);
        }

        public async Task<string> CreateToken()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaimsAsync();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_jwtSettings.GetSection("key").Value);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaimsAsync()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, _user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(_user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var expireDays = Convert.ToDouble(_jwtSettings.GetSection("ExpireDays").Value);

            return new JwtSecurityToken
            (
                issuer: _jwtSettings.GetSection("Issuer").Value,
                audience: _jwtSettings.GetSection("Audience").Value,
                expires: DateTime.Now.AddDays(expireDays),
                claims: claims,
                signingCredentials: signingCredentials
            );
        }
    }
}