using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using RabbitWeApiDemo.Entities;
using RabbitWeApiDemo.MessageBusRepositories;

namespace RabbitWeApiDemo
{
    public class Startup
    {
        //TODO move it to resource/config file
        private static string AuthorRepositoryName = "Author";

        private static string BookRepositoryName = "Book";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver =
                        new DefaultContractResolver();
                });
            //Injecting MessageRepositoryFactory
            services.AddTransient<IMessageRepositoryFactory<Author>, MessageRepositoryFactory<Author>>(s => new MessageRepositoryFactory<Author>(AuthorRepositoryName));
            services.AddTransient<IMessageRepositoryFactory<Book>, MessageRepositoryFactory<Book>>(s => new MessageRepositoryFactory<Book>(BookRepositoryName));
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
