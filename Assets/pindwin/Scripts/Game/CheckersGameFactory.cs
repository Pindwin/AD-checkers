using System.Collections.Generic;
using pindwin.Scripts.Board;
using pindwin.Scripts.Pawns;
using UnityEngine;

namespace pindwin.Scripts.Game
{
	public class CheckersGameFactory
	{
		private readonly PawnView _pawnPrefab;
		private readonly BoardView _boardView;
		private readonly Transform _pawnsRoot;
		private readonly List<Pawn> _pawnsBuffer;

		public CheckersGameFactory(PawnView pawnPrefab, BoardView boardView, Transform pawnsRoot)
		{
			_pawnPrefab = pawnPrefab;
			_boardView = boardView;
			_pawnsRoot = pawnsRoot;
			_pawnsBuffer = new List<Pawn>();
		}
		
		public CheckersGame CreateNewGame()
		{
			var gameState = new TileState[64];
			_pawnsBuffer.Clear();
			for (int y = 0; y < 8; y++)
			{
				for (int x = 0; x < 8; x++)
				{
					var t = new Tile(x, y);
					if (t.IsBlack)
					{
						var state = TileState.Empty;
						if (y < 3 || y > 4)
						{
							state |= TileState.Pawn;
							if (y < 3)
							{
								state = TileState.White;
							}
							_pawnsBuffer.Add(new Pawn(state, t, _pawnPrefab, _boardView, _pawnsRoot));
						}
						
						gameState[t] = state;
					}
				}
			}

			var game = new CheckersGame(gameState);
			game.Pawns.AddRange(_pawnsBuffer);
			return game;
		}
	}
}