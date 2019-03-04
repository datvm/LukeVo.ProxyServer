using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using LukeVo.ProxyServer.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProxyKit;

namespace LukeVo.ProxyServer
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var settings = new ProxyServerSettings();
            this.Configuration.Bind(settings);
            services.AddSingleton(settings);

            services.AddProxy(configureOptions: options =>
            {
                options.IgnoreSSLCertificate = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ProxyServerSettings settings)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // Pre-parse URI for faster performance
            foreach (var proxy in settings.Proxy)
            {
                proxy.FromUri = proxy.From
                    .Select(q => new Uri(q))
                    .ToList();
            }

            app.RunProxy(context =>
            {
                var request = context.Request;

                var forwardTarget = settings.Proxy
                        .FirstOrDefault(q => q.FromUri.Any(p => p.IsSameOrigin(request)));

                if (forwardTarget != null)
                {
                    var message = context
                        .ForwardTo(forwardTarget.To)
                        .Send();

                    return message;
                }
                else
                {
                    return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.NotFound));
                }
            });
        }
    }
}
