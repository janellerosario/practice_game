using System;
using System.Text.Json.Serialization;

namespace GameViewer.JSInteroperability
{
	///	<summary>
	///	A class representing the dimensions of a piece of text in the canvas.
	///	<summary>
	///	<seealso>
	///	[MDN web docs - TextMetrics](https://developer.mozilla.org/en-US/docs/Web/API/TextMetrics)
	///	</seealso>
	public class CanvasTextMetrics
	{
		///	<summary>
		///	The calculated width of a segment of inline text in CSS pixels.
		///	It takes into account the current font of the context.
		///	</summary>
		///	<value>
		///	A <see cref="double" /> of the calculated width.
		///	</value>
		public double Width { get; set; }

		///	<summary>
		///	The distance from the alignment point given by the
		///	<see cref="Canvas.GetTextAlignmentAsync(Microsoft.JSInterop.IJSRuntime, string, System.Threading.CancellationToken)" /> method
		///	to the left side of the bounding rectangle of the given text, in CSS pixels.
		///	The distance is measured parallel to the baseline.
		///	</summary>
		///	<value>
		///	A <see cref="double" /> of the calculated distance of the actual left bounding box.
		///	</value>
		public double ActualBoundingBoxLeft { get; set; }

		///	<summary>
		///	The distance from the alignment point given by the
		///	<see cref="Canvas.GetTextAlignmentAsync(Microsoft.JSInterop.IJSRuntime, string, System.Threading.CancellationToken)" /> method
		///	to the right side of the bounding rectangle of the given text, in CSS pixels.
		///	The distance is measured parallel to the baseline.
		///	</summary>
		///	<value>
		///	A <see cref="double" /> of the calculated distance of the actual right bounding box.
		///	</value>
		public double ActualBoundingBoxRight { get; set; }

		///	<summary>
		///	The distance from the horizontal line indicated by the 
		///	<see cref="Canvas.GetTextBaselineAsync(Microsoft.JSInterop.IJSRuntime, string, System.Threading.CancellationToken)" /> method
		///	to the top of the highest bounding rectangle of all of the founts used to render the text,
		///	in CSS pixels.
		///	</summary>
		///	<value>
		///	A <see cref="double" /> of the calculated distance of the font ascent bounding box.
		///	</value>
		public double? FontBoundingBoxAscent { get; set; }

		///	<summary>
		///	The distance from the horizontal line indicated by the 
		///	<see cref="Canvas.GetTextBaselineAsync(Microsoft.JSInterop.IJSRuntime, string, System.Threading.CancellationToken)" /> method
		///	to the bottom of the bounding rectangle of all of the founts used to render the text,
		///	in CSS pixels.
		///	</summary>
		///	<value>
		///	A <see cref="double" /> of the calculated distance of the font descent bounding box.
		///	</value>
		public double? FontBoundingBoxDescent { get; set; }

		///	<summary>
		///	The distance from the horizontal line indicated by the
		///	<see cref="Canvas.GetTextBaselineAsync(Microsoft.JSInterop.IJSRuntime, string, System.Threading.CancellationToken)" /> method
		///	to the top of the bounding rectangle used to render the text, in CSS pixels.
		///	</summary>
		///	<value>
		///	A <see cref="double" /> of the calculated distance of the actual ascent bounding box.
		///	</value>
		public double ActualBoundingBoxAscent { get; set; }

		///	<summary>
		///	The distance from the horizontal line indicated by the
		///	<see cref="Canvas.GetTextBaselineAsync(Microsoft.JSInterop.IJSRuntime, string, System.Threading.CancellationToken)" /> method
		///	to the bottom of the bounding rectangle used to render the text, in CSS pixels.
		///	</summary>
		///	<value>
		///	A <see cref="double" /> of the calculated distance of the actual descent bounding box.
		///	</value>
		public double ActualBoundingBoxDescent { get; set; }

		///	<summary>
		///	The distance from the horizontal line indicated by the
		///	<see cref="Canvas.GetTextBaselineAsync(Microsoft.JSInterop.IJSRuntime, string, System.Threading.CancellationToken)" /> method
		///	to the top of the *em* square in the line box, in CSS pixels.
		///	</summary>
		///	<value>
		///	A <see cref="double" /> of the calculated distance of the *em* hight ascent.
		///	</value>
		public double? EmHeightAscent { get; set; }

		///	<summary>
		///	The distance from the horizontal line indicated by the
		///	<see cref="Canvas.GetTextBaselineAsync(Microsoft.JSInterop.IJSRuntime, string, System.Threading.CancellationToken)" /> method
		///	to the bottom of the *em* square in the line box, in CSS pixels.
		///	</summary>
		///	<value>
		///	A <see cref="double" /> of the calculated distance of the *em* hight descent.
		///	</value>
		public double? EmHeightDescent { get; set; }

		///	<summary>
		///	The distance from the horizontal line indicated by the 
		///	<see cref="Canvas.GetTextBaselineAsync(Microsoft.JSInterop.IJSRuntime, string, System.Threading.CancellationToken)" /> method
		///	to the hanging baseline of the line box, in CSS pixels.
		///	</summary>
		///	<value>
		///	A <see cref="double" /> of the calculated distance of the hanging baseline.
		///	</value>
		public double? HangingBaseline { get; set; }

		///	<summary>
		///	The distance from the horizontal line indicated by the 
		///	<see cref="Canvas.GetTextBaselineAsync(Microsoft.JSInterop.IJSRuntime, string, System.Threading.CancellationToken)" /> method
		///	to the alphabetic baseline of the line box, in CSS pixels.
		///	</summary>
		///	<value>
		///	A <see cref="double" /> of the calculated distance of the alphabetic baseline.
		///	</value>
		public double? AlphabeticBaseline { get; set; }

		///	<summary>
		///	The distance from the horizontal line indicated by the 
		///	<see cref="Canvas.GetTextBaselineAsync(Microsoft.JSInterop.IJSRuntime, string, System.Threading.CancellationToken)" /> method
		///	to the ideographic baseline of the line box, in CSS pixels.
		///	</summary>
		///	<value>
		///	A <see cref="double" /> of the calculated distance of the ideographic baseline.
		///	</value>
		public double? IdeographicBaseline { get; set; }
	}
}