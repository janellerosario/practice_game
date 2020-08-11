using System;
using System.Threading;
using System.Threading.Tasks;

namespace Engine
{
	public class GameEngine
	{
		private GameEngineOptions _options;

		private CancellationTokenSource _cts;

		///	<summary>
		///	Occurs when the engine is rendering a frame.
		///	</summary>
		public event EventHandler<RenderingEventArgs> Rendering;

		///	<summary>
		///	Occurs when the engine is updating frame.
		///	</summary>
		public event EventHandler<UpdatingEventArgs> Updating;

		public GameEngine(GameEngineOptions options)
		{
			_options = options
				?? throw new ArgumentNullException(nameof(options));
		}

		private async Task Loop()
		{
			//	Determine the length of time, in milliseconds, allowed per frame.
			var millisecondsPerFrame = 1000 / _options.FrameRate;

			//	Get the current moment in time.
			var sync = DateTime.Now;
			var frameId = 0;

			while (!_cts.IsCancellationRequested)
			{
				unchecked
				{
					//	'unchecked' keyword suppresses overflow-checking for
					//	integral-type arithmetic operations and conversions.
					//	
					//	In this scenario, the game loop can run indefinitely,
					//	so the increment of 'frameId' can theoretically go from
					//	0 to maximum of its data type (int.MAX_VALUE = 2,147,483,647),
					//	in which the next increment would throw a 'OverflowException'.
					//	
					//	What unchecked does in this case is cause the arithmetic 
					//	to loop around from its maximum to minimum value and vice versa.
					//
					//	So, for example, if 'frameId == int.MAX_VALUE' then
					//	'frameId + 50 == int.MIN_VALUE + 49'.
					frameId++;
				}

				//	'Then' is the moment in time now before any computation within this frame.
				var then = DateTime.Now;
				var expiration = then.AddMilliseconds(millisecondsPerFrame);
				//	Dispatch the 'Updating' event.
				var updatingEvent = new UpdatingEventArgs(
					frameId,
					expiration
				);
				OnUpdating(updatingEvent);

				//	'UpdateNow' is the moment in time now after updates of objects in this engine.
				var updateNow = DateTime.Now;

				//	Determine the time span between 'then' and 'now'.
				var offset = updateNow - then;

				//	Determine if this frame exceeded its time or threshold.
				if (
					offset.TotalMilliseconds <= millisecondsPerFrame
					&& offset.TotalMilliseconds <= (_options.FrameRenderIgnoreThreshold ?? 1) * millisecondsPerFrame
				)
				{
					//	Since this frame completed within the allowed time,
					//	Dispatch the 'Rendering' event.
					var renderingEvent = new RenderingEventArgs(
						frameId,
						expiration
					);
					OnRendering(renderingEvent);
				}

				var syncOffset = updateNow - sync;
				var msOffset = (int)syncOffset.TotalMilliseconds % millisecondsPerFrame;

				//	Delay this thread for the remaining time required to remain in sync with 'sync'.
				await Task.Delay(millisecondsPerFrame - msOffset, _cts.Token);
			}

			//	Destroy all resources used with the cancellation token source.
			_cts.Dispose();
			_cts = null;
		}

		///	<summary>
		///	Invokes <see cref="Updating" /> whenever <see cref="GameEngine" /> is updating a frame.
		///	</summary>
		protected void OnUpdating(UpdatingEventArgs e)
			=> Updating?.Invoke(this, e);

		///	<summary>
		///	Invokes <see cref="Rendering" /> whenever <see cref="GameEngine" /> is rendering a frame.
		///	</summary>
		protected void OnRendering(RenderingEventArgs e)
			=> Rendering?.Invoke(this, e);

		///	<summary>
		///	Causes the <see cref="GameEngine" /> to begin its loop.
		///	</summary>
		public void Start()
		{
			if (_cts != null)
				return;

			//	Creates a token to use to cancel the loop.
			_cts = new CancellationTokenSource();

			//	Start the loop on a separate thread.
			//	The 'Loop' method is executed on its own thread, which allows this thread to resume after
			//	it executes the 'Loop' thread.
			//	See (Managed Threading)[https://docs.microsoft.com/en-us/dotnet/standard/threading/]
			Task.Run(Loop);
		}

		///	<summary>
		///	Causes the <see cref="GameEngine" /> to end its loop.
		///	</summary>
		public void Stop()
		{
			if (_cts == null)
				return;

			//	Informs the game loop to stop looping.
			_cts.Cancel();
		}
	}
}