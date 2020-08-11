using System;

namespace Engine
{
	public class RenderingEventArgs
		: UpdatingEventArgs
	{
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