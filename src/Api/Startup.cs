namespace Api
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using Api.Mediator;
    using Api.Utils;
    using Data;
    using FluentValidation;
    using FluentValidation.AspNetCore;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.IdentityModel.Logging;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.OpenApi.Models;
    using Services.Validators.Shared;

    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _webHostEnvironment;

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
            services.AddSwaggerGen(c =>
            {
                c.SchemaFilter<SwaggerUtils.BookSchemaFilter>();
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Library Api",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Name = "Daniel Londoño Ospina",
                        Email = "daniel_londono82122@elpoli.edu.co"
                    },
                    Description = "To generate a valid token consume the login controller endpoint, then add this token in the \"Authorize\" button"
                });
                // Include 'SecurityScheme' to use JWT Authentication
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Bearer token:",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        { jwtSecurityScheme, Array.Empty<string>() }
                    });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            Console.WriteLine("Adding Authentication");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });

            Console.WriteLine("Adding DB");
            var dbConnectionString = DataUtils.GetDbConnectionString(Configuration, _webHostEnvironment.ContentRootPath);
            services.AddDbContext<Context>(options =>
                                options.UseSqlServer(dbConnectionString,
                                sqlOptions =>
                                {
                                    sqlOptions.EnableRetryOnFailure(
                                    maxRetryCount: 5,
                                    maxRetryDelay: TimeSpan.FromSeconds(10),
                                    errorNumbersToAdd: null);
                                    sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                                }));


            Console.WriteLine("Adding MediatR");
            services.AddMediatRConf();

            Console.WriteLine("Adding AutoMapper");
            services.AddAutoMapper(Assembly.Load("Services"));

            Console.WriteLine("Adding DI");
            services.AddScoped<IContext, Context>();
            services.AddScoped<ICommonValidators, CommonValidators>();

            services.AddValidatorsFromAssembly(typeof(CommonValidators).Assembly);

            IdentityModelEventSource.ShowPII = true;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web Service V1");
                    c.RoutePrefix = string.Empty;
                    c.DisplayOperationId();
                    c.DisplayRequestDuration();
                    c.EnableDeepLinking();
                });
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.EnsureMigrationOfContext<Context>();
        }
    }
}
