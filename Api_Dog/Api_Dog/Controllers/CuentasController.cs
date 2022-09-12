using Api_Dog.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api_Dog.Controllers
{
    [ApiController]
    [Route("api/cuentas")]
    public class CuentasController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<IdentityUser> signInManager;

        public CuentasController(UserManager<IdentityUser> userManager,
            IConfiguration configuration,SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
        }

        [HttpPost("registrar")]
        public async Task<ActionResult<RespuestaAutenticacion>> Registrar(CredencialesUsuarios credencialesUsuarios)
        {
            var usuario = new IdentityUser { UserName = credencialesUsuarios.Email,Email = credencialesUsuarios.Email };
            var resultado = await userManager.CreateAsync(usuario,credencialesUsuarios.Password);

            if (resultado.Succeeded)
            {
                return ConstruirToken(credencialesUsuarios);
            }
            else
            {
                return BadRequest(resultado.Errors);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<RespuestaAutenticacion>> Login(CredencialesUsuarios credencialesUsuarios)
        {
            var resultado = await signInManager.PasswordSignInAsync(credencialesUsuarios.Email,
                credencialesUsuarios.Password, isPersistent: false, lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                return ConstruirToken(credencialesUsuarios);
            }
            else
            {
                return BadRequest("Login Incorrecto");
            }
        }

        private RespuestaAutenticacion ConstruirToken(CredencialesUsuarios credencialesUsuarios)
        {
            var claims = new List<Claim>()
            {
                new Claim("email",credencialesUsuarios.Email)
            };

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["llaveJWT"]));
            var cred = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);
            var expiracion = DateTime.UtcNow.AddYears(1);
            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiracion, 
                signingCredentials: cred);

            return new RespuestaAutenticacion()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiracion = expiracion
            };
        }
    }
}
