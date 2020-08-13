(function () {
	"use strict";

	const Namespace = "GameViewer.JSInteroperability";
	const Class = "Canvas";

	if (GameViewer == undefined)
		throw new Error("GameViewer library not found!");

	const _dataTypes = GameViewer.DataTypes;

	function GetContextByCanvasId(canvasId) {
		if (!_dataTypes.Is(canvasId, _dataTypes.String))
			throw new GameViewer.UnexpectedDataTypeException("canvasId");

		var canvas = document.getElementById(canvasId);
		if (canvas == null)
			return null;

		return canvas.getContext("2d");
	}

	var _syncArray = [];

	class Canvas {
		constructor() {
			throw new GameViewer.CannotInstantiateStaticClassException(Class);
		}

		static Clear(canvasId) {
			if (!_dataTypes.Is(canvasId, _dataTypes.String))
				throw new GameViewer.UnexpectedDataTypeException("canvasId");

			var context = GetContextByCanvasId(canvasId);
			if (context == null)
				return;

			var width = context
				.canvas
				.width
				;
			var height = context
				.canvas
				.height
				;

			context.clearRect(0, 0, width, height);
		}

		static GetMousePosition(canvasId, clientX, clientY) {
			if (!_dataTypes.Is(canvasId, _dataTypes.String))
				throw new GameViewer.UnexpectedDataTypeException("canvasId");

			if (!_dataTypes.Is(clientX, _dataTypes.Number))
				throw new GameViewer.UnexpectedDataTypeException("clientX");

			if (!_dataTypes.Is(clientY, _dataTypes.Number))
				throw new GameViewer.UnexpectedDataTypeException("clientY");

			var context = GetContextByCanvasId(canvasId);
			if (context == null)
				return;

			var rectangle = context
				.canvas
				.getBoundingClientRect()
				;

			return JSON.stringify({
				x: clientX - rectangle.left,
				y: clientY - rectangle.top
			});
		}

		static StrokeCircle(
			canvasId,
			x,
			y,
			radius,
			start,
			end,
			counterClockwise,
			lineWidth,
			strokeStyle,
			dashStyle
		) {
			if (!_dataTypes.Is(canvasId, _dataTypes.String))
				throw new GameViewer.UnexpectedDataTypeException("canvasId");

			if (!_dataTypes.Is(x, _dataTypes.Number))
				throw new GameViewer.UnexpectedDataTypeException("x");

			if (!_dataTypes.Is(y, _dataTypes.Number))
				throw new GameViewer.UnexpectedDataTypeException("y");

			if (!_dataTypes.Is(radius, _dataTypes.Number))
				throw new GameViewer.UnexpectedDataTypeException("radius");

			if (!_dataTypes.Is(start, _dataTypes.Number))
				throw new GameViewer.UnexpectedDataTypeException("start");

			if (!_dataTypes.Is(end, _dataTypes.Number))
				throw new GameViewer.UnexpectedDataTypeException("end");

			var context = GetContextByCanvasId(canvasId);
			if (context == null)
				return;

			if (_dataTypes.Is(lineWidth, _dataTypes.Number))
				context.lineWidth = lineWidth;

			if (_dataTypes.Is(strokeStyle, _dataTypes.String))
				context.strokeStyle = strokeStyle;

			if (Array.isArray(dashStyle))
				context.setLineDash(dashStyle);

			context.beginPath();
			context.arc(x, y, radius, start, end, counterClockwise);
			context.stroke();
		}

		static StrokeRectangle(
			canvasId,
			x,
			y,
			width,
			height,
			lineWidth,
			strokeStyle,
			dashStyle
		) {
			if (!_dataTypes.Is(canvasId, _dataTypes.String))
				throw new GameViewer.UnexpectedDataTypeException("canvasId");

			if (!_dataTypes.Is(x, _dataTypes.Number))
				throw new GameViewer.UnexpectedDataTypeException("x");

			if (!_dataTypes.Is(y, _dataTypes.Number))
				throw new GameViewer.UnexpectedDataTypeException("y");

			if (!_dataTypes.Is(width, _dataTypes.Number))
				throw new GameViewer.UnexpectedDataTypeException("width");

			if (!_dataTypes.Is(height, _dataTypes.Number))
				throw new GameViewer.UnexpectedDataTypeException("height");

			var context = GetContextByCanvasId(canvasId);
			if (context == null)
				return;

			if (_dataTypes.Is(lineWidth, _dataTypes.Number))
				context.lineWidth = lineWidth;

			if (_dataTypes.Is(strokeStyle, _dataTypes.String))
				context.strokeStyle = strokeStyle;

			if (Array.isArray(dashStyle))
				context.setLineDash(dashStyle);

			context.strokeRect(x, y, width, height);
		}

		static FillText(
			canvasId,
			text,
			x,
			y,
			font,
			fillStyle,
			textAlign,
			textBaseline,
			shadowColor,
			shadowOffsetX,
			shadowOffsetY
		) {
			if (!_dataTypes.Is(canvasId, _dataTypes.String))
				throw new GameViewer.UnexpectedDataTypeException("canvasId");

			if (!_dataTypes.Is(text, _dataTypes.String))
				throw new GameViewer.UnexpectedDataTypeException("text");

			if (!_dataTypes.Is(x, _dataTypes.Number))
				throw new GameViewer.UnexpectedDataTypeException("x");

			if (!_dataTypes.Is(y, _dataTypes.Number))
				throw new GameViewer.UnexpectedDataTypeException("y");

			var context = GetContextByCanvasId(canvasId);
			if (context == null)
				return;

			if (_dataTypes.Is(font, _dataTypes.String))
				context.font = font;

			if (_dataTypes.Is(fillStyle, _dataTypes.String))
				context.fillStyle = fillStyle;

			if (_dataTypes.Is(textAlign, _dataTypes.String))
				context.textAlign = textAlign;

			if (_dataTypes.Is(textBaseline, _dataTypes.String))
				context.textBaseline = textBaseline;

			if (_dataTypes.Is(shadowColor, _dataTypes.String))
				context.shadowColor = shadowColor;

			if (_dataTypes.Is(shadowOffsetX, _dataTypes.Number))
				context.shadowOffsetX = shadowOffsetX;

			if (_dataTypes.Is(shadowOffsetY, _dataTypes.Number))
				context.shadowOffsetY = shadowOffsetY;

			context.fillText(text, x, y);
		}

		static MeasureText(canvasId, text, font) {
			if (!_dataTypes.Is(canvasId, _dataTypes.String))
				throw new GameViewer.UnexpectedDataTypeException("canvasId");

			if (!_dataTypes.Is(text, _dataTypes.String))
				throw new GameViewer.UnexpectedDataTypeException("text");

			var context = GetContextByCanvasId(canvasId);
			if (context == null)
				return null;

			if (_dataTypes.Is(font, _dataTypes.String))
				context.font = font;

			var result = context.measureText(text);
			var poco = GameViewer
				.Object
				.ToPoco(result)
				;

			var json = JSON.stringify(poco);

			return json;
		}

		static GetTextAlignment(canvasId) {
			if (!_dataTypes.Is(canvasId, _dataTypes.String))
				throw new GameViewer.UnexpectedDataTypeException("canvasId");

			var context = GetContextByCanvasId(canvasId);
			if (context == null)
				return null;

			return context.textAlign;
		}

		static SetTextAlignment(canvasId, value) {
			if (!_dataTypes.Is(canvasId, _dataTypes.String))
				throw new GameViewer.UnexpectedDataTypeException("canvasId");

			if (!_dataTypes.Is(value, _dataTypes.String))
				throw new GameViewer.UnexpectedDataTypeException("value");

			var context = GetContextByCanvasId(canvasId);
			if (context == null)
				return;

			context.textAlign = value;
		}

		static GetWidth(canvasId) {
			if (!_dataTypes.Is(canvasId, _dataTypes.String))
				throw new GameViewer.UnexpectedDataTypeException("canvasId");

			var context = GetContextByCanvasId(canvasId);
			if (context == null)
				return null;

			return context
				.canvas
				.width
				;
		}

		static GetHeight(canvasId) {
			if (!_dataTypes.Is(canvasId, _dataTypes.String))
				throw new GameViewer.UnexpectedDataTypeException("canvasId");

			var context = GetContextByCanvasId(canvasId);
			if (context == null)
				return null;

			return context
				.canvas
				.height
				;
		}

		static SyncScale(canvasId) {
			if (!_dataTypes.Is(canvasId, _dataTypes.String))
				throw new GameViewer.UnexpectedDataTypeException("canvasId");

			var canvas = document.getElementById(canvasId);
			if (canvas == null)
				return null;

			canvas.width = canvas.offsetWidth;
			canvas.height = canvas.offsetHeight;
		}
	}

	GameViewer.BindToNamespace(
		Namespace,
		Class,
		Canvas
	);
})();