namespace BasicAndJwtAuthenticationTogether.Handlers;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return Task.FromResult(AuthenticateResult.Fail("Authentication failed"));
        }

        AuthenticationHeaderValue headerValue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
        byte[] headerBytes = Convert.FromBase64String(headerValue.Parameter ?? "");
        string credentials = Encoding.UTF8.GetString(headerBytes);
        if (string.IsNullOrEmpty(credentials))
        {
            return Task.FromResult(AuthenticateResult.Fail("UnAuthorized"));
        }
        
        string[] credentialArray = credentials.Split(":");
        string userName = credentialArray[0];
        string password = credentialArray[1];

        if (userName == "admin" && password == "admin")
        {
            Claim[] claims = new[] {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.NameIdentifier, userName),
            };
            ClaimsIdentity identity = new ClaimsIdentity(claims, Scheme.Name);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            AuthenticationTicket authTicket = new AuthenticationTicket(principal, Scheme.Name);
            return Task.FromResult(AuthenticateResult.Success(authTicket));
        }
        else
        {
            return Task.FromResult(AuthenticateResult.Fail("invalid credential"));
        }
    }
}