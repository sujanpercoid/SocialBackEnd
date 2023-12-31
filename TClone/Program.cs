using Data;
using Microsoft.EntityFrameworkCore;

using TClone.Implementation;
using TClone.RepoImplementation;
using TClone.Repository;
using TClone.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<TcDbcontext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("conn")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<IAuth, Auth>();
builder.Services.AddScoped<IFeed, Feed>();
builder.Services.AddScoped<INotification,Noti>();
builder.Services.AddScoped<IBookmark,Bookmarkss>();
builder.Services.AddScoped<IProfile,Profile>();
//Repository For all
builder.Services.AddTransient(typeof(IGenericRepository<>),typeof(GenericRepository<>));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
