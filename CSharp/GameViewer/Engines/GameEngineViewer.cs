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

		protected string CanvasId { get; }

		protected IJSRuntime JSRuntime { get; }

		protected CancellationToken CancellationToken
			=> _cancellationTokenSource.Token;

		protected int FrameRate { get; private set; }

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

			var canvasWidth = await Canvas
				.GetWidthAsync(
					JSRuntime,
					CanvasId,
					_cancellationTokenSource.Token
				)
					?? throw new InvalidOperationException($"Cannot get canvas width: [Id: {CanvasId}]");
			var xOffset = canvasWidth - 2d;
			var yOffset = 2d;

			CanvasTextMetrics textMetrics;

			var timestamp = DateTime
				.Now
				.ToString("O")
				;
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
				textAlignment: CanvasTextAlignment.Left,
				textBaseline: CanvasTextBaseline.Top,
				cancellationToken: _cancellationTokenSource.Token
			);
			yOffset += textMetrics.ActualBoundingBoxAscent + 5;

			var fps = $"{FrameRate}fps";
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
				textAlignment: CanvasTextAlignment.Left,
				textBaseline: CanvasTextBaseline.Top,
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
			var now = DateTime.Now;
			var duration = now - (_lastRenderTimestamp ?? now.AddSeconds(-1));
			_lastRenderTimestamp = now;

			FrameRate = (int)(2 / duration.TotalSeconds);

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