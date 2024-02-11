using pindwin.Scripts.Board;
using pindwin.Scripts.Game;
using UnityEngine;

namespace pindwin.Scripts.Pawns
{
	public class Pawn
	{
		private readonly PawnView _pawnView;
		private readonly BoardView _boardView;
		
		public int Team { get; }
		public bool IsQueen { get; set; }
		public bool IsDead { get; set; }
		public bool IsBlack => Team == 1;
		public Tile Position { get; set; }
		
		public Pawn(TileState state, Tile position, PawnView pawnPrefab, BoardView boardView, Transform root)
		{
			Debug.Assert(state != TileState.Empty);
			Team = state.Team();
			IsQueen = state.IsQueen();
			Position = position;
			_boardView = boardView;
			
			_pawnView = Object.Instantiate(pawnPrefab.gameObject, root).GetComponent<PawnView>();
			_pawnView.Refresh(this, _boardView);
		}
	}
}