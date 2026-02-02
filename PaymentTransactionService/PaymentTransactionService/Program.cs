using Microsoft.EntityFrameworkCore;
using PaymentTransactionService.Business.IServices;
using PaymentTransactionService.Business.Services;
using PaymentTransactionService.Data.IRepositories;
using PaymentTransactionService.Data.Models;
using PaymentTransactionService.Data.Repositories;
using PaymentTransactionService.Helper;
using PaymentTransactionService.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TransactionDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHttpClient();
builder.Services.AddTransient<ITransactionService, TransactionService>();
builder.Services.AddScoped<IPaymentTransactionRepository, PaymentTransactionRepository>();
builder.Services.AddScoped<IBaseRepository<PaymentTransaction>,BaseRepository<PaymentTransaction>>();
builder.Services.AddTransient<HttpClientHelper>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:4200") // Angular URL
                .AllowAnyHeader()
                .AllowAnyMethod()
                .WithExposedHeaders("X-Signature");
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<WebhookMiddleware>();
app.UseHttpsRedirection();
app.UseCors("AllowAngular");
app.UseAuthorization();

app.MapControllers();

app.Run();
