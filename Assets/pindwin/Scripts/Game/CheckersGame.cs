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
				return CanQueenMoveTo(from, to, out capturedTile);
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

		private MoveValidity CanQueenMoveTo(Tile from, Tile to, out Tile capturedTile)
		{
			capturedTile = Tile.NullTile;
			Vector2Int delta = to - from;
			int distance = Mathf.Abs(delta.x);
			if (distance != Mathf.Abs(delta.y))
			{
				return MoveValidity.Invalid;
			}

			if (distance == 1)
			{
				return MoveValidity.Valid;
			}

			var fromState = _board[from];
			int xSign = Mathf.RoundToInt(Mathf.Sign(delta.x));
			int ySign = Mathf.RoundToInt(Mathf.Sign(delta.y));

			for (int i = 2; i < distance; i++)
			{
				Tile tile = from + new Vector2Int(i * xSign, i * ySign);
				TileState tileState = _board[tile];
				if (tileState.IsEmpty() == false)
				{
					if (capturedTile.IsNull == false || tileState.Team() == fromState.Team())
					{
						return MoveValidity.Invalid;
					}

					capturedTile = tile;
				}
			}

			return capturedTile.IsNull ? MoveValidity.Valid : MoveValidity.Capture;
		}

		public void MovePawn(Tile selectedTile, Tile tile)
		{
			_board[tile] = _board[selectedTile];
			_board[selectedTile] = TileState.Empty;
			Pawn pawn = Pawns.Find(p => p.Position == selectedTile);
			Debug.Assert(pawn != null);
			pawn.Position = tile;
			if ((tile + new Vector2Int(0, pawn.Team)).IsNull)
			{
				_board[tile] |= TileState.Promoted;
				pawn.IsQueen = true;
			}
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
			GetMovesInDirection(-1);
			GetMovesInDirection(1);
			GetMovesInDirection(-1, -1, state.IsQueen());
			GetMovesInDirection(1, -1, state.IsQueen());
			return;

			void GetMovesInDirection(int horizontalDirection, int verticalDirection = 1, bool allowNonCaptures = true)
			{
				Tile captureTile = Tile.NullTile;
				for (int i = horizontalDirection; Mathf.Abs(i) <= maxRange; i += horizontalDirection)
				{
					Tile to = from + new Vector2Int(i, Mathf.Abs(i) * state.Team() * verticalDirection);
					if (to.IsNull)
					{
						break;
					}

					if (_board[to].IsEmpty())
					{
						if (captureTile.IsNull)
						{
							if (Mathf.Abs(i) < maxRange && allowNonCaptures)
							{
								moves.Add(new PossibleMove(from, to));
							}
							else
							{
								break;
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