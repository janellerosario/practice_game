using System;
using System.Threading.Tasks;
using Engine.Tests.Engines;
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
			var engine = new BareMinimumEngine(
				options,
				//	On every frame update, increment the count.
				e => updateCount++,
				//	On every frame render, increment the count.
				e => renderCount++
			);

			//	Start the engine.
			engine.Start();

			//	Delay for one second.
			await Task.Delay(1000);

			//	Stop the engine.
			engine.Stop();

			_output.WriteLine($"Update count: {updateCount}");
			_output.WriteLine($"Render count: {renderCount}");

			//	Tests passes if both 'Updating' and 'Rendering' events
			//	are triggered at the specified frame rate.
			//	A padding of five frames is allowed.
			Assert.True(
				updateCount >= options.FrameRate
				&& updateCount < options.FrameRate + 5
				&& renderCount >= options.FrameRate
				&& renderCount < options.FrameRate + 5
			);
		}
	}
}
