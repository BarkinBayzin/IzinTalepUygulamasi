using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddApplicationServices();

builder.Services.AddPersistenceServices();
builder.Services.AddDbContext<IzinTalepAPIContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MSSQL")));

builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>()) //Kendi filterımı container'a ekliyorum.
    .AddFluentValidation(configuration =>
        configuration.RegisterValidatorsFromAssemblyContaining<CreateLeaveRequestValidator>()) //Reflection sayesinde kendisi çalıştığı container içerisindeki assemblyde ne kadr validator sınıfı varsa hepsini kullanacağını söylüyoruz. Artık sadece application'ı tarif eden bir sınıf bildirmek yeterli olacaktır.
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);//mevcut olan filtrelemeleri kapatır. Bundan sonra sadece benim yazdığım validasyonları çalıştır diyorum. 

builder.Services.AddCors(options =>
options.AddDefaultPolicy(policy =>
    policy.WithOrigins("http://localhost:7296", "https://localhost:5001")
    .AllowAnyHeader().
    AllowAnyMethod()));

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

app.MapControllers();

app.Run();
