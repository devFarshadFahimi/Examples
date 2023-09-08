using CustomMiddlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapWhen(context=>
{
    var header = context.Request.Headers.ContainsKey("BackOfficeUser");
    return header;
}, mapWhen =>
{
    mapWhen.Map("/admin", p =>
    {
        p.UseMiddleware<AdminMiddleware>();
    });

    mapWhen.Map("/payment", p =>
    {
        p.UseMiddleware<BackOfficePaymentServiceMiddleware>();
    });
});

app.MapWhen(context=>
{
    var headerValue = context.Request.Headers.ContainsKey("FrontOfficeUser");
    return headerValue;
}, mapWhen =>
{
    mapWhen.Map("/payment", p =>
    {
        p.UseMiddleware<FrontOfficePaymentServiceMiddleware>();
    });
});


app.MapControllers();

app.Run();
