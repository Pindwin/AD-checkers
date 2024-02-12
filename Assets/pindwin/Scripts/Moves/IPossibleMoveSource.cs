using System.Collections.Generic;
using pindwin.Board;

namespace pindwin.Moves
{
	public interface IPossibleMoveSource
	{
		void GetAllPossibleMoves(List<PossibleMove> possibleMoves, int team);
		void GetAllPossibleMoves(List<PossibleMove> possibleMoves, int team, ref bool kill);
		void GetPossibleMoves(Tile origin, List<PossibleMove> moves, ref bool kill);
		MoveValidity IsMoveValid(Tile from, Tile to, out Tile capturedTile);
	}
}