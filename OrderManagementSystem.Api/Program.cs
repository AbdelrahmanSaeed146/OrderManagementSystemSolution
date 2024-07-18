using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OrderManagementSystem.Api.Extentions;
using OrderManagementSystem.Core;
using OrderManagementSystem.Core.Entities;
using OrderManagementSystem.Repsitory.Data;
using OrderManagementSystem.Repsitory.Identity;



namespace OrderManagementSystem.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
       
        builder.Services.AddApplicationServices();
        builder.Services.AddDbContext<OrderManagementDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefultConnection"));
        });

        builder.Services.AddDbContext<AppIdentityDbConext>(Options =>
        {
            Options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
        });
        //builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbConext>();
        builder.Services.AddIdentityService(builder.Configuration);
        //builder.Services.Configure<EmailStamp>(builder.Configuration.GetSection("EmailStamp"));
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "OrderManagementSystem API", Version = "v1" });

            // Customize the schema ID generation to avoid conflicts
            c.CustomSchemaIds(type => type.FullName);

            // Other Swagger configurations
        });
        var app = builder.Build();


        using var Scope = app.Services.CreateScope();

        var services = Scope.ServiceProvider;

        var LoogerFactory = services.GetRequiredService<ILoggerFactory>();
        var _dbContext = services.GetRequiredService<OrderManagementDbContext>();
        var IdentityDbContext = services.GetRequiredService<AppIdentityDbConext>();

        try
        {
            //await _dbContext.Database.MigrateAsync();
            await StoreContextSeed.SeedAsync(_dbContext);


            //await IdentityDbContext.Database.MigrateAsync();
            var UserManager = services.GetRequiredService<UserManager<User>>();
            //await AppIdentityDbContextSeed.SeedUserAsync(UserManager);



        }
        catch (Exception ex)
        {
            var looger = LoogerFactory.CreateLogger<Program>();

            looger.LogError(ex, "An Error Has been occured during Apply Migration");

        }


        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseStaticFiles();
        app.MapControllers();

      await  app.RunAsync();
    }
}
