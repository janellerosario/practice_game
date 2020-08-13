namespace GameViewer.JSInteroperability
{
	///	<summary>
	///	Defines text alignment used when drawing text.
	///	
	///	</summary>
	///	<seealso>
	///	[MDN web docs - CanvasRenderingContext2D.textAlign](https://developer.mozilla.org/en-US/docs/Web/API/CanvasRenderingContext2D/textAlign)
	///	</seealso>
	public enum CanvasTextAlignment
	{
		///	<summary>
		///	The text is aligned at the normal start of the line (left-aligned for
		///	left-toright locales, right-aligned for right-to-left locales).
		///	</summary>
		Start,

		///	<summary>
		///	The text is aligned at the normal end of the line (right-aligned for
		///	left-toright locales, left-aligned for right-to-left locales).
		///	</summary>

		End,
		///	<summary>
		///	The text is left-aligned.
		///	</summary>
		Left,

		///	<summary>
		///	The text is right-aligned.
		///	</summary>
		Right,

		///	<summary>
		///	The text is centered.
		///	</summary>
		Center
	}
}