using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Models;
using Models.Models;
using Services;
using DataAccess.EF;
using University.MVC;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace WebApi
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
            //var connectionString = (@"Data Source=DESKTOP-G7US54D\SQLEXPRESS;Initial Catalog=University;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

            //my service 
            //services.AddScoped<IRepository<HomeTaskAssessment>>(p => new HomeTaskAssessmentRepository(connectionString));
            //services.AddScoped<IRepository<Student>>(p => new StudentRepository(connectionString));
            //services.AddScoped<IRepository<Course>>(p => new CourseRepository(connectionString));
            //services.AddScoped<IRepository<HomeTask>>(p => new HomeTaskRepository(connectionString));
            services.Configure<RepositoryOptions>(Configuration);
            services.AddDbContext<UniversityContext>();
            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("demo.db"));

            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            
            services.AddScoped<StudentService>();
            services.AddScoped<HomeTaskService>();
            services.AddScoped<CourseService>();

            services.Add(ServiceDescriptor.Scoped(typeof(IRepository<>), typeof(UniversityRepository<>)));

            services.ConfigureApplicationCookie(p =>
            {
                p.LoginPath = "/Account/Login";
                p.Cookie.Name = "MyWebApi";
            });

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,UserManager<IdentityUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            CreateAdminUser(userManager, roleManager).Wait();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("defoult","{controller}/{action}/{id?}");
            });
        }

        private async Task CreateAdminUser(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            IdentityUser identityUser = new IdentityUser
            {
                Email = "Admin@test.com",
                UserName = "Admin@test.com"
            };
            var userRes = await userManager.CreateAsync(identityUser, "SuperAdmin#1234");
            var rol = await roleManager.CreateAsync(new IdentityRole("Admin"));
            var res = await userManager.AddToRoleAsync(identityUser, "Admin");

        }
    }
}