using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Engine.Tests
{
	///	<summary>
	///	Tests for <see cref="GameEngine" />.
	///	</summary>
	public class GameEngineTests
	{
		private const int DefaultFrameRate
			= 30;

		private const int DoubleFrameRate
			= DefaultFrameRate * 2;

		private const int QuadrupleFrameRate
			= DoubleFrameRate * 2;

		private readonly ITestOutputHelper _output;

		///	<summary>
		///	Initializes a new instance of the <see cref="GameEngineTests" /> class.
		///	</summary>
		///	<param name="output">
		///	The output helper.
		///	</param>
		public GameEngineTests(ITestOutputHelper output)
		{
			_output = output;
		}

		///	<summary>
		///	Tests the <see cref="GameEngine" /> loop.
		///	</summary>
		[Fact]
		public async Task GameLoopExecutionTest()
		{
			var options = new GameEngineOptions
			{
				FrameRate = DefaultFrameRate
			};

			var updateCount = 0;
			var renderCount = 0;
			var engine = new GameEngine(options);

			//	Add an event handler for the updating event.
			engine.Updating += (sender, e)
				=> updateCount++;

			//	Add an event handler for the rendering event.
			engine.Rendering += (sender, e)
				=> renderCount++;

			//	Start the engine.
			engine.Start();

			//	Delay for one second.
			await Task.Delay(1000);

			//	Stop the engine.
			engine.Stop();

			_output.WriteLine($"Update count: {updateCount}");
			_output.WriteLine($"Render count: {renderCount}");

			Assert.True(
				updateCount >= options.FrameRate
				&& updateCount < options.FrameRate + 5
				&& renderCount >= options.FrameRate
				&& renderCount < options.FrameRate + 5
			);
		}
	}
}
