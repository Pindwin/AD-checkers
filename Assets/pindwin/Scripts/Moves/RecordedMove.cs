using pindwin.Board;

namespace pindwin.Moves
{
	public readonly struct RecordedMove
	{
		public RecordedMove(int team, TileState originalState, Tile from, Tile to, Tile capture, TileState captureState)
		{
			Team = team;
			OriginalState = originalState;
			From = from;
			To = to;
			Capture = capture;
			CaptureState = captureState;
		}
		public int Team { get; }
		public TileState OriginalState { get; }
		public Tile From { get; }
		public Tile To { get; }
		public Tile Capture { get; }
		public TileState CaptureState { get; }
	}
}