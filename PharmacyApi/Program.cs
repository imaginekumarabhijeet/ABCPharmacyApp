using PharmacyApi.Data;
using PharmacyApi.Services;

var builder = WebApplication.CreateBuilder(args);

const string AngularDevCorsPolicy = "AngularDevCorsPolicy";


builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddSingleton<MedicineRepository>();
builder.Services.AddSingleton<SalesRepository>();
builder.Services.AddSingleton<SalesService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(AngularDevCorsPolicy, policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors(AngularDevCorsPolicy);
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var medicineRepository = scope.ServiceProvider.GetRequiredService<MedicineRepository>();
    await SeedData.EnsureSeededAsync(medicineRepository);
}

app.Run();
