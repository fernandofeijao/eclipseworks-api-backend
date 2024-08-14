using TaskManager.Api;
using TaskManager.Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddBaseConfig();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingProfiles));
builder.Services.AddRepositories(builder.Configuration.GetConnectionString("DbConnect"));
builder.Services.AddApplicationServices();

var app = builder.Build();

app.UseSwaggerDocs();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();