using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Engine;
using Engine.Mathematics;
using GameViewer.JSInteroperability;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace GameViewer.Engines
{
	public class UnloadedGameEngineViewer
		: GameEngineViewer
	{
		private const string Font
			= "36px 'Times New Roman'";

		private Dictionary<string, KeyValuePair<string, Point2D>> _keyPressDictionary
			= new Dictionary<string, KeyValuePair<string, Point2D>>();

		private Queue<KeyValuePair<DateTime, Point2D>> _mouseTrails
			= new Queue<KeyValuePair<DateTime, Point2D>>();

		public UnloadedGameEngineViewer(
			GameEngineOptions options,
			IJSRuntime jsRuntime,
			string canvasId
		)
			: base(
				options,
				jsRuntime,
				canvasId
			)
		{
			Console.WriteLine($"Instantiated {nameof(UnloadedGameEngineViewer)}");
			Console.WriteLine($"\t{nameof(options.FrameRate)}: {options.FrameRate}");
		}

		#region GameEngineViewer

		protected internal override async void OnCanvasKeyDown(KeyboardEventArgs e)
		{
			base.OnCanvasKeyUp(e);

			if (_keyPressDictionary.ContainsKey(e.Code))
				return;

			var width = await Canvas.GetWidthAsync(
				JSRuntime,
				CanvasId,
				CancellationToken
			)
				?? throw new InvalidOperationException("Cannot retrieve canvas width.")
				;

			var height = await Canvas.GetHeightAsync(
				JSRuntime,
				CanvasId,
				CancellationToken
			)
				?? throw new InvalidOperationException("Cannot retrieve canvas height.")
				;

			var keyText = e.Code;

			var textMetrics = await Canvas.MeasureTextAsync(
				JSRuntime,
				CanvasId,
				keyText,
				font: Font,
				cancellationToken: CancellationToken
			);

			width -= textMetrics.Width;
			height -= textMetrics.ActualBoundingBoxAscent;

			var random = new Random();
			var randomPoint = new Point2D
			{
				X = width * random.NextDouble(),
				Y = height * random.NextDouble()
			};

			_keyPressDictionary.Add(
				e.Code,
				new KeyValuePair<string, Point2D>(
					keyText,
					randomPoint
				)
			);
		}

		protected internal override void OnCanvasKeyUp(KeyboardEventArgs e)
		{
			if (!_keyPressDictionary.ContainsKey(e.Code))
				return;

			Console.WriteLine($"{nameof(OnCanvasKeyUp)}: {e.Code}");
			_keyPressDictionary.Remove(e.Code);
		}

		protected internal override async void OnCanvasMouseMove(MouseEventArgs e)
		{
			const int Threshold
				= 30;

			var point = await Canvas.GetMousePosition(
				JSRuntime,
				CanvasId,
				e.ClientX,
				e.ClientY,
				cancellationToken: CancellationToken
			);

			_mouseTrails.Enqueue(
				new KeyValuePair<DateTime, Point2D>(
					DateTime.Now,
					new Point2D
					{
						X = point.X,
						Y = point.Y
					}
				)
			);

			if (_mouseTrails.Count > Threshold)
				_mouseTrails.Dequeue();
		}

		protected override async void OnRendering(RenderingEventArgs e)
		{
			base.OnRendering(e);

			var width = await Canvas.GetWidthAsync(
				JSRuntime,
				CanvasId,
				cancellationToken: CancellationToken
			)
				?? throw new InvalidOperationException("Cannot retrieve canvas width.")
				;


			var height = await Canvas.GetHeightAsync(
				JSRuntime,
				CanvasId,
				cancellationToken: CancellationToken
			)
				?? throw new InvalidOperationException("Cannot retrieve canvas height.")
				;

			//	Render key press
			await RenderKeyPressAsync();

			//	Render mouse
			await RenderMouseAsync();
		}

		protected async Task RenderKeyPressAsync()
		{
			foreach (var kvp in _keyPressDictionary)
			{
				var x = kvp
					.Value
					.Value
					.X
					;
				var y = kvp
					.Value
					.Value
					.Y
					;
				var text = kvp
					.Value
					.Key
					;

				await Canvas.FillTextAsync(
					JSRuntime,
					CanvasId,
					text,
					x,
					y,
					font: Font,
					textBaseline: CanvasTextBaseline.Top,
					textAlignment: CanvasTextAlignment.Left,
					cancellationToken: CancellationToken
				);
			}
		}

		protected async Task RenderMouseAsync()
		{
			//	Cursor size
			const double CursorSize
				= 2.5;

			//	Five seconds.
			var fadeThreshold = new TimeSpan(0, 0, 10);

			var now = DateTime.Now;

			foreach (var mouseTrail in _mouseTrails)
			{
				var duration = now - mouseTrail.Key;

				if (duration > fadeThreshold)
					continue;

				var alpha = ((int)(255d * duration.Ticks / fadeThreshold.Ticks))
					.ToString("X2")
					;
				var strokeStyle = (now.Second % 3) switch
				{
					0 => $"#FF{alpha}{alpha}",
					1 => $"#{alpha}FF{alpha}",
					_ => $"#{alpha}{alpha}FF"
				};

				await Canvas.StrokeCircleAsync(
					JSRuntime,
					CanvasId,
					mouseTrail
						.Value
						.X
						,
					mouseTrail
						.Value
						.Y
						,
					CursorSize,
					lineWidth: CursorSize / 2,
					strokeStyle: strokeStyle,
					cancellationToken: CancellationToken
				);
			}
		}

		protected override void OnUpdating(UpdatingEventArgs e)
		{

		}

		public override void Start()
		{
			base.Start();
		}

		public override void Stop()
		{
			base.Stop();
		}

		#endregion

	}
}