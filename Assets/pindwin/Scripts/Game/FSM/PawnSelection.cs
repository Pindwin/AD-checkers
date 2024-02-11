namespace pindwin.Game.FSM
{
	public class PawnSelection : GameState
	{
		public override void OnEnter(CheckersGameController gameController)
		{
			TryGoToTargetSelection(gameController, gameController.Game.SelectedTile);
		}

		public override void OnTileClicked(CheckersGameController gameController, Tile tile)
		{
			if (gameController.Game.SelectedTile == tile)
			{
				gameController.SetSelectedTile(tile, false);
				return;
			}
			
			TryGoToTargetSelection(gameController, tile);
		}

		private static void TryGoToTargetSelection(CheckersGameController gameController, Tile tile)
		{
			CheckersGame game = gameController.Game;
			if (tile.IsNull == false)
			{
				bool isValidTeam = game[tile].Team() == gameController.CurrentTeam;
				gameController.SetSelectedTile(tile, isValidTeam);
				if (isValidTeam)
				{
					gameController.GoToState(GameStateType.TargetSelection);
				}
			}
		}
	}
}