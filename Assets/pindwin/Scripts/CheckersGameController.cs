using System.Collections.Generic;
using pindwin.Board;
using pindwin.Game;
using pindwin.Game.FSM;
using pindwin.Pawns;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        private int _localTeam = TileState.White.Team();
        public int CurrentTeam => _players[_currentPlayer].Team;
        public List<PossibleMove> PossibleMovesBuffer { get; } = new();
        public bool IsMidCombo { get; set; }
        public Stack<RecordedMove> MovesHistory { get; } = new();

        private readonly List<IPlayer> _players = new();
        
        private readonly Dictionary<GameStateType, GameState> _states = new()
        {
            { GameStateType.PawnSelection, new PawnSelection() },
            { GameStateType.TargetSelection, new TargetSelection() }
        };

        private void Start()
        {
            _players.AddRange(new IPlayer[] {
                new LocalPlayer(_localTeam), 
                new AIPlayer(_localTeam * -1, this)
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
        
        public void CommitMove(Tile from, Tile to, Tile capturedTile)
        {
            MovesHistory.Push(new RecordedMove(CurrentTeam, from, to, capturedTile, capturedTile.IsValid ? Board[capturedTile] : TileState.Empty));
            Board.MovePawn(from, to);
            if (capturedTile.IsValid)
            {
                Board.Capture(capturedTile);
            }
        }

        public void UndoLastMove()
        {
            if (MovesHistory.Count == 0)
            {
                return;
            }

            RecordedMove move = MovesHistory.Pop();
            Board.MovePawn(move.To, move.From);
            if (move.Capture.IsValid)
            {
                Board.SpawnPawn(move.Capture, move.CaptureState);
                Board.Pawns.Add(new Pawn(move.CaptureState, move.Capture, _pawnPrefab, _boardView, transform));
            }
        }

        public void PassTurn()
        {
            _currentPlayer = (_currentPlayer + 1) % _players.Count;
            _players[_currentPlayer].StartTurn(this);
        }

        public void OnResetClicked()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void OnUndoClicked()
        {
            if (CurrentTeam != _localTeam)
            {
                return;
            }
            
            bool reversedOpponentMoves = false;
            bool reversedLastTurn = false;
            IsMidCombo = false;
            
            while (MovesHistory.Count > 0)
            {
                RecordedMove move = MovesHistory.Peek();
                if (move.Team != CurrentTeam)
                {
                    if (reversedLastTurn)
                    {
                        break;
                    }
                    reversedOpponentMoves = true;
                }
                else if (reversedOpponentMoves && move.Team == CurrentTeam)
                {
                    reversedLastTurn = true;
                }
                
                UndoLastMove();
            }
            
            _players[_currentPlayer].StartTurn(this);
        }
    }
}