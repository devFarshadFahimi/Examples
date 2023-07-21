# TowMethodRateLimitedAuthenticationAPI
.NET Core 6 Web API ~> Basic authentication and JWT authentication implemented together for some security purposes,
        this project is also included rate limited request for all routes. (every second one request)

All methods are implemented for tutorial purposes and hard-coded in this project, for example you should use your data-souce service to check username and password in 
        "BasicAuthenticationHandler" class.
Also in "WeatherForecastController/GetToken" method, username and password are implemented in hard-coded for simplicity

Authentication and authorizeation Demonstration examples:

For this method ("WeatherForecastController/Get") you should use "Basic authentication" like below example :
<br />
![image](https://user-images.githubusercontent.com/65845296/198990079-fa4035d9-5e45-4dda-9204-47944b45f7a8.png)
--------------------------------------------------------------------------------------------------------------
For this method ("WeatherForecastController/TestJwt") you should use "Bearer token authentication" like below example :
<br />
![image](https://user-images.githubusercontent.com/65845296/198990587-0515cba5-5466-4c03-9fd2-74267d0d5acb.png)
--------------------------------------------------------------------------------------------------------------
