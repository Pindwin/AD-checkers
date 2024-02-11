namespace pindwin.Game
{
	public readonly struct RecordedMove
	{
		public RecordedMove(int team, Tile from, Tile to, Tile capture, TileState captureState)
		{
			Team = team;
			From = from;
			To = to;
			Capture = capture;
			CaptureState = captureState;
		}
		public readonly int Team { get; }
		public readonly Tile From { get; }
		public readonly Tile To { get; }
		public readonly Tile Capture { get; }
		public readonly TileState CaptureState { get; }
	}
}