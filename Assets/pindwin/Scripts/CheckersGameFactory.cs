using System.Collections.Generic;
using pindwin.Board;
using pindwin.Board.View;
using pindwin.Pawns;
using UnityEngine;

namespace pindwin
{
	public class CheckersGameFactory
	{
		private readonly BoardView _boardView;
		private readonly PawnView _pawnPrefab;
		private readonly Transform _pawnsRoot;

		public CheckersGameFactory(PawnView pawnPrefab, BoardView boardView, Transform pawnsRoot)
		{
			_pawnPrefab = pawnPrefab;
			_boardView = boardView;
			_pawnsRoot = pawnsRoot;
		}

		public CheckersBoard SetupBoard(List<Pawn> pawns)
		{
			var gameState = new TileState[64];
			pawns.Clear();
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
								state |= TileState.White;
							}

							pawns.Add(new Pawn(state, t, _pawnPrefab, _boardView, _pawnsRoot));
						}

						gameState[t] = state;
					}
				}
			}

			return new CheckersBoard(gameState);
		}
	}
}