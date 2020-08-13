using System;
using System.Threading;
using System.Threading.Tasks;

namespace Engine
{
	///	<summary>
	///	Represents a game engine.
	///	</summary>
	///	<remarks>
	///	<see cref="GameEngine" /> is used by derrived types to sychronize game logic by execution of frames.
	///	</remarks>
	public abstract class GameEngine
		: IDisposable
	{
		private GameEngineOptions _options;

		private CancellationTokenSource _cts;

		///	<summary>
		///	Instantiates a new instance of <see cref="GameEngine" />.
		///	</summary>
		///	<param name="options">
		///	The options used to instantiate this instance of <see cref="GameEngine" />.
		///	</param>
		protected GameEngine(GameEngineOptions options)
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

#if DEBUG

					RenderDebugInformation();

#endif

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
		///	Invoked when <see cref="GameEngine" /> is updating a frame.
		///	</summary>
		protected abstract void OnUpdating(UpdatingEventArgs e);

		///	<summary>
		///	Invoked when <see cref="GameEngine" /> is rendering a frame.
		///	</summary>
		protected abstract void OnRendering(RenderingEventArgs e);

#if DEBUG

		protected virtual void RenderDebugInformation()
		{

		}

#endif

		///	<summary>
		///	Causes the <see cref="GameEngine" /> to begin its loop.
		///	</summary>
		public virtual void Start()
		{
			if (_cts != null)
				return;

			//	Creates a token to use to cancel the loop.
			_cts = new CancellationTokenSource();

			//	Start the loop on a separate thread.
			//	The 'Loop' method is executed on its own thread, which allows this thread to resume after
			//	it executes the 'Loop' thread.
			//	See (Managed Threading)[https://docs.microsoft.com/en-us/dotnet/standard/threading/]
			Task.Run(Loop, _cts.Token);
		}

		///	<summary>
		///	Causes the <see cref="GameEngine" /> to end its loop.
		///	</summary>
		public virtual void Stop()
		{
			if (_cts == null)
				return;

			//	Informs the game loop to stop looping.
			_cts.Cancel();
		}

		#region IDisposable

		private bool disposedValue;

		///	<summary>
		///	Disposes this instance of <see cref="GameEngine" /> and associated resources.
		///	</summary>
		///	<remarks>
		///	Derived types should override this method if it has resources to dispose of.
		///	</remarks>
		///	<param name="disposing">
		///	<c>true</c> if disposing managed state as well as unmanaged resources; otherwise
		///	<c>false</c> if disposing only unmanaged resources.
		///	</param>
		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects)
				}

				// TODO: free unmanaged resources (unmanaged objects) and override finalizer
				// TODO: set large fields to null
				disposedValue = true;
			}
		}

		// // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
		// ~GameEngine()
		// {
		//     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		//     Dispose(disposing: false);
		// }

		///	<summary>
		///	Disposes this instance of <see cref="GameEngine" /> and associated resources.
		///	</summary>
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		#endregion
	}
}