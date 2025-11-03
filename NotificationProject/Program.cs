using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NotificationProject.Data;
using NotificationProject.Repository;
using NotificationProject.Repository.IRepository;
using NotificationProject.Service;
using NotificationProject.Service.IService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options=>
{
    //Document For V1
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1.0",
        Title = "Notification Module",
        Description = "API For Notification Module",
    });
}
);


//DataBaseContext For SqlServer
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IMessageSender, SmsSender>();
builder.Services.AddScoped<IMessageSender, EmailSender>();
builder.Services.AddScoped<INotificationService, NotificationService>();

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
