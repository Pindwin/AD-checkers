using System;

namespace pindwin.Scripts.Game
{
	public readonly struct Tile : IEquatable<Tile>
	{
		public Tile(int x, int y)
		{
			X = x;
			Y = y;
		}
		
		public int X { get; }
		public int Y { get; }
		
		public bool IsBlack => (X + Y) % 2 == 1;

		public bool Equals(Tile other)
		{
			return X == other.X && Y == other.Y;
		}

		public override bool Equals(object obj)
		{
			return obj is Tile other && Equals(other);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(X, Y);
		}
		
		public static implicit operator int(Tile t) => t.Y * 8 + t.X;
		public static implicit operator Tile(int i) => new Tile(i % 8, i / 8);
	}
}