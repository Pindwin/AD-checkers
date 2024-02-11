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
		private readonly List<PossibleCapture> _possibleCaptures = new();
		private readonly List<PossibleMove> _possibleMoves = new();
		
		public void StartTurn(CheckersGameController gameController)
		{
			gameController.SetSelectedTile(gameController.Game.SelectedTile, false);
			_possibleMoves.Clear();
			_possibleCaptures.Clear();
			gameController.Game.GetAllPossibleMoves(_possibleMoves, _possibleCaptures, gameController.Game.PlayerTeam * -1);
			if (_possibleCaptures.Count > 0)
			{
				PossibleCapture capture = _possibleCaptures[Random.Range(0, _possibleCaptures.Count)];
				_coroutineRunner.StartCoroutine(PlayTurn(capture.From, capture.To, gameController));
			}
			else if (_possibleMoves.Count > 0)
			{
				PossibleMove move = _possibleMoves[Random.Range(0, _possibleMoves.Count)];
				_coroutineRunner.StartCoroutine(PlayTurn(move.From, move.To, gameController));
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
		}
	}
}