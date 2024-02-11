namespace pindwin.Game.FSM
{
	public class Waiting : GameState
	{
		public override GameStateType Type => GameStateType.Waiting;
		public override void OnEnter(CheckersGameController gameController)
		{
			gameController.SetSelectedTile(gameController.Game.SelectedTile, false);
			// todo fix this
			gameController.GoToState(GameStateType.PawnSelection);
		}

		public override void OnTileClicked(CheckersGameController gameController, Tile tile)
		{
			//do nothing
		}
	}
}