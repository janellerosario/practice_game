using System;

namespace Engine
{
	///	<summary>
	///	Provides data for the <see cref="GameEngine.OnRendering(RenderingEventArgs)" /> method.
	///	</summary>
	public class RenderingEventArgs
		: UpdatingEventArgs
	{
		///	<summary>
		///	Initializes a new instance of the <see cref="RenderingEventArgs" /> class.
		///	</summary>
		///	<param name="frameId">
		///	The frame identification.
		///	</param>
		///	<param name="expiration">
		///	The frame expiration.
		///	</param>
		public RenderingEventArgs(
			int frameId,
			DateTime expiration
		)
			: base(
				frameId,
				expiration
			)
		{

		}
	}
}