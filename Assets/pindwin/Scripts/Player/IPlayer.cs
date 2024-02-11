namespace pindwin
{
	public interface IPlayer
	{
		public int Team { get; }
		void StartTurn(CheckersGameController gameController);
	}
}