using EipqLibrary.EmailService.Interfaces;
using EipqLibrary.EmailService.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EipqLibrary.EmailService.Extensions
{
	public static class EmailServiceExtension
	{
		public static IServiceCollection AddEmailService(this IServiceCollection services)
		{
			//service
			services.AddTransient<IEmailService, EipqLibrary.EmailService.Services.EmailService>();

			//factory
			services.AddSingleton(typeof(IMessageDeliveryClientFactory<>), typeof(MessageDeliveryClientFactory<>));

			return services;
		}

	}
}
