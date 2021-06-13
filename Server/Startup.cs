using Application.Commands.DeleteAllEmails;
using Application.Commands.DeleteEmail;
using Application.Commands.SaveEmail;
using Application.Queries.GetAllEmails;
using Application.Queries.GetEmail;
using Domain.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence;
using Server.Middlewares;

namespace Server
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

            //Persistence
            services.AddSingleton<IEmailStorage, EmailStorage>();

            //Application
            services.AddSingleton<IGetAllEmailsHandler, GetAllEmailsHandler>();
            services.AddSingleton<IGetEmailHandler, GetEmailHandler>();
            services.AddSingleton<ISaveEmailHandler, SaveEmailHandler>();
            services.AddSingleton<IDeleteEmailHandler, DeleteEmailHandler>();
            services.AddSingleton<IDeleteAllEmailsHandler, DeleteAllEmailsHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseMiddleware<Middleware>();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
