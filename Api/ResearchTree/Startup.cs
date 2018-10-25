﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using ResearchTree.Context;
using ResearchTree.Service.FeedService;
using ResearchTree.Service.JobService;

namespace ResearchTree
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
            // don't know if this is really needed
            services.Configure<CookiePolicyOptions>(options =>
            {
                
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // database connection
            var connection = @"Server=tcp:researchtree.database.windows.net,1433;Initial Catalog=ResearchTree;Persist Security Info=False;User ID=researchtree;Password=Mizzou@2019;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            services.AddDbContext<FeedContext>
                (options => options.UseSqlServer(connection));
            services.AddDbContext<JobContext>
                (options => options.UseSqlServer(connection));
            services.AddScoped<FeedHelper>();
            services.AddScoped<JobHelper>();

            services.AddMvc();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("http://localhost:3000"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseCors("AllowSpecificOrigin");

        }
    }
}