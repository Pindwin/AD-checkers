using System.Collections.Generic;
using pindwin.Moves;
using UnityEngine;

namespace pindwin.Board
{
	public class CheckersBoard : IPossibleMoveSource
	{
		private readonly TileState[] _tileStates;

		public CheckersBoard(TileState[] gameState)
		{
			Debug.Assert(gameState.Length == 64);
			_tileStates = gameState;
		}

		public TileState this[Tile tile] => _tileStates[tile];

		public bool Capture(Tile capturedTile)
		{
			if (capturedTile.IsNull)
			{
				return false;
			}

			_tileStates[capturedTile] = TileState.Empty;
			return true;
		}

		public void ForceState(Tile position, TileState state)
		{
			_tileStates[position] = state;
		}

		public void GetAllPossibleMoves(List<PossibleMove> possibleMoves, int team)
		{
			bool kill = false;
			GetAllPossibleMoves(possibleMoves, team, ref kill);
		}

		public void GetAllPossibleMoves(List<PossibleMove> possibleMoves, int team, ref bool kill)
		{
			for (int i = 0; i < _tileStates.Length; i++)
			{
				TileState tileState = _tileStates[i];
				if (tileState.IsEmpty() || tileState.Team() != team)
				{
					continue;
				}

				GetPossibleMoves(i, possibleMoves, ref kill);
			}
		}

		public void GetPossibleMoves(Tile origin, List<PossibleMove> moves, ref bool kill)
		{
			TileState state = _tileStates[origin];

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

		public MoveValidity IsMoveValid(Tile from, Tile to, out Tile capturedTile)
		{
			capturedTile = Tile.NullTile;
			if (from.IsNull || to.IsNull)
			{
				return MoveValidity.Invalid;
			}

			if (_tileStates[to].IsEmpty() == false)
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

			TileState fromState = _tileStates[from];
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
				TileState tileState = _tileStates[tile];
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

			int team = _tileStates[from].Team();
			_tileStates[to] = _tileStates[from];
			_tileStates[from] = TileState.Empty;
			if ((to + new Vector2Int(0, team)).IsNull)
			{
				_tileStates[to] |= TileState.Promoted;
			}

			return _tileStates[to];
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

				if (_tileStates[to].IsEmpty())
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

						moves.Add(new PossibleMove(origin, to));
					}
					else
					{
						if (foundCapture == false)
						{
							//first capture found - not interested in regular moves anymore
							moves.Clear();
							foundCapture = true;
						}

						moves.Add(new PossibleMove(origin, to));
					}

					continue;
				}

				if (captureTile.IsNull && _tileStates[to].Team() != team)
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