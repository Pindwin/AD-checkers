using System.Collections.Generic;
using pindwin.Pawns;
using UnityEngine;

namespace pindwin.Game
{
	public class CheckersBoard
	{
		private readonly TileState[] _board;
		public Tile SelectedTile { get; private set; } = Tile.NullTile;
		public int PlayerTeam { get; } = TileState.White.Team();
		
		public TileState this[Tile tile] => _board[tile];

		public CheckersBoard(TileState[] gameState)
		{
			Debug.Assert(gameState.Length == 64);
			_board = gameState;
		}

		public void SetSelectedTile(Tile tile, bool isSelected)
		{
			if (SelectedTile.IsValid)
			{
				_board[SelectedTile] &= ~TileState.Selected;
			}
			SelectedTile = isSelected ? tile : Tile.NullTile;
			if (SelectedTile.IsValid)
			{
				_board[SelectedTile] |= TileState.Selected;
			}
		}
		
		public MoveValidity IsMoveValid(Tile from, Tile to, out Tile capturedTile)
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
			
			TileState fromState = _board[from];
			int moveRange = fromState.IsQueen() ? 8 : 2;
			if (distance > moveRange)
			{
				return MoveValidity.Invalid;
			}
			
			int xSign = Mathf.RoundToInt(Mathf.Sign(delta.x));
			int ySign = Mathf.RoundToInt(Mathf.Sign(delta.y));

			for (int i = 1; i < distance; i++)
			{
				Tile tile = from + new Vector2Int(i * xSign, i * ySign);
				TileState tileState = _board[tile];
				if (tileState.IsEmpty() == false)
				{
					if (capturedTile.IsValid || tileState.Team() == fromState.Team())
					{
						return MoveValidity.Invalid;
					}

					capturedTile = tile;
				}
			}

			return capturedTile.IsNull ? MoveValidity.Valid : MoveValidity.Capture;
		}

		public TileState MakeMove(Tile from, Tile to)
		{
			if (from.IsNull)
			{
				return TileState.Empty;
			}
			
			int team = _board[from].Team();
			_board[to] = _board[from];
			_board[from] = TileState.Empty;
			if ((to + new Vector2Int(0, team)).IsNull)
			{
				_board[to] |= TileState.Promoted;
			}

			return _board[to];
		}
		
		public bool Capture(Tile capturedTile)
		{
			if (capturedTile.IsNull)
			{
				return false;
			}
			
			_board[capturedTile] = TileState.Empty;
			return true;
		}

		public void ForceState(Tile position, TileState state)
		{
			_board[position] = state;
		}
		
		public void GetAllPossibleMoves(List<PossibleMove> possibleMoves, int team)
		{
			bool kill = false;
			GetAllPossibleMoves(possibleMoves, team, ref kill);
		}
		
		public void GetAllPossibleMoves(List<PossibleMove> possibleMoves, int team, ref bool kill)
		{
			for (int i = 0; i < _board.Length; i++)
			{
				TileState tileState = _board[i];
				if (tileState.IsEmpty() || tileState.Team() != team)
				{
					continue;
				}

				GetPossibleMoves(i, possibleMoves, ref kill);
			}
		}

		public void GetPossibleMoves(Tile origin, List<PossibleMove> moves, ref bool kill)
		{
			TileState state = _board[origin];
			
			if (state == TileState.Empty)
			{
				return;
			}

			int range = state.IsQueen() ? 8 : 2;
			int team = state.Team();
			bool isPawn = state.IsQueen() == false;
			GetMovesInDirection(-1, 1, kill, range, origin, team, ref kill, moves);
			GetMovesInDirection(1, 1, kill, range, origin, team, ref kill, moves);
			GetMovesInDirection(-1, -1, kill || isPawn, range, origin, team, ref kill, moves);
			GetMovesInDirection(1, -1, kill || isPawn, range, origin, team, ref kill, moves);
		}

		private void GetMovesInDirection(
			int yDir, 
			int xDir, 
			bool blockNonCaptures, 
			int maxRange, 
			Tile origin, 
			int team, 
			ref bool foundCapture, 
			List<PossibleMove> moves)
		{
			Tile captureTile = Tile.NullTile;
			for (int i = yDir; Mathf.Abs(i) <= maxRange; i += yDir)
			{
				Tile to = origin + new Vector2Int(i, Mathf.Abs(i) * team * xDir);
				if (to.IsNull)
				{
					//out of board
					break;
				}

				if (_board[to].IsEmpty())
				{
					if (captureTile.IsNull)
					{
						if (Mathf.Abs(i) == maxRange)
						{
							//only captures allowed at max range
							break;
						}
							
						if (foundCapture || blockNonCaptures)
						{
							//not interested in non-captures at this point
							continue;
						}
							
						moves.Add(new PossibleMove(origin, to, Tile.NullTile, false));
					}
					else
					{
						if (foundCapture == false)
						{
							//first capture found - not interested in regular moves anymore
							moves.Clear();
							foundCapture = true;
						}
						moves.Add(new PossibleMove(origin, to, captureTile, false));
						//todo establish follow ups
					}
					continue;
				}
					
				if (captureTile.IsNull && _board[to].Team() != team)
				{
					//first enemy piece found - it's a potential capture
					captureTile = to;
					continue;
				}
					
				break;
			}
		}
	}
}