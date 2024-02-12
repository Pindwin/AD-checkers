using pindwin.Board;

namespace pindwin.States
{
	public class PawnSelection : GameState
	{
		public override void OnEnter(CheckersGameController gameController)
		{
			TryGoToTargetSelection(gameController, gameController.SelectedTile);
		}

		public override void OnTileClicked(CheckersGameController gameController, Tile tile)
		{
			if (gameController.SelectedTile == tile)
			{
				gameController.SetSelectedTile(tile, false);
				return;
			}
			
			TryGoToTargetSelection(gameController, tile);
		}

		private static void TryGoToTargetSelection(CheckersGameController gameController, Tile tile)
		{
			if (tile.IsNull)
			{
				return;
			}

			bool isValidTeam = gameController.GetStateByTile(tile).Team() == gameController.CurrentTeam;
			gameController.SetSelectedTile(tile, isValidTeam);
			if (isValidTeam)
			{
				gameController.GoToState(GameStateType.TargetSelection);
			}
		}
	}
}