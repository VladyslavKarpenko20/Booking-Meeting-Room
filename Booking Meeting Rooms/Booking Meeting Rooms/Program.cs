using Booking_Meeting_Rooms.Context;
using Booking_Meeting_Rooms.Interface;
using Booking_Meeting_Rooms.Middleware;
using Booking_Meeting_Rooms.Models;
using Booking_Meeting_Rooms.Repository;
using Booking_Meeting_Rooms.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IAuthServices, AuthServices>();
builder.Services.AddScoped<IRoomServices, RoomServices>();
builder.Services.AddScoped<IEquipmentServices, EquipmentServices>();
builder.Services.AddScoped<IBookingServices, BookingServices>();
builder.Services.AddScoped<IPdfServices, GeneratePdfServices>();


builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IEquipmentRepository, EquipmentRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();


builder.Services.AddDbContext<AddDbContext>(options => options.UseNpgsql(builder.Configuration["Database:Strings"], optionsSql => optionsSql.EnableRetryOnFailure()));



builder.Services.AddAuthentication("Bearer")
.AddJwtBearer("Bearer", options =>
{

    var secretKey = builder.Configuration["JWTSetting:SecretKey"] ?? "1234";

    options.TokenValidationParameters = new TokenValidationParameters
    {

        ValidateIssuer = true,
        ValidIssuer = "MyApp",

        ValidateAudience = true,
        ValidAudience = "User",

        ValidateLifetime = true,

        ValidateIssuerSigningKey = true,

        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),

    };

});

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorize",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Description = "Write Referens Token",
        Scheme = "bearer",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        BearerFormat = "JWT",


    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }

            },
            new string[] { }



        }


    });


});


var app = builder.Build();

app.UseMiddleware<MyExceptionsMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<MyLoggMiddleware>();

app.MapControllers();

app.Run();
