using System.Collections;
using System.Collections.Generic;
using pindwin.Board;
using pindwin.Moves;
using pindwin.States;
using UnityEngine;

namespace pindwin.Player
{
	public class AIPlayer : IPlayer
	{
		public int Team { get; }
		
		private readonly MonoBehaviour _coroutineRunner;
		
		public AIPlayer(int team, MonoBehaviour coroutineRunner)
		{
			Team = team;
			_coroutineRunner = coroutineRunner;
		}
		
		public void StartTurn(CheckersGameController gameController)
		{
			gameController.SetSelectedTile(gameController.SelectedTile, false);
			List<PossibleMove> moves = gameController.PossibleMovesBuffer;
			moves.Clear();
			gameController.GetAllPossibleMoves(moves, gameController.LocalTeam * -1);
			if (moves.Count > 0)
			{
				PossibleMove move = moves[Random.Range(0, moves.Count)];
				_coroutineRunner.StartCoroutine(PlayTurn(move.From, move.To, gameController));
				return;
			}
			
			Debug.Log("Game ended - player won!");
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
				gameController.GetPossibleMoves(to, moves, ref killsOnly);
				if (moves.Count > 0)
				{
					PossibleMove capture = moves[Random.Range(0, moves.Count)];
					_coroutineRunner.StartCoroutine(PlayTurn(capture.From, capture.To, gameController));
				}
			}
		}
	}
}