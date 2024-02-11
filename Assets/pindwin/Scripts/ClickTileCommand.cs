using pindwin.Scripts.Board;
using pindwin.Scripts.Game;
using UnityEngine;

public class ClickTileCommand
{
	private CheckersGame _game;
	private readonly BoardView _view;

	public ClickTileCommand(CheckersGame game, BoardView view)
	{
		_game = game;
		_view = view;
	}
	
	public void Execute(Tile tile)
	{
		Debug.Log($"OnClick! {tile.X} {tile.Y}");
		bool isSelected = _game[tile].IsSelected();
		_game.SetTileSelected(tile, !isSelected);
		_view.GetTileByBoardCoord(tile.X, tile.Y).Selected = !isSelected;
	}
}