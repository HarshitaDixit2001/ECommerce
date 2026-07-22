using AppModels;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
namespace AppServices
{
    public interface IAuthServices
    {
        public Task<object> GetUserLogin(LoginRequest obj);
    }

    public class AuthServices : IAuthServices
    {
        // Existing code...
        private readonly IConfiguration _config;
        private readonly IDapperContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthServices(IConfiguration config, IDapperContext db, IHttpContextAccessor httpContextAccessor)
        {
            _config = config;
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }

        #region GetUserLogi
        public async Task<object> GetUserLogin(LoginRequest req)
        {
            try
            {
                if ((req.Num1 + req.Num2) != req.Sum_input)
                    return new { success = false, message = "Invalid Captcha. Please re-enter the correct code.!!" };

                var login_req = new dbLoginRequest { login_id = req.login_id, password = req.password };
                var result = await _db.ExecuteQueryFirstOrDefaultAsync<db_login_res>("sp_login_authorization", login_req);
                if (result?.error == "0")
                {
                    var jwtSettings = _config.GetSection("JwtSettings");
                    var token = GenerateJwtToken(result.id.ToString(), result.last_login);
                    // ✅ Create claims
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Sid, result.id.ToString()),
                        new Claim(ClaimTypes.NameIdentifier, result.login_id),
                        new Claim(ClaimTypes.Name, result.name),
                        new Claim(ClaimTypes.SerialNumber, token),
                        new Claim("last_login", result.last_login ?? ""),
                        new Claim("pf_img", result.pf_img ?? ""),
                        new Claim(ClaimTypes.Role, result.role_name ?? ""),
                        new Claim("role_name", result.role_name ?? ""),
                        new Claim("sub_role_name", result.sub_role_name ?? "")
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(claimsIdentity);

                    await _httpContextAccessor.HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        principal,
                        new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryInMinutes"]))
                        });
                    return new { success = true, role_name = result.role_name, message = token };
                }
            }
            catch (Exception er) { return new { success = false, role_name = "", message = er.Message }; }
            return new { success = false, role_name = "", message = "Invalid login" };
        }
        #endregion


        #region GenerateJwtToken
        private string GenerateJwtToken(string userId, string role)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim("role", role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryInMinutes"])),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion
    }

}
 