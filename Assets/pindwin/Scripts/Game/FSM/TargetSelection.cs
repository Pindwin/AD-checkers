namespace pindwin.Game.FSM
{
	public class TargetSelection : GameState
	{
		public override void OnEnter(CheckersGameController gameController)
		{ }

		public override void OnTileClicked(CheckersGameController gameController, Tile tile)
		{
			Tile selectedTile = gameController.Board.SelectedTile;
			MoveValidity validity = gameController.Board.IsMoveValid(selectedTile, tile, out Tile capturedTile);
			if (validity != MoveValidity.Invalid)
			{
				switch (validity)
				{
					case MoveValidity.Valid:
						if (gameController.IsMidCombo || CanStartCapture(gameController))
						{
							return;
						}
						gameController.Board.MovePawn(selectedTile, tile);
						break;
					case MoveValidity.Capture:
						gameController.Board.MovePawn(selectedTile, tile);
						gameController.Board.Capture(capturedTile);
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

		private bool CanStartCapture(CheckersGameController gameController)
		{
			var buffer = gameController.PossibleMovesBuffer;
			buffer.Clear();
			bool foundKill = false;
			gameController.Board.GetAllPossibleMoves(buffer, gameController.CurrentTeam, ref foundKill);
			return foundKill;
		}

		private bool CanContinueCapture(CheckersGameController gameController, Tile fromTile)
		{
			var buffer = gameController.PossibleMovesBuffer;
			buffer.Clear();
			bool canContinue = false;
			gameController.Board.GetPossibleMoves(fromTile, buffer, ref canContinue);
			return canContinue;
		}
	}
}