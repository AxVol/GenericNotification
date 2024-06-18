using System.Reflection;
using NotificationSender.Application.DependencyInjection;
using NotificationSender.Consumer.DependencyInjection;
using NotificationSender.DAL.DependencyInjection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
        $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
});

builder.Services.AddDataAccessLevel(builder.Configuration);
builder.Services.AddBrokerConsumer(builder.Configuration);
builder.Services.AddServices();
builder.Services.AddLocalization(opt => opt.ResourcesPath = "../Core/Resources");

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .CreateLogger();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();