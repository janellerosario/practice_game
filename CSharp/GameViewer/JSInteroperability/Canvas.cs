using System;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.JSInterop;
using System.Reflection;
using System.Threading;
using GameViewer.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace GameViewer.JSInteroperability
{
	public static class Canvas
	{
		private const string Async = nameof(Async);

		public static async Task ClearAsync(
			IJSRuntime jSRuntime,
			string canvasId,
			CancellationToken cancellationToken = default
		)
		{
			if (jSRuntime == null)
				throw new ArgumentNullException(nameof(jSRuntime));

			if (canvasId == null)
				throw new ArgumentNullException(nameof(canvasId));

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
					canvasId
				}
			);
		}

		public static async Task StrokeRectangleAsync(
			IJSRuntime jSRuntime,
			string canvasId,
			double x,
			double y,
			double width,
			double height,
			double? lineWidth = null,
			string strokeStyle = null,
			IEnumerable<KeyValuePair<double, double>> dashStyle = null,
			CancellationToken cancellationToken = default
		)
		{
			if (jSRuntime == null)
				throw new ArgumentNullException(nameof(jSRuntime));

			if (canvasId == null)
				throw new ArgumentNullException(nameof(canvasId));

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
					canvasId,
					x,
					y,
					width,
					height,
					lineWidth,
					strokeStyle,
					dashStyle
						?.SelectMany(p => new []
						{
							p.Key,
							p.Value
						})
						?? Enumerable.Empty<double>()
				}
			);
		}

		public static async Task FillTextAsync(
			IJSRuntime jSRuntime,
			string canvasId,
			string text,
			double x,
			double y,
			string font = null,
			string fillStyle = null,
			CanvasTextAlignment? textAlignment = null,
			CanvasTextBaseline? textBaseline = null,
			string shadowColor = null,
			double? shadowOffsetX = null,
			double? shadowOffsetY = null,
			CancellationToken cancellationToken = default
		)
		{
			if (jSRuntime == null)
				throw new ArgumentNullException(nameof(jSRuntime));

			if (canvasId == null)
				throw new ArgumentNullException(nameof(canvasId));

			if (text == null)
				throw new ArgumentNullException(nameof(text));

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
					canvasId,
					text,
					x,
					y,
					font,
					fillStyle,
					textAlignment.HasValue
						? Enum
							.GetName(typeof(CanvasTextAlignment), textAlignment)
							.ToLower()
						: null
						,
					textBaseline.HasValue
						? Enum
							.GetName(typeof(CanvasTextBaseline), textBaseline)
							.ToLower()
						: null
						,
					shadowColor,
					shadowOffsetX,
					shadowOffsetY
				}
			);
		}

		public static async Task<CanvasTextMetrics> MeasureTextAsync(
			IJSRuntime jSRuntime,
			string canvasId,
			string text,
			string font = null,
			CancellationToken cancellationToken = default
		)
		{
			if (jSRuntime == null)
				throw new ArgumentNullException(nameof(jSRuntime));

			if (canvasId == null)
				throw new ArgumentNullException(nameof(canvasId));

			var methodName = MethodBase
				.GetCurrentMethod()
				.GetCurrentMethodFullName()
				.Replace(Async, string.Empty)
				;

			var jsonElement = await jSRuntime.InvokeAsync<JsonElement>(
				methodName,
				cancellationToken,
				new object[]
				{
					canvasId,
					text,
					font
				}
			);

			var textMetrics = JsonSerializer.Deserialize<CanvasTextMetrics>(
				jsonElement.ToString(),
				new JsonSerializerOptions
				{
					PropertyNamingPolicy = JsonNamingPolicy.CamelCase
				}
			);

			return textMetrics;
		}

		public static async Task<CanvasTextAlignment> GetTextAlignmentAsync(
			IJSRuntime jSRuntime,
			string canvasId,
			CancellationToken cancellationToken = default
		)
		{
			if (jSRuntime == null)
				throw new ArgumentNullException(nameof(jSRuntime));

			if (canvasId == null)
				throw new ArgumentNullException(nameof(canvasId));

			var methodName = MethodBase
				.GetCurrentMethod()
				.GetCurrentMethodFullName()
				.Replace(Async, string.Empty)
				;

			var value = await jSRuntime.InvokeAsync<string>(
				methodName,
				cancellationToken,
				new object[]
				{
					canvasId
				}
			);

			return Enum.TryParse<CanvasTextAlignment>(
				value,
				true,
				out var alignment
			)
				? alignment
				: throw new InvalidOperationException($"Unsupported {nameof(CanvasTextAlignment)} value \"{value}\".")
				;
		}

		public static async Task SetTextAlignmentAsync(
			IJSRuntime jSRuntime,
			string canvasId,
			CanvasTextAlignment textAlignment,
			CancellationToken cancellationToken = default
		)
		{
			if (jSRuntime == null)
				throw new ArgumentNullException(nameof(jSRuntime));

			if (canvasId == null)
				throw new ArgumentNullException(nameof(canvasId));

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
					canvasId,
					textAlignment.ToString()
				}
			);
		}

		public static async Task<CanvasTextBaseline> GetTextBaselineAsync(
			IJSRuntime jSRuntime,
			string canvasId,
			CancellationToken cancellationToken = default
		)
		{
			if (jSRuntime == null)
				throw new ArgumentNullException(nameof(jSRuntime));

			if (canvasId == null)
				throw new ArgumentNullException(nameof(canvasId));

			var methodName = MethodBase
				.GetCurrentMethod()
				.GetCurrentMethodFullName()
				.Replace(Async, string.Empty)
				;

			var value = await jSRuntime.InvokeAsync<string>(
				methodName,
				cancellationToken,
				new object[]
				{
					canvasId
				}
			);

			return Enum.TryParse<CanvasTextBaseline>(
				value,
				true,
				out var alignment
			)
				? alignment
				: throw new InvalidOperationException($"Unsupported {nameof(CanvasTextBaseline)} value \"{value}\".")
				;
		}

		public static async Task SetTextBaselineAsync(
			IJSRuntime jSRuntime,
			string canvasId,
			CanvasTextBaseline textBaseline,
			CancellationToken cancellationToken = default
		)
		{
			if (jSRuntime == null)
				throw new ArgumentNullException(nameof(jSRuntime));

			if (canvasId == null)
				throw new ArgumentNullException(nameof(canvasId));

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
					canvasId,
					textBaseline.ToString()
				}
			);
		}

		public static async Task<double?> GetWidthAsync(
			IJSRuntime jSRuntime,
			string canvasId,
			CancellationToken cancellationToken = default
		)
		{
			if (jSRuntime == null)
				throw new ArgumentNullException(nameof(jSRuntime));

			if (canvasId == null)
				throw new ArgumentNullException(nameof(canvasId));

			var methodName = MethodBase
				.GetCurrentMethod()
				.GetCurrentMethodFullName()
				.Replace(Async, string.Empty)
				;

			return await jSRuntime.InvokeAsync<double?>(
				methodName,
				cancellationToken,
				new object[]
				{
					canvasId
				}
			);
		}

		public static async Task<double?> GetHeightAsync(
			IJSRuntime jSRuntime,
			string canvasId,
			CancellationToken cancellationToken = default
		)
		{
			if (jSRuntime == null)
				throw new ArgumentNullException(nameof(jSRuntime));

			if (canvasId == null)
				throw new ArgumentNullException(nameof(canvasId));

			var methodName = MethodBase
				.GetCurrentMethod()
				.GetCurrentMethodFullName()
				.Replace(Async, string.Empty)
				;

			return await jSRuntime.InvokeAsync<double?>(
				methodName,
				cancellationToken,
				new object[]
				{
					canvasId
				}
			);
		}

		public static async Task SyncScaleAsync(
			IJSRuntime jSRuntime,
			string canvasId,
			CancellationToken cancellationToken = default
		)
		{
			if (jSRuntime == null)
				throw new ArgumentNullException(nameof(jSRuntime));

			if (canvasId == null)
				throw new ArgumentNullException(nameof(canvasId));

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
					canvasId
				}
			);
		}
	}
}