using pindwin.States;

namespace pindwin.Player
{
	public class LocalPlayer : IPlayer
	{
		public LocalPlayer(int team)
		{
			Team = team;
		}

		public int Team { get; }

		public void StartTurn(CheckersGameController gameController)
		{
			gameController.GoToState(GameStateType.PawnSelection);
		}
	}
}