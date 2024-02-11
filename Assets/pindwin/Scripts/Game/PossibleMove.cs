﻿namespace pindwin.Game
{
	public readonly struct PossibleMove
	{
		public PossibleMove(Tile from, Tile to)
		{
			From = from;
			To = to;
		}
		
		public Tile From { get; }
		public Tile To { get; }
	}
}