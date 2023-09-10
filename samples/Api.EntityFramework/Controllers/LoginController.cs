namespace SparkPlug.Sample.WebApi.Controllers;

[ApiController, Route("auth"), ApiExplorerSettings(GroupName = "v1")]
public class LoginController : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login(Login login)
    {
        try
        {
            if (string.IsNullOrEmpty(login.UserName) || string.IsNullOrEmpty(login.Password))
            {
                return BadRequest("Username and/or Password not specified");
            }
            if (login.UserName.Equals("admin") && login.Password.Equals("admin@123"))
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("thisisasecretkey@123"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var jwtSecurityToken = new JwtSecurityToken(
                    issuer: "ABCXYZ",
                    audience: "http://localhost:51398",
                    claims: new List<Claim>(),
                    expires: DateTime.Now.AddMinutes(10),
                    signingCredentials: signinCredentials
                );
                Ok(new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken));
            }
        }
        catch
        {
            return BadRequest("An error occurred in generating the token");
        }
        return Unauthorized();
    }
}