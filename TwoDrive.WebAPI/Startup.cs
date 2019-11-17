﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TwoDrive.DataAccess;
using Microsoft.EntityFrameworkCore;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.Domain;
using TwoDrive.BusinessLogic.Logic;
using TwoDrive.BusinessLogic;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain.FileManagement;
using TwoDrive.WebApi.Interfaces;
using TwoDrive.WebApi.Helpers;
using TwoDrive.BusinessLogic.Validators;
using TwoDrive.BusinessLogic.Interfaces.LogicInput;
using TwoDrive.BusinessLogic.Helpers.LogicInput;
using Newtonsoft.Json;

namespace TwoDrive.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    );
            });

            services.AddMvc().AddJsonOptions(opts =>
            {
                opts.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
                opts.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<TwoDriveDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default")));

            services.AddScoped<ILogic<Writer>, WriterLogic>();
            services.AddScoped<IFolderLogic, FolderLogic>();
            services.AddScoped<ICurrent, CurrentSession>();
            services.AddScoped<ISessionLogic, SessionLogic>();
            services.AddScoped<ILogic<File>, FileLogic>();
            services.AddScoped<IFileLogic, FileLogic>();
            services.AddScoped<IModificationLogic, ModificationLogic>();
            services.AddScoped<IImporterLogic, ImporterLogic>();

            services.AddScoped<IFolderValidator, FolderValidator>();
            services.AddScoped<IValidator<Element>, FileValidator>();
            services.AddScoped<IValidator<Writer>, WriterValidator>();
            services.AddScoped<ElementLogicDependencies, ElementLogicDependencies>();
            services.AddScoped<ImporterDependencies, ImporterDependencies>();

            services.AddScoped<IFolderRepository, FolderRepository>();
            services.AddScoped<IFileRepository, FileRepository>();
            services.AddScoped<IRepository<Element>, Repository<Element>>();
            services.AddScoped<IRepository<Writer>, WriterRepository>();
            services.AddScoped<IRepository<Session>, SessionRepository>();
            services.AddScoped<IRepository<Modification>, ModificationRepository>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMvc();
            app.UseCors("CorsPolicy");
        }
    }
}
