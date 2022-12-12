using CoffeeShopAPI.Repositories;
using CoffeeShopAPI.Interfaces.Repositories;
using CoffeeShopAPI.Interfaces.Repositories.Products;
using CoffeeShopAPI.Models;
using CoffeeShopAPI.Repositories.Products;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using CoffeeShopAPI.Repositories.Ingredients;
using CoffeeShopAPI.Interfaces.Repositories.Ingredients;
using CoffeeShopAPI.Repositories.Orders;
using CoffeeShopAPI.Interfaces.Repositories.Orders;
using CoffeeShopAPI.Repositories.Sizes;
using CoffeeShopAPI.Interfaces.Repositories.Sizes;

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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CoffeeShopAPI", Version = "v1" });
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            #region Adding ingredients repositories
            services.AddScoped<IAlcoholRepository, AlcoholRepository>();
            services.AddScoped<IMilkRepository, MilkRepository>();
            services.AddScoped<ISauceRepository, SauceRepository>();
            services.AddScoped<ISupplementsRepository, SupplementsRepository>();
            services.AddScoped<ISyrupRepository, SyrupRepository>();
            #endregion
            #region Adding orders repositories
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ICoffeeOrderRepository, CoffeeOrderRepository>();
            services.AddScoped<IDessertOrderRepository, DessertOrderRepository>();
            services.AddScoped<ISandwichOrderRepository, SandwichOrderRepository>();
            services.AddScoped<ISnackOrderRepository, SnackOrderRepository>();
            services.AddScoped<ITeaOrderRepository, TeaOrderRepository>();
            #endregion
            #region Adding products repositories
            services.AddScoped<ICoffeeRepository, CoffeeRepository>();
            services.AddScoped<IDessertRepository, DessertRepository>();
            services.AddScoped<ISandwichRepository, SandwichRepository>();
            services.AddScoped<ISnackRepository, SnackRepository>();
            services.AddScoped<ITeaRepository, TeaRepository>();
            #endregion
            #region Adding sizes repositories
            services.AddScoped<ICoffeeSizeRepository, CoffeeSizeRepository>();
            services.AddScoped<IDessertSizeRepository, DessertSizeRepository>();
            services.AddScoped<ISandwichSizeRepository, SandwichSizeRepository>();
            services.AddScoped<ISnackSizeRepository, SnackSizeRepository>();
            services.AddScoped<ITeaSizeRepository, TeaSizeRepository>();
            #endregion

            services.AddDbContext<CoffeeShopContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoffeeShopAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
