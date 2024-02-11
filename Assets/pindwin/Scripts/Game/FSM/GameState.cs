namespace pindwin.Game.FSM
{
	public abstract class GameState
	{
		public abstract void OnEnter(CheckersGameController gameController);
		public abstract void OnTileClicked(CheckersGameController gameController, Tile tile);
	}
}