using System.Collections.Generic;
using UnityEngine;

namespace pindwin.Game.FSM
{
	public class ComputerTurn : GameState
	{
		private readonly List<PossibleCapture> _possibleCaptures = new();
		private readonly List<PossibleMove> _possibleMoves = new();
		
		public override void OnEnter(CheckersGameController gameController)
		{
			gameController.SetSelectedTile(gameController.Game.SelectedTile, false);
			_possibleMoves.Clear();
			_possibleCaptures.Clear();
			gameController.Game.GetAllPossibleMoves(_possibleMoves, _possibleCaptures, gameController.Game.PlayerTeam * -1);
			if (_possibleCaptures.Count > 0)
			{
				PossibleCapture capture = _possibleCaptures[Random.Range(0, _possibleCaptures.Count)];
				gameController.Game.MovePawn(capture.From, capture.To);
				gameController.Game.Capture(capture.Capture);
			}
			else if (_possibleMoves.Count > 0)
			{
				PossibleMove move = _possibleMoves[Random.Range(0, _possibleMoves.Count)];
				gameController.Game.MovePawn(move.From, move.To);
			}
			else
			{
				Debug.Log("Game ended - player won!");
			}
			
			gameController.GoToState(GameStateType.PawnSelection);
		}

		public override void OnTileClicked(CheckersGameController gameController, Tile tile)
		{
			//do nothing
		}
	}
}