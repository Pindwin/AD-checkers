using System.Collections.Generic;
using pindwin.Game.FSM;
using pindwin.Pawns;
using UnityEngine;

namespace pindwin.Game
{
	public class CheckersGame
	{
		private TileState[] _board;
		public Tile SelectedTile { get; private set; } = Tile.NullTile;
		public List<Pawn> Pawns { get; private set; } = new();
		public int PlayerTeam { get; } = TileState.White.Team();

		private Dictionary<GameStateType, GameState> _states = new()
		{
			{ GameStateType.PawnSelection, new PawnSelection() },
			{ GameStateType.TargetSelection, new TargetSelection() },
			{ GameStateType.Waiting, new Waiting() }
		};

		public CheckersGame(TileState[] gameState)
		{
			Debug.Assert(gameState.Length == 64);
			_board = gameState;
		}

		public void SetSelectedTile(Tile tile, bool isSelected)
		{
			if (SelectedTile.IsNull == false)
			{
				_board[SelectedTile] &= ~TileState.Selected;
			}
			SelectedTile = isSelected ? tile : Tile.NullTile;
			if (SelectedTile.IsNull == false)
			{
				_board[SelectedTile] |= TileState.Selected;
			}
		}
		
		public MoveValidity CanMoveTo(Tile from, Tile to, out Tile capturedTile)
		{
			capturedTile = Tile.NullTile;
			if (from.IsNull || to.IsNull)
			{
				return MoveValidity.Invalid;
			}
			
			if (_board[to].IsEmpty() == false)
			{
				return MoveValidity.Invalid;
			}

			TileState fromState = _board[from];
			if (_board[from].IsQueen())
			{
				return CanQueenMoveTo(from, to);
			}

			Vector2Int delta = to - from;
			if (Mathf.Abs(delta.x) == 1 && delta.y == fromState.Team())
			{
				return MoveValidity.Valid;
			}

			if (Mathf.Abs(delta.x) == 2 
				&& Mathf.Abs(delta.y) == 2 
				&& _board[from + delta / 2].Team() != _board[from].Team())
			{
				capturedTile = from + delta / 2;
				return MoveValidity.Capture;
			}

			return MoveValidity.Invalid;
		}

		private MoveValidity CanQueenMoveTo(Tile from, Tile to)
		{
			//todo implement
			return MoveValidity.Invalid;
		}
		
		public TileState this[Tile tile] => _board[tile];

		public void MovePawn(Tile selectedTile, Tile tile)
		{
			_board[tile] = _board[selectedTile];
			_board[selectedTile] = TileState.Empty;
			Pawn pawn = Pawns.Find(p => p.Position == selectedTile);
			Debug.Assert(pawn != null);
			pawn.Position = tile;
		}
	}
}