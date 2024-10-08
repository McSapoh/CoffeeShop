using CoffeeShopAPI.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using System;
using CoffeeShopAPI.UnitOfWork;
using CoffeeShopAPI.UnitOfWork.Repositories;
using CoffeeShopAPI.Services;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http;

namespace CoffeeShopAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddMvc().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            // Adding session.
            services.AddSession(options => {
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CoffeeShopAPI", Version = "v1" });

                // Include 'SecurityScheme' to use JWT Authentication
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

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

                c.TagActionsBy(api =>
                {
                    if (api.GroupName != null)
                    {
                        return new[] { api.GroupName };
                    }

                    var controllerActionDescriptor = api.ActionDescriptor as ControllerActionDescriptor;
                    if (controllerActionDescriptor != null)
                    {
                        return new[] { controllerActionDescriptor.ControllerName };
                    }

                    throw new InvalidOperationException("Unable to determine tag for endpoint.");
                });
                c.DocInclusionPredicate((name, api) => true);

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            // Adding authentication.
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Auth:Issuer"],
                        ValidAudience = Configuration["Auth:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration["Auth:Key"])
                        ),
                        ClockSkew = TimeSpan.Zero
                    };
                })
                // External auth.
                .AddCookie()
                .AddGoogle(options =>
                {
                    options.ClientId = Configuration["Auth:Google:ClientId"];
                    options.ClientSecret = Configuration["Auth:Google:ClientSecret"];
                });

            // Adding cors for Angular application.
            services.AddCors(options => options.AddPolicy(name: "CoffeeShopOrigins",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .AllowAnyHeader();
                    }
                )
            );

            services.AddAutoMapper(typeof(Program).Assembly);
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
            #region Adding services
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IIngredientService, IngredientService>();
            services.AddScoped<IImagesService, ImagesService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IExternalAuthService, ExternalAuthService>();
            #endregion
            #region Adding repositories
            services.AddScoped<IIngredientRepository, IngredientRepository>();
            services.AddScoped<IConfirmEmailTokenRepository, ConfirmEmailTokenRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductOrderRepository, ProductOrderRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISizeRepository, SizeRepository>();
            services.AddScoped<IUserRepository,UserRepository>();
            #endregion

            services.AddDbContext<CoffeeShopContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            );
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            DbManagementService.MigrationInitialization(app);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoffeeShopAPI v1"));
            }

            app.UseHttpsRedirection();

            //Setting middleware for sending images.
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Configuration["ImagesDirPath"]),
                RequestPath = "/api/images"
            });

            app.UseRouting();

            //Using Cors.
            app.UseCors("CoffeeShopOrigins");

            app.UseAuthentication();
            app.UseAuthorization();

            // Using session.
            app.UseSession(); 

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
