using pindwin.Board;

namespace pindwin.States
{
	public abstract class GameState
	{
		public abstract void OnEnter(CheckersGameController gameController);
		public abstract void OnTileClicked(CheckersGameController gameController, Tile tile);
	}
}