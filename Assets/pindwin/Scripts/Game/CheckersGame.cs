using System.Collections.Generic;
using pindwin.Game.FSM;
using pindwin.Pawns;
using UnityEngine;

namespace pindwin.Game
{
	public class CheckersGame
	{
		private readonly TileState[] _board;
		public Tile SelectedTile { get; private set; } = Tile.NullTile;
		public List<Pawn> Pawns { get; private set; } = new();
		public int PlayerTeam { get; } = TileState.White.Team();
		
		public TileState this[Tile tile] => _board[tile];

		private Dictionary<GameStateType, GameState> _states = new()
		{
			{ GameStateType.PawnSelection, new PawnSelection() },
			{ GameStateType.TargetSelection, new TargetSelection() },
			{ GameStateType.ComputerTurn, new ComputerTurn() }
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

		public void MovePawn(Tile selectedTile, Tile tile)
		{
			_board[tile] = _board[selectedTile];
			_board[selectedTile] = TileState.Empty;
			Pawn pawn = Pawns.Find(p => p.Position == selectedTile);
			Debug.Assert(pawn != null);
			pawn.Position = tile;
		}
		
		public void Capture(Tile capturedTile)
		{
			_board[capturedTile] = TileState.Empty;
			Pawn pawn = Pawns.Find(p => p.Position == capturedTile);
			Debug.Assert(pawn != null);
			pawn.IsDead = true;
			pawn.Position = Tile.NullTile;
			Pawns.Remove(pawn);
		}
		
		public void GetAllPossibleMoves(List<PossibleMove> moves, List<PossibleCapture> captures, int team)
		{
			foreach (Pawn pawn in Pawns)
			{
				if (pawn.Team != team)
				{
					continue;
				}
				
				GetPossibleMoves(pawn.Position, moves, captures);
			}
		}

		public void GetPossibleMoves(Tile from, List<PossibleMove> moves, List<PossibleCapture> captures)
		{
			TileState state = _board[from];
			
			if (state == TileState.Empty)
			{
				return;
			}

			int maxRange = state.IsQueen() ? 8 : 2;
			Tile captureTile = Tile.NullTile;
			GetMovesInDirection(-1, 1);
			GetMovesInDirection(1, 1);
			//GetMovesInDirection(-1, -1, false);
			//GetMovesInDirection(1, -1, false);
			return;

			void GetMovesInDirection(int horizontalDirection, int verticalDirection, bool allowNonCaptures = true)
			{
				for (int i = horizontalDirection; Mathf.Abs(i) <= maxRange; i += horizontalDirection)
				{
					Tile to = from + new Vector2Int(i, i * state.Team() * verticalDirection);
					if (to.IsNull)
					{
						break;
					}

					if (_board[to].IsEmpty())
					{
						if (captureTile.IsNull)
						{
							if (allowNonCaptures && Mathf.Abs(i) < maxRange)
							{
								moves.Add(new PossibleMove(from, to));
							}
						}
						else
						{
							captures.Add(new PossibleCapture(from, to, captureTile, false));
							//todo establish follow ups
						}
					}
					else if (captureTile.IsNull && _board[to].Team() != state.Team())
					{
						captureTile = to;
					}
					else
					{
						break;
					}
				}
			}
		}
	}
}