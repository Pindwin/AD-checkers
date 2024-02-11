using pindwin.Scripts.Game;
using UnityEngine;

public class ClickTileCommand
{
	private CheckersController _controller;

	public ClickTileCommand(CheckersController controller)
	{
		_controller = controller;
	}
	
	public void Execute(Tile tile)
	{
		Debug.Log($"OnClick! {tile.X} {tile.Y}");
		_controller.ToggleTileSelected(tile);
	}
}