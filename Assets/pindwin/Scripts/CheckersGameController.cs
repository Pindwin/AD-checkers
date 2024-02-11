using System.Collections.Generic;
using pindwin.Board;
using pindwin.Game;
using pindwin.Game.FSM;
using pindwin.Pawns;
using UnityEngine;

namespace pindwin
{
    public class CheckersGameController : MonoBehaviour
    {
        [SerializeField] private BoardView _boardView;
        [SerializeField] private PawnView _pawnPrefab;
        
        public CheckersGame Game { get; private set; }
        
        private GameState _currentState;
        private CheckersGameFactory _gameFactory;
        
        private readonly Dictionary<GameStateType, GameState> _states = new()
        {
            { GameStateType.PawnSelection, new PawnSelection() },
            { GameStateType.TargetSelection, new TargetSelection() },
            { GameStateType.ComputerTurn, new ComputerTurn() }
        };
    
        private void Start()
        {
            _gameFactory = new CheckersGameFactory(_pawnPrefab, _boardView, transform);
            Game = _gameFactory.CreateNewGame();
            _boardView.Initialize(OnTileClicked);
            GoToState(GameStateType.PawnSelection);
        }

        public void GoToState(GameStateType gameStateType)
        {
            if (_states.TryGetValue(gameStateType, out var gameState) == false)
            {
                return;
            }

            _currentState = gameState;
            gameState.OnEnter(this);
        }
        
        private void OnTileClicked(Tile tile)
        {
            _currentState?.OnTileClicked(this, tile);
        }

        public void SetSelectedTile(Tile tile, bool isSelected)
        {
            if (Game.SelectedTile.IsNull == false)
            {
                Tile t = Game.SelectedTile;
                _boardView.GetTileByBoardCoord(t.X, t.Y).Selected = false;
            }
            Game.SetSelectedTile(tile, isSelected);
            if (tile.IsNull == false)
            {
                _boardView.GetTileByBoardCoord(tile.X, tile.Y).Selected = isSelected;
            }
        }
    }
}