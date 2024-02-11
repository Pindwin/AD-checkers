namespace pindwin.Scripts.Game
{
	public enum TileState
	{
		Empty,
		WhitePawn,
		WhiteQueen,
		BlackPawn,
		BlackQueen
	}

	public static class TileStateExtensions
	{
		public static bool IsWhite(this TileState state) => state == TileState.WhitePawn || state == TileState.WhiteQueen;
		public static bool IsBlack(this TileState state) => state == TileState.BlackPawn || state == TileState.BlackQueen;
		public static bool IsQueen(this TileState state) => state == TileState.WhiteQueen || state == TileState.BlackQueen;
		public static int Team(this TileState state) => state.IsWhite() ? 0 : 1;
	}
}