using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GameViewer
{
	///	<summary>
	///	The main program.  It contains the entry code <c>Main(string[])</c>
	///	that the host of this application will invoke when loading.
	///	</summary>
	public class Program
	{
		///	<summary>
		///	Configures a <see cref="WebAssemblyHost" /> that encapsulates all of the app's
		///	resources, such as:
		///	
		///	- An HTTP server implementation
		///	- Middleware components
		///	- Logging
		///	- Dependency injection (DI) services
		///	- Configuration
		///	
		///	</summary>
		///	<seealso>
		///	[ASP.NET Core fundamentals - Host](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/#host)
		///	</seealso>
		public static async Task Main(string[] args)
		{
			var builder = CreateHostBuilder(args);

			var host = builder.Build();
			
			await host.RunAsync();
		}

		///	<summary>
		///	Creates a WebAssembly host.
		///	</summary>
		///	<param name="args">
		///	A <see cref="String" />[] containing arguments sent by the host.
		///	</param>
		///	<returns>
		///	Returns a <see cref="WebAssemblyHostBuilder" /> with initial configurations.
		///	</returns>
		public static WebAssemblyHostBuilder CreateHostBuilder(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);

			//	Maps the app root to the root HTML DOM object that will
			//	contain the app.  The CSS selector '#app' is the unique
			//	identifier in the 'index.html' document to find the
			//	HTML DOM object.
			//
			//	See: https://www.w3schools.com/cssref/css_selectors.asp
			builder
				.RootComponents
				.Add<App>("#app")
				;

			//	Adds a HTTPClient instance to the 'Scoped' services.
			//	A scoped service is created once per client request (connection)
			//	Scoped is one of the three service livfetimes.  The other two are
			//	Transient, which creates a service every time it is requested, and
			//	Singleton, which creates a service only once and is used throughout
			//	the remainder of the host's lifetime.
			//	
			//	This is called Dependency Injection as components within the
			//	application may require connecting to remote APIs to exchange
			//	information.
			builder
				.Services
				.AddScoped(
					sp => new HttpClient
					{
						BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
					}
				);

			return builder;
		}
	}
}
