using pindwin.Board;
using pindwin.Moves;

namespace pindwin.States
{
	public class TargetSelection : GameState
	{
		public override void OnEnter(CheckersGameController gameController) { }

		public override void OnTileClicked(CheckersGameController gameController, Tile tile)
		{
			Tile selectedTile = gameController.SelectedTile;
			MoveValidity validity = gameController.IsMoveValid(selectedTile, tile, out Tile capturedTile);
			if (validity != MoveValidity.Invalid)
			{
				switch (validity)
				{
					case MoveValidity.Valid:
						if (gameController.IsMidCombo || CanStartCapture(gameController))
						{
							return;
						}

						gameController.CommitMove(selectedTile, tile, Tile.NullTile);
						break;
					case MoveValidity.Capture:
						gameController.CommitMove(selectedTile, tile, capturedTile);
						gameController.IsMidCombo = CanContinueCapture(gameController, tile);
						if (gameController.IsMidCombo)
						{
							gameController.SetSelectedTile(tile, true);
							gameController.GoToState(GameStateType.PawnSelection);
							return;
						}

						break;
				}

				gameController.PassTurn();
				return;
			}

			if (gameController.IsMidCombo == false)
			{
				gameController.SetSelectedTile(tile, true);
				gameController.GoToState(GameStateType.PawnSelection);
			}
		}

		private bool CanContinueCapture(CheckersGameController gameController, Tile fromTile)
		{
			var buffer = gameController.PossibleMovesBuffer;
			buffer.Clear();
			bool canContinue = false;
			gameController.GetPossibleMoves(fromTile, buffer, ref canContinue);
			return canContinue;
		}

		private bool CanStartCapture(CheckersGameController gameController)
		{
			var buffer = gameController.PossibleMovesBuffer;
			buffer.Clear();
			bool foundKill = false;
			gameController.GetAllPossibleMoves(buffer, gameController.CurrentTeam, ref foundKill);
			return foundKill;
		}
	}
}