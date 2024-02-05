using Products.Api.HealthChecks;
using Products.Api.ProductEndpoints;
using Products.Module;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureSwagger();
builder.Services.AddHealthChecks();

builder.Services
    .AddCors()
    .AddProductsAuthz()
    .AddProducts()
    ;

var app = builder.Build();

app.UseCors()
   .UseHsts()
   .UseHttpsRedirection(); 

app
    .UsePathBase("/api/v1") // rundimentary api versioning 
    .UseSwagger()
    .UseSwaggerUI()
    ;

app
    .MapHealthCheckEndpoints()
    .AddProductEndpoints();

app.UseAuthentication();
app.UseAuthorization();

app.Run();

public partial class Program { }