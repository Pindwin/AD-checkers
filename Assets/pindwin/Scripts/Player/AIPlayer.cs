using System.Collections;
using System.Collections.Generic;
using pindwin.Game;
using pindwin.Game.FSM;
using UnityEngine;

namespace pindwin
{
	public class AIPlayer : IPlayer
	{
		public AIPlayer(int team, MonoBehaviour coroutineRunner)
		{
			Team = team;
			_coroutineRunner = coroutineRunner;
		}

		public int Team { get; }
		
		private readonly MonoBehaviour _coroutineRunner;
		
		public void StartTurn(CheckersGameController gameController)
		{
			gameController.SetSelectedTile(gameController.Board.SelectedTile, false);
			List<PossibleMove> moves = gameController.PossibleMovesBuffer;
			moves.Clear();
			gameController.Board.GetAllPossibleMoves(moves, gameController.Board.PlayerTeam * -1);
			if (moves.Count > 0)
			{
				PossibleMove capture = moves[Random.Range(0, moves.Count)];
				_coroutineRunner.StartCoroutine(PlayTurn(capture.From, capture.To, gameController));
			}
			else
			{
				Debug.Log("Game ended - player won!");
			}
		}

		private IEnumerator PlayTurn(Tile from, Tile to, CheckersGameController gameController)
		{
			yield return new WaitForSeconds(0.5f);
			gameController.GoToState(GameStateType.PawnSelection);
			gameController.OnTileClicked(from);
			gameController.OnTileClicked(to);
			if (gameController.IsMidCombo)
			{
				List<PossibleMove> moves = gameController.PossibleMovesBuffer;
				bool killsOnly = true;
				gameController.Board.GetPossibleMoves(to, moves, ref killsOnly);
				if (moves.Count > 0)
				{
					PossibleMove capture = moves[Random.Range(0, moves.Count)];
					_coroutineRunner.StartCoroutine(PlayTurn(capture.From, capture.To, gameController));
				}
			}
		}
	}
}