using pindwin.Scripts.Board;
using pindwin.Scripts.Game;
using pindwin.Scripts.Pawns;
using UnityEngine;

public class CheckersController : MonoBehaviour
{
    [SerializeField] private BoardView _boardView;
    [SerializeField] private PawnView _pawnPrefab;
    
    private CheckersGameFactory _gameFactory;
    private CheckersGame _game;
    private ClickTileCommand _clickTileCommand;
    
    private void Start()
    {
        _gameFactory = new CheckersGameFactory(_pawnPrefab, _boardView, transform);
        _game = _gameFactory.CreateNewGame();
        _clickTileCommand = new ClickTileCommand(_game, _boardView);
        _boardView.Initialize(_clickTileCommand);
    }
}