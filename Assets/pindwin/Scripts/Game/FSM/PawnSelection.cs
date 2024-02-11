namespace pindwin.Game.FSM
{
	public class PawnSelection : GameState
	{
		public override void OnEnter(CheckersGameController gameController)
		{
			TryGoToTargetSelection(gameController, gameController.Board.SelectedTile);
		}

		public override void OnTileClicked(CheckersGameController gameController, Tile tile)
		{
			if (gameController.Board.SelectedTile == tile)
			{
				gameController.SetSelectedTile(tile, false);
				return;
			}
			
			TryGoToTargetSelection(gameController, tile);
		}

		private static void TryGoToTargetSelection(CheckersGameController gameController, Tile tile)
		{
			CheckersBoard board = gameController.Board;
			if (tile.IsValid)
			{
				bool isValidTeam = board[tile].Team() == gameController.CurrentTeam;
				gameController.SetSelectedTile(tile, isValidTeam);
				if (isValidTeam)
				{
					gameController.GoToState(GameStateType.TargetSelection);
				}
			}
		}
	}
}