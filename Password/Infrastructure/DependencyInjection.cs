using Microsoft.Extensions.DependencyInjection;
using Password.Interfaces;
using Password.Services;

namespace Password.Infrastructure
{
    public static class DependencyInjection
    {
        public static ServiceProvider Build()
        {
            var services = new ServiceCollection();


            services.AddSingleton<IActiveDirectoryService, ActiveDirectoryService>();

            services.AddSingleton<IMailService, MailService>();

            services.AddSingleton<IJsonService, JsonService>();

            services.AddSingleton<IPasswordGenerator, PasswordGenerator>();

            services.AddSingleton<IPasswordService, PasswordService>();


            return services.BuildServiceProvider();
        }
    }
}