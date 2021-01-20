namespace chat
{
    using chat.Cache;
    using chat.Data;
    using chat.Data.Repositories;
    using chat.Services.Engines;
    using chat.Services.Interfaces;
    using chat.SignalR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            //TODO: Third party loglama kullanılacak ise ayarları burada yapılacak
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddEntityFrameworkNpgsql().AddDbContext<ChatDbContext>((optionBuilder) =>
            optionBuilder.UseNpgsql(Configuration.GetConnectionString(nameof(ChatDbContext))));

            services.AddControllersWithViews();
            services.AddSignalR();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<ChatDbContext>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRedisCacheService, RedisCacheService>();

            //* Configure Settings
            services.Configure<RedisCacheSettings>(Configuration.GetSection("Redis"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=User}/{action=Index}/{id?}");

                endpoints.MapHub<ChatHub>("/chatHub");
            });
        }
    }
}
