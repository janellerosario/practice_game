using System;
using System.Threading;
using System.Threading.Tasks;

namespace Engine
{
	///	<summary>
	///	Defines options used for <see cref="GameEngine" />.
	///	</summary>
	public class GameEngineOptions
	{
		private int _frameRate;
		private float? _frameRenderIgnoreThreshold;

		///	<summary>
		///	Gets/Sets the frame rate of the game engine in frames per second.
		///	</summary>
		///	<value>
		///	An integer representing the frame rate in frames per second.
		///	</value>
		public int FrameRate
		{
			get
				=> _frameRate;
			set
			{
				if (value < 0)
					throw new InvalidOperationException($"{nameof(FrameRate)} must be greater than zero.");

				_frameRate = value;
			}
		}

		///	<summary>
		///	Gets/Sets the threshold in which to ignore frame rendering.
		///	</summary>
		///	<value>
		///	An percentage representing the threshold in which to ignore frame rendering;
		///	otherwise, <c>null</c>.
		///	</value>
		///	<remarks>
		///	If a value is set and the remaining duration percentage of the frame
		///	is less than the percentage of this value then frame rendering will be ignored.
		///	</remarks>
		public float? FrameRenderIgnoreThreshold
		{
			get
				=> _frameRenderIgnoreThreshold;
			set
			{
				if (
					value < 0
					|| value > 1
				)
					throw new InvalidOperationException($"{nameof(FrameRenderIgnoreThreshold)} must be between 0 and 1.");

				_frameRenderIgnoreThreshold = value;
			}
		}
	}
}