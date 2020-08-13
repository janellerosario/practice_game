using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Engine;
using GameViewer.JSInteroperability;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace GameViewer.Engines
{
	public abstract class GameEngineViewer
		: GameEngine
	{
		private readonly CancellationTokenSource _cancellationTokenSource
			= new CancellationTokenSource();

		private DateTime? _lastRenderTimestamp;
		private DateTime? _lastUpdateTimestamp;

#if DEBUG

		private string _debugFillStyle;
		private string _debugShadowColor;

#endif

		protected string CanvasId { get; }

		protected IJSRuntime JSRuntime { get; }

		protected CancellationToken CancellationToken
			=> _cancellationTokenSource.Token;

		public string DebugFont { get; set; }
			= "10px Arial";

		protected GameEngineViewer(
			GameEngineOptions options,
			IJSRuntime jsRuntime,
			string canvasId
		)
			: base(options)
		{
			CanvasId = canvasId
				?? throw new ArgumentNullException(nameof(canvasId))
				;

			JSRuntime = jsRuntime
				?? throw new ArgumentNullException(nameof(jsRuntime))
				;
		}

#if DEBUG

		protected override async void RenderDebugInformation()
		{
			base.RenderDebugInformation();

			var now = DateTime.Now;

			var canvasWidth = await Canvas
				.GetWidthAsync(
					JSRuntime,
					CanvasId,
					_cancellationTokenSource.Token
				)
					?? throw new InvalidOperationException($"Cannot get canvas width: [Id: {CanvasId}]")
					;

			var canvasHeight = await Canvas
				.GetHeightAsync(
					JSRuntime,
					CanvasId,
					_cancellationTokenSource.Token
				)
					?? throw new InvalidOperationException($"Cannot get canvas height: [Id: {CanvasId}]")
					;

			await Canvas.StrokeRectangleAsync(
				JSRuntime,
				CanvasId,
				1,
				1,
				canvasWidth - 2,
				canvasHeight - 2,
				lineWidth: 2,
				strokeStyle: _debugFillStyle,
				dashStyle: now.Second % 2 == 0
					? new[]
					{
						new KeyValuePair<double, double>(20, 3),
						new KeyValuePair<double, double>(3, 3),
						new KeyValuePair<double, double>(3, 3),
						new KeyValuePair<double, double>(3, 3)
					}
					: new[]
					{
						new KeyValuePair<double, double>(3, 3),
						new KeyValuePair<double, double>(3, 3),
						new KeyValuePair<double, double>(3, 3),
						new KeyValuePair<double, double>(20, 3)
					}
					,
				cancellationToken: CancellationToken
			);

			var xOffset = canvasWidth - 4d;
			var yOffset = 2d;

			CanvasTextMetrics textMetrics;

			var timestamp = now.ToString("O");

			textMetrics = await Canvas.MeasureTextAsync(
				JSRuntime,
				CanvasId,
				timestamp,
				font: DebugFont,
				cancellationToken: _cancellationTokenSource.Token
			);

			await Canvas.FillTextAsync(
				JSRuntime,
				CanvasId,
				timestamp,
				xOffset - textMetrics.Width,
				yOffset,
				font: DebugFont,
				fillStyle: _debugFillStyle,
				textAlignment: CanvasTextAlignment.Left,
				textBaseline: CanvasTextBaseline.Top,
				shadowColor: _debugShadowColor,
				shadowOffsetX: 1,
				shadowOffsetY: 1,
				cancellationToken: _cancellationTokenSource.Token
			);
			yOffset += textMetrics.ActualBoundingBoxAscent + 5;

			var fps = $"{RenderRate}fps";
			textMetrics = await Canvas.MeasureTextAsync(
				JSRuntime,
				CanvasId,
				fps,
				font: DebugFont,
				cancellationToken: _cancellationTokenSource.Token
			);

			await Canvas.FillTextAsync(
				JSRuntime,
				CanvasId,
				fps,
				xOffset - textMetrics.Width,
				yOffset,
				font: DebugFont,
				fillStyle: _debugFillStyle,
				textAlignment: CanvasTextAlignment.Left,
				textBaseline: CanvasTextBaseline.Top,
				shadowColor: _debugShadowColor,
				shadowOffsetX: 1,
				shadowOffsetY: 1,
				cancellationToken: _cancellationTokenSource.Token
			);
		}

#endif

		#region Input Event Handling

		#region Keyboard Event Handling

		protected internal virtual void OnCanvasKeyDown(KeyboardEventArgs e)
		{

		}

		protected internal virtual void OnCanvasKeyPress(KeyboardEventArgs e)
		{

		}

		protected internal virtual void OnCanvasKeyUp(KeyboardEventArgs e)
		{

		}

		#endregion

		#region Mouse Event Handling

		protected internal virtual void OnCanvasMouseDown(MouseEventArgs e)
		{

		}

		protected internal virtual void OnCanvasMouseMove(MouseEventArgs e)
		{

		}

		protected internal virtual void OnCanvasMouseOut(MouseEventArgs e)
		{

		}

		protected internal virtual void OnCanvasMouseOver(MouseEventArgs e)
		{

		}

		protected internal virtual void OnCanvasMouseUp(MouseEventArgs e)
		{

		}

		protected internal virtual void OnCanvasMouseWheel(WheelEventArgs e)
		{

		}

		#endregion

		#endregion

		#region GameEngine

		protected override async void OnRendering(RenderingEventArgs e)
		{

#if DEBUG

			//	Calculate debug text color using DateTime.Now
			//	
			//	Typically computing uses 16-bits to determine color.
			//	We often see hexadecimal equivalent representation of color on the web.
			//	e.g.: #0099FF, which is short hand for #FF0099FF
			//	
			//	Hexadecimal representation of color is #AARRGGBB, where:
			//	A - Alpha
			//	R - Red
			//	G - Green
			//	B - Blue
			//	
			//	A two digit hexadecimal can be as low as 00 (decimal = 0) or as high
			//	as FF (decimal = 255).
			//
			//	Using the example #FF0099FF, the color has:
			//	Alpha - 100%
			//	Red - 0%
			//	Green - 60%
			//	Blue - 100%
			//	
			//	The following will strip parts of the DateTime.Now to generate a color.

			//	Capture the last 8 bits (0xFF hexadecimal = 255 decimal = 8 bits).
			//	Convert those last 8 bits into a hexadecimal representation.
			var now = DateTime.Now;
			var red = 255 * now.Second / 60;

			//	Shift the entire number to the right 8 bits (which causes the previous 8
			//	to be lost) then capture the new last 8 bits.
			//	Convert those last 8 bits into a hexadecimal representation.
			var green = 255 * now.Minute / 60;

			//	Shift the entire number to the right 16 bits (which causes the previous 16
			//	to be lost) then capture the new last 8 bits.
			//	Convert those last 8 bits into a hexadecimal representation.
			var blue = 255 * now.Hour / 24;

			//	Concatenate the hexadecimal values.
			_debugFillStyle = $"#{red.ToString("X2")}{green.ToString("X2")}{blue.ToString("X2")}";
			_debugShadowColor = $"#{(255 - red).ToString("X2")}{(255 - green).ToString("X2")}{(255 - blue).ToString("X2")}";

#endif

			await Canvas.SyncScaleAsync(
				JSRuntime,
				CanvasId
			);

			await Canvas.ClearAsync(
				JSRuntime,
				CanvasId,
				_cancellationTokenSource.Token
			);
		}

		public override void Start()
		{
			base.Start();
		}

		public override async void Stop()
		{
			base.Stop();
		}

		protected override void Dispose(bool disposing)
		{
			_cancellationTokenSource.Dispose();

			// Invoke the base version of Dispose.
			base.Dispose(disposing);
		}

		#endregion

	}
}