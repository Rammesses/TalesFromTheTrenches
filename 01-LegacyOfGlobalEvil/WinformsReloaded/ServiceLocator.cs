using System;
using Microsoft.Extensions.DependencyInjection;

namespace WinformsReloaded
{
    public class ServiceLocator
    {
        IServiceProvider serviceProvider { get; init; }

        private static ServiceLocator? Instance { get; set; }

        public ServiceLocator(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public static void Initialize(IServiceProvider serviceProvider) => Instance = new ServiceLocator(serviceProvider);

        public static T GetRequired<T>()
            where T : class => Instance.serviceProvider.GetRequiredService<T>();
    }
}
