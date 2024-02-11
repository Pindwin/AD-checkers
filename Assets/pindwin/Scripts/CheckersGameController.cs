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
        
        public CheckersBoard Board { get; private set; }
        
        private GameState _currentState;
        private CheckersGameFactory _gameFactory;
        private int _currentPlayer; 
        public int CurrentTeam => _players[_currentPlayer].Team;
        public List<PossibleMove> PossibleMovesBuffer { get; } = new();
        public bool IsMidCombo { get; set; }

        private readonly List<IPlayer> _players = new();
        
        private readonly Dictionary<GameStateType, GameState> _states = new()
        {
            { GameStateType.PawnSelection, new PawnSelection() },
            { GameStateType.TargetSelection, new TargetSelection() }
        };

        private void Awake()
        {
            _players.AddRange(new IPlayer[] {
                new LocalPlayer(TileState.White.Team()), 
                new AIPlayer(TileState.White.Team() * -1, this)
            });
            
            _gameFactory = new CheckersGameFactory(_pawnPrefab, _boardView, transform);
            Board = _gameFactory.CreateNewGame();
            _boardView.Initialize(OnTileClicked);
            _currentPlayer = Random.Range(0, _players.Count);
            _players[_currentPlayer].StartTurn(this);
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
        
        public void OnTileClicked(Tile tile)
        {
            _currentState?.OnTileClicked(this, tile);
        }

        public void SetSelectedTile(Tile tile, bool isSelected)
        {
            if (Board.SelectedTile.IsValid)
            {
                Tile t = Board.SelectedTile;
                _boardView.GetTileByBoardCoord(t.X, t.Y).Selected = false;
            }
            Board.SetSelectedTile(tile, isSelected);
            if (tile.IsValid)
            {
                _boardView.GetTileByBoardCoord(tile.X, tile.Y).Selected = isSelected;
            }
        }

        public void PassTurn()
        {
            _currentPlayer = (_currentPlayer + 1) % _players.Count;
            _players[_currentPlayer].StartTurn(this);
        }
    }
}