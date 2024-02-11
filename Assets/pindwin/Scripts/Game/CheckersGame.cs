using System.Collections.Generic;
using pindwin.Scripts.Pawns;
using UnityEngine;

namespace pindwin.Scripts.Game
{
	public class CheckersGame
	{
		private TileState[] _board;
		private Tile _selectedTile;
		public List<Pawn> Pawns { get; private set; } = new();

		public CheckersGame(TileState[] gameState)
		{
			Debug.Assert(gameState.Length == 64);
			_board = gameState;
		}

		public void SetTileSelected(Tile tile, bool isSelected)
		{
			_board[tile] = isSelected 
				? _board[tile] | TileState.Selected 
				: _board[tile] & ~TileState.Selected;
		}
		
		public TileState this[Tile tile]
		{
			get => _board[tile];
			set => _board[tile] = value;
		}

		public CheckersGame() : this(new TileState[64])
		{ }
	}
}