using System;
using System.Collections.Generic;
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
			= "12px 'Comic Sans MS'";

		private Dictionary<string, KeyValuePair<string, Point2D>> _keyPressDictionary
			= new Dictionary<string, KeyValuePair<string, Point2D>>();

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

			// var textMetrics = await Canvas.MeasureTextAsync(
			// 	JSRuntime,
			// 	CanvasId,
			// 	keyText,
			// 	font: Font,
			// 	cancellationToken: CancellationToken
			// );

			// width -= textMetrics.Width;
			// height -= textMetrics.ActualBoundingBoxAscent;

			var someWidth = width * 0.50d;
			var someHeight = height * 0.50d;

			width -= someWidth / 2;
			height -= someHeight / 2;

			var random = new Random();
			var randomPoint = new Point2D
			{
				X = someWidth + width * random.NextDouble(),
				Y = someHeight + height * random.NextDouble()
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

		protected override async void OnRendering(RenderingEventArgs e)
		{
			base.OnRendering(e);

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