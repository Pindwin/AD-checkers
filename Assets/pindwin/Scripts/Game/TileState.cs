using System;

namespace pindwin.Game
{
	[Flags]
	public enum TileState
	{
		Empty = 0,
		White = 1 << 0,
		Pawn = 1 << 1,
		Promoted = 1 << 2,
		Selected = 1 << 3
	}

	public static class TileStateExtensions
	{
		public static bool IsWhite(this TileState state) => (state & TileState.White) != 0;
		public static bool IsBlack(this TileState state) => (state & TileState.White) == 0;
		public static bool IsQueen(this TileState state) => (state & TileState.Promoted) != 0;
		public static bool IsSelected(this TileState state) => (state & TileState.Selected) != 0;
		public static bool IsEmpty(this TileState state) => state == TileState.Empty;
		
		// team marks also the direction of movement - 1 being up, -1 being down
		public static int Team(this TileState state) => state.IsWhite() ? 1 : -1;
	}
}