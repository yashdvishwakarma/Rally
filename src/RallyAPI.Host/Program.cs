using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RallyAPI.Users.Infrastructure;
using RallyAPI.Users.Endpoints;
using RallyAPI.Catalog.Infrastructure;
using RallyAPI.Catalog.Endpoints;
using RallyAPI.Integrations.ProRouting;
using RallyAPI.SharedKernel.Abstractions.Delivery;
using RallyAPI.SharedKernel.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add Users Module
builder.Services.AddUsersInfrastructure(builder.Configuration);
builder.Services.AddUsersEndpoints();

// Catalog Module
builder.Services.AddCatalogInfrastructure(builder.Configuration);
builder.Services.AddCatalogEndpoints();

// Add ProRouting Integration
builder.Services.AddProRoutingIntegration(builder.Configuration);

// Add MediatR for Application layer
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(RallyAPI.Users.Application.Abstractions.IUnitOfWork).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(RallyAPI.Catalog.Application.Abstractions.IUnitOfWork).Assembly);
});

// Add Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration.GetSection("JwtSettings");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!))
        };
    });

// Add Authorization Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Customer", policy =>
        policy.RequireClaim("user_type", "customer"));
    options.AddPolicy("Rider", policy =>
        policy.RequireClaim("user_type", "rider"));
    options.AddPolicy("Restaurant", policy =>
        policy.RequireClaim("user_type", "restaurant"));
    options.AddPolicy("Admin", policy =>
        policy.RequireClaim("user_type", "admin"));
});

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Add Global Exception Handler (early in pipeline!)
app.UseGlobalExceptionHandler();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

// Map endpoints
app.MapUsersEndpoints();
app.MapGet("/", () => "Rally API is running!");

// ============================================
// TEST ENDPOINT - Remove after testing
// ============================================
// RAW TEST - Add this after your other test endpoint
app.MapGet("/api/test/raw-prorouting", async () =>
{
    var handler = new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };

    using var client = new HttpClient(handler);
    client.BaseAddress = new Uri("https://preprod.logistics-buyer.mp2.in");
    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
    // Set headers exactly like PowerShell
    client.DefaultRequestHeaders.Add("x-pro-api-key", "mfnbkfyn23onbxtwrkr6tfmhxesb55sh37fr6cppgu47sb52te6ws5fdgsolxgql");

    var requestBody = """
    {
        "pickup": {"lat": 12.921, "lng": 77.588, "pincode": "560041"},
        "drop": {"lat": 12.920, "lng": 77.586, "pincode": "560041"},
        "city": "Bangalore",
        "order_category": "F&B",
        "search_category": "Immediate Delivery",
        "order_amount": 200,
        "order_weight": 2
    }
    """;

    var content = new StringContent(requestBody, System.Text.Encoding.UTF8, "application/json");

    var response = await client.PostAsync("/partner/estimate", content);
    var responseBody = await response.Content.ReadAsStringAsync();

    return Results.Ok(new
    {
        StatusCode = (int)response.StatusCode,
        Headers = response.Headers.ToString(),
        Body = responseBody
    });
})
.WithName("RawProRoutingTest")
.WithTags("Test")
.AllowAnonymous();
app.MapPost("/api/test/delivery-quote", async (
    IDeliveryQuoteProvider quoteProvider,
    DeliveryQuoteTestRequest request) =>
{
    var quoteRequest = DeliveryQuoteRequest.Create(
        pickupLatitude: request.PickupLat,
        pickupLongitude: request.PickupLng,
        pickupPincode: request.PickupPincode,
        dropLatitude: request.DropLat,
        dropLongitude: request.DropLng,
        dropPincode: request.DropPincode,
        city: request.City,
        orderAmount: request.OrderAmount,
        orderWeight: request.OrderWeight);

    var result = await quoteProvider.GetQuoteAsync(quoteRequest);

    return result.IsSuccess
        ? Results.Ok(result)
        : Results.BadRequest(result);
})
.WithName("TestDeliveryQuote")
.WithTags("Test")
.AllowAnonymous();
// ============================================

app.Run();

// ============================================
// TEST REQUEST MODEL - Remove after testing
// ============================================
public record DeliveryQuoteTestRequest(
    double PickupLat,
    double PickupLng,
    string PickupPincode,
    double DropLat,
    double DropLng,
    string DropPincode,
    string City,
    decimal OrderAmount,
    decimal? OrderWeight = null);