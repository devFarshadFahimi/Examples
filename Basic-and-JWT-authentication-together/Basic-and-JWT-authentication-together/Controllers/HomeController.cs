namespace BasicAndJwtAuthenticationTogether.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class HomeController : ControllerBase
{
    #region Fields
    private static readonly string[] Summaries = new[]
    {
            "Freezing",
            "Bracing",
            "Chilly",
            "Cool",
            "Mild",
            "Warm",
            "Balmy",
            "Hot",
            "Sweltering",
            "Scorching"
        };

    private readonly IConfiguration _configuration;

    #endregion

    #region Constructors

    public HomeController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    #region Basic endpoints

    [HttpGet]
    [Authorize(AuthenticationSchemes = BasicAuthenticationDefaults.AuthenticationScheme)]
    public IActionResult GetBasicResponse()
    {
        WeatherForecast[] result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();

        return Ok(new { ResultType = "Basic", Data = result });
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult GetBasicToken()
    {
        string? userName = _configuration["TestUser:UserName"];
        string? password = _configuration["TestUser:Password"];
        if (userName != "admin" || password != "admin")
        {
            return Unauthorized();
        }

        var concatUserNamePassword = $"{userName}:{password}";
        byte[] tokenBytes = Encoding.UTF8.GetBytes(concatUserNamePassword);
        string token = Convert.ToBase64String(tokenBytes);
        return Ok(token);
    }
    #endregion

    #region Jwt endpoints

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult GetJwtResponse()
    {
        WeatherForecast[] result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();

        return Ok(new { ResultType = "JWT", Data = result });
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult GetJwtToken()
    {
        string? userName = _configuration["TestUser:UserName"];
        string? password = _configuration["TestUser:Password"];
        if (userName != "admin" || password != "admin")
        {
            return Unauthorized();
        }

        string? issuer = _configuration["Jwt:Issuer"];
        string? audience = _configuration["Jwt:Audience"];
        byte[] key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                    new Claim("Id", Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, userName),
                    new Claim(JwtRegisteredClaimNames.Email, userName),
                    new Claim(JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString())
                 }),
            Expires = DateTime.UtcNow.AddMinutes(5),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials
            (new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha512Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken? token = tokenHandler.CreateToken(tokenDescriptor);
        string? jwtToken = tokenHandler.WriteToken(token);
        return Ok(jwtToken);
    }

    #endregion
}