using System;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.JSInterop;
using System.Reflection;
using System.Threading;
using GameViewer.Extensions;

namespace GameViewer.JSInteroperability
{
	public class LocalStorage
	{
		private const string Async = nameof(Async);

		protected IJSRuntime JSRuntime { get; }
		private string _key;

		public LocalStorage(
			IJSRuntime jSRuntime,
			string key
		)
		{
			JSRuntime = jSRuntime
				?? throw new ArgumentNullException(nameof(jSRuntime));

			_key = key
				?? throw new ArgumentNullException(nameof(key));
		}

		public static async Task<T> GetAsync<T>(
			IJSRuntime jSRuntime,
			string key,
			CancellationToken cancellationToken = default
		)
		{
			if (jSRuntime == null)
				throw new ArgumentNullException(nameof(jSRuntime));

			if (key == null)
				throw new ArgumentNullException(nameof(key));

			var methodName = MethodBase
				.GetCurrentMethod()
				.GetCurrentMethodFullName()
				.Replace(Async, string.Empty)
				;

			var result = await jSRuntime.InvokeAsync<T>(
				methodName,
				cancellationToken,
				new object[]
				{
					key
				}
			);

			return result;
		}

		public virtual async Task<T> GetAsync<T>(
			string key,
			CancellationToken cancellationToken = default
		)
			=> await GetAsync<T>(
				JSRuntime,
				$@"{_key}.{key}",
				cancellationToken
			);

		public static async Task SetAsync<T>(
			IJSRuntime jSRuntime,
			string key,
			T value,
			CancellationToken cancellationToken = default
		)
		{
			if (jSRuntime == null)
				throw new ArgumentNullException(nameof(jSRuntime));

			if (key == null)
				throw new ArgumentNullException(nameof(key));

			var methodName = MethodBase
				.GetCurrentMethod()
				.GetCurrentMethodFullName()
				.Replace(Async, string.Empty)
				;

			await jSRuntime.InvokeVoidAsync(
				methodName,
				cancellationToken,
				new object[]
				{
					key,
					value
				}
			);
		}

		public virtual async Task SetAsync<T>(
			string key,
			T value,
			CancellationToken cancellationToken = default
		)
			=> await SetAsync(
				JSRuntime,
				$@"{_key}.{key}",
				value,
				cancellationToken
			);

		public static async Task RemoveAsync(
			IJSRuntime jSRuntime,
			string key,
			CancellationToken cancellationToken = default
		)
		{
			if (jSRuntime == null)
				throw new ArgumentNullException(nameof(jSRuntime));

			if (key == null)
				throw new ArgumentNullException(nameof(key));

			var methodName = MethodBase
				.GetCurrentMethod()
				.GetCurrentMethodFullName()
				.Replace(Async, string.Empty)
				;

			await jSRuntime.InvokeVoidAsync(
				methodName,
				cancellationToken,
				new object[]
				{
					key
				}
			);
		}

		public virtual async Task RemoveAsync(
			string key,
			CancellationToken cancellationToken = default
		)
			=> await RemoveAsync(
				JSRuntime,
				$@"{_key}.{key}",
				cancellationToken
			);
	}
}