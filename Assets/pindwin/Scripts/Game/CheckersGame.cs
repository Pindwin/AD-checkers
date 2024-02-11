using System.Collections.Generic;
using pindwin.Scripts.Board;
using pindwin.Scripts.Pawns;
using UnityEngine;

namespace pindwin.Scripts.Game
{
	public class CheckersGame
	{
		private TileState[] _board;
		public List<Pawn> Pawns { get; private set; } = new();

		public CheckersGame(TileState[] gameState, BoardView boardView)
		{
			Debug.Assert(gameState.Length == 64);
			_board = gameState;
		}

		public CheckersGame(BoardView boardView) : this(new TileState[64], boardView)
		{ }
	}
}