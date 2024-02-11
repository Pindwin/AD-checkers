using UnityEngine;

namespace pindwin.Game.FSM
{
	public class TargetSelection : GameState
	{
		public override GameStateType Type => GameStateType.TargetSelection;
		public override void OnEnter(CheckersGameController gameController)
		{
			Debug.Log(Type);
		}

		public override void OnTileClicked(CheckersGameController gameController, Tile tile)
		{
			Tile selectedTile = gameController.Game.SelectedTile;
			MoveValidity validity = gameController.Game.CanMoveTo(selectedTile, tile, out Tile capturedTile);
			if (validity != MoveValidity.Invalid)
			{
				switch (validity)
				{
					case MoveValidity.Valid:
						gameController.Game.MovePawn(selectedTile, tile);
						break;
					case MoveValidity.Capture:
						gameController.Game.MovePawn(selectedTile, tile);
						//gameController.Game.Capture(capturedTile);
						//gameController.GoToState(GameStateType.TargetSelection);
						break;
				}
				gameController.GoToState(GameStateType.Waiting);
				return;
			}
			
			gameController.SetSelectedTile(tile, true);
			gameController.GoToState(GameStateType.PawnSelection);
		}
	}
}