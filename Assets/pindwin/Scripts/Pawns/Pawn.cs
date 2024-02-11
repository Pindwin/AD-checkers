using pindwin.Board;
using pindwin.Game;
using UnityEngine;

namespace pindwin.Pawns
{
	public class Pawn
	{
		private readonly PawnView _pawnView;
		private readonly BoardView _boardView;
		private Tile _position;
		private bool _isDead;
		private bool _isQueen;

		public int Team { get; }

		public bool IsQueen
		{
			get => _isQueen;
			set
			{
				_isQueen = value;
				_pawnView.Refresh(this, _boardView);
			}
		}

		public bool IsDead
		{
			get => _isDead;
			set
			{
				_isDead = value;
				_pawnView.Refresh(this, _boardView);
			}
		}

		public bool IsWhite => Team == TileState.White.Team();

		public Tile Position
		{
			get => _position;
			set
			{
				_position = value;
				if (_position.IsNull)
				{
					return;
				}
				
				_pawnView.Refresh(this, _boardView);
			}
		}

		public Pawn(TileState state, Tile position, PawnView pawnPrefab, BoardView boardView, Transform root)
		{
			Debug.Assert(state != TileState.Empty);
			_boardView = boardView;
			
			_pawnView = Object.Instantiate(pawnPrefab.gameObject, root).GetComponent<PawnView>();
			
			Team = state.Team();
			IsQueen = state.IsQueen();
			Position = position;
		}
	}
}