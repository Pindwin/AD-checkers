using System;
using UnityEngine;

namespace pindwin.Game
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
		
		public static readonly Tile NullTile = new (-1, -1);

		public bool IsNull => X < 0 || Y < 0 || X > 7 || Y > 7;
		public bool IsValid => IsNull == false;
		public bool IsBlack => (X + Y) % 2 == 0;

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
		public static Vector2Int operator -(Tile a, Tile b) => new Vector2Int(a.X - b.X, a.Y - b.Y);
		public static Tile operator +(Tile a, Vector2Int b) => new Tile(a.X + b.x, a.Y + b.y);
	}
}