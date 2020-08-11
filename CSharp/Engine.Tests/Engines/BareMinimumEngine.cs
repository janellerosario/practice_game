using System;

namespace Engine.Tests.Engines
{
	internal class BareMinimumEngine
		: GameEngine
	{
		private readonly Action<RenderingEventArgs> _renderingAction;
		private readonly Action<UpdatingEventArgs> _updatingAction;

		internal BareMinimumEngine(
			GameEngineOptions options,
			Action<RenderingEventArgs> renderingAction,
			Action<UpdatingEventArgs> updatingAction
		)
			: base(
				options
					?? throw new ArgumentNullException(nameof(options))
			)
		{
			_renderingAction = renderingAction
				?? throw new ArgumentNullException(nameof(renderingAction));

			_updatingAction = updatingAction
				?? throw new ArgumentNullException(nameof(updatingAction));
		}

		#region GameEngine

		protected override void OnRendering(RenderingEventArgs e)
			=> _renderingAction(e);

		protected override void OnUpdating(UpdatingEventArgs e)
			=> _updatingAction(e);

		#endregion

	}
}