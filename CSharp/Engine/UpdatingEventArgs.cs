using System;

namespace Engine
{
	///	<summary>
	///	Provides data for the <see cref="GameEngine.OnUpdating(UpdatingEventArgs)" /> method.
	///	</summary>
	public class UpdatingEventArgs
		: EventArgs
	{
		///	<summary>
		///	Gets the frame identification.
		///	</summary>
		///	<value>
		///	An integer value identifying the frame.
		///	</value>
		public int FrameId { get; }

		///	<summary>
		///	Gets the expiration of this frame.
		///	</summary>
		///	<value>
		///	A <see cref="DateTime" /> representing the expiration of this frame.
		///	</value>
		public DateTime Expiration { get; }

		///	<summary>
		///	Initializes a new instance of the <see cref="UpdatingEventArgs" /> class.
		///	</summary>
		///	<param name="frameId">
		///	The frame identification.
		///	</param>
		///	<param name="expiration">
		///	The frame expiration.
		///	</param>
		public UpdatingEventArgs(
			int frameId,
			DateTime expiration
		)
		{
			FrameId = frameId;
			Expiration = expiration;
		}
	}
}