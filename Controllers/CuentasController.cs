using back_end.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace back_end.Controllers
{
    [Route("api/cuentas")]
    [ApiController]
    public class CuentasController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<IdentityUser> signInManager;

        public CuentasController(UserManager<IdentityUser> userManager, IConfiguration configuration, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
        }

        [HttpPost("crear")]
        public async Task<ActionResult<RespuestaAutenticacion>> Crear([FromBody] UsuarioInfo usuarioInfo)
        {
            var usuario = new IdentityUser { UserName = usuarioInfo.Email, Email = usuarioInfo.Email };
            var resultado = await userManager.CreateAsync(usuario, usuarioInfo.Password);

            if (resultado.Succeeded)
            {
                return await ConstruirToken(usuarioInfo);
            }
            else
            {
                return BadRequest(resultado.Errors);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<RespuestaAutenticacion>> Login([FromBody] UsuarioInfo usuarioInfo)
        {
            var resultado = await signInManager.PasswordSignInAsync(usuarioInfo.Email, usuarioInfo.Password, isPersistent: false, lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                return await ConstruirToken(usuarioInfo);
            }
            else
            {
                return BadRequest("Login Incorrecto");
            }
        }



        private async Task<RespuestaAutenticacion> ConstruirToken(UsuarioInfo usuarioInfo)
        {
            var claims = new List<Claim>()
            {
                new Claim("email", usuarioInfo.Email),
            };

            var usuario = await userManager.FindByEmailAsync(usuarioInfo.Email);
            var claimDB = await userManager.GetClaimsAsync(usuario);

            claims.AddRange(claimDB);

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["llavejwt"]));
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddYears(1);

            var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiration, signingCredentials: creds);

            return new RespuestaAutenticacion()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiracion = expiration
            };
        }
    }
}
