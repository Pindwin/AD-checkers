namespace pindwin.Game
{
	public readonly struct PossibleCapture
	{
		public PossibleCapture(Tile from, Tile to, Tile capture, bool hasFollowUp)
		{
			From = from;
			To = to;
			Capture = capture;
			HasFollowUp = hasFollowUp;
		}
		
		public Tile From { get; }
		public Tile To { get; }
		public Tile Capture { get; }
		public bool HasFollowUp { get; }
	}
}