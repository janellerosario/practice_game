namespace GameViewer.JSInteroperability
{
	///	<summary>
	///	Defines text baseline used when drawing text.
	///	
	///	</summary>
	///	<seealso>
	///	[MDN web docs - CanvasRenderingContext2D.textBaseline](https://developer.mozilla.org/en-US/docs/Web/API/CanvasRenderingContext2D/textBaseline)
	///	</seealso>
	public enum CanvasTextBaseline
	{
		///	<summary>
		///	The text baseline is the normal alphabetic baseline
		///	</summary>
		Alphabetic,

		///	<summary>
		///	The text baseline is ideographic baseline; this is the bottom
		///	of the body of the character, if the main body of characters
		///	protrudes beneath the alphabetic baseline.
		///	(Used by Chinese, Japanese, and Korean scripts.)
		///	</summary>
		Ideographic,

		///	<summary>
		///	The text baseline is the top of the em square.
		///	</summary>
		Top,

		///	<summary>
		///	The text baseline is the bottom of the bounding box.  This differs
		///	from the ideographic baseline in that the ideographic baseline doesn't
		///	consider descenders.
		///	</summary>
		Bottom,

		///	<summary>
		///	The text baseline is the middle of the em square.
		///	</summary>
		Middle,

		///	<summary>
		///	The text baseline is the hanging baseline.
		///	(Used by Tibetan and other Indic scripts.)
		///	</summary>
		Hanging,
	}
}