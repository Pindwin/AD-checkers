using System.Collections.Generic;
using pindwin.Board;
using pindwin.Board.View;
using pindwin.Moves;
using pindwin.Pawns;
using pindwin.Player;
using pindwin.States;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace pindwin
{
	public class CheckersGameController : MonoBehaviour, IPossibleMoveSource
	{
		[SerializeField] private BoardView _boardView;
		[SerializeField] private PawnView _pawnPrefab;
		private readonly Stack<RecordedMove> _movesHistory = new();

		private readonly List<Pawn> _pawns = new();
		private readonly List<IPlayer> _players = new();

		private readonly Dictionary<GameStateType, GameState> _states = new()
		{
			{ GameStateType.PawnSelection, new PawnSelection() },
			{ GameStateType.TargetSelection, new TargetSelection() }
		};

		private int _currentPlayer;

		private GameState _currentState;

		private CheckersBoard Board { get; set; }
		public Tile SelectedTile { get; private set; } = Tile.NullTile;
		public int CurrentTeam => _players[_currentPlayer].Team;
		public int LocalTeam { get; } = TileState.White.Team();
		public List<PossibleMove> PossibleMovesBuffer { get; } = new();
		public bool IsMidCombo { get; set; }

		private void Start()
		{
			_players.AddRange(new IPlayer[]
			{
				new LocalPlayer(LocalTeam),
				new AIPlayer(LocalTeam * -1, this)
			});

			var gameFactory = new CheckersGameFactory(_pawnPrefab, _boardView, transform);
			Board = gameFactory.SetupBoard(_pawns);
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
			if (SelectedTile.IsValid)
			{
				_boardView.GetTileByBoardCoord(SelectedTile.X, SelectedTile.Y).Selected = false;
			}

			SelectedTile = tile;
			if (SelectedTile.IsValid)
			{
				_boardView.GetTileByBoardCoord(tile.X, tile.Y).Selected = isSelected;
			}
		}

		public TileState GetStateByTile(Tile tile)
		{
			return Board[tile];
		}

		public void CommitMove(Tile from, Tile to, Tile capturedTile)
		{
			_movesHistory.Push(
				new RecordedMove(
					CurrentTeam,
					Board[from],
					from,
					to,
					capturedTile,
					capturedTile.IsValid ? Board[capturedTile] : TileState.Empty));

			TileState resultingState = Board.MakeMove(from, to);
			UpdatePawn(from, to, resultingState);

			if (capturedTile.IsValid && Board.Capture(capturedTile))
			{
				KillPawn(capturedTile);
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
			if (CurrentTeam != LocalTeam)
			{
				return;
			}

			bool reversedOpponentMoves = false;
			bool reversedLastTurn = false;
			IsMidCombo = false;

			while (_movesHistory.Count > 0)
			{
				RecordedMove move = _movesHistory.Peek();
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

		private void UndoLastMove()
		{
			if (_movesHistory.Count == 0)
			{
				return;
			}

			RecordedMove move = _movesHistory.Pop();
			Board.MakeMove(move.To, move.From);
			Board.ForceState(move.From, move.OriginalState);
			UpdatePawn(move.To, move.From, move.OriginalState);
			if (move.Capture.IsValid)
			{
				Board.ForceState(move.Capture, move.CaptureState);
				SpawnPawn(move.Capture, move.CaptureState);
			}
		}

		private void UpdatePawn(Tile from, Tile to, TileState resultingState)
		{
			Pawn pawn = _pawns.Find(p => p.Position == from);
			pawn.Position = to;
			pawn.IsQueen = resultingState.IsQueen();
		}

		private void KillPawn(Tile capturedTile)
		{
			Pawn pawn = _pawns.Find(p => p.Position == capturedTile);
			_pawns.Remove(pawn);
			pawn.IsDead = true;
			pawn.Position = Tile.NullTile;
		}

		private void SpawnPawn(Tile position, TileState state)
		{
			_pawns.Add(new Pawn(state, position, _pawnPrefab, _boardView, transform));
		}

		#region IPossibleMoveSource delegation

		public MoveValidity IsMoveValid(Tile from, Tile to, out Tile capturedTile)
		{
			return Board.IsMoveValid(from, to, out capturedTile);
		}

		public void GetAllPossibleMoves(List<PossibleMove> possibleMoves, int team)
		{
			Board.GetAllPossibleMoves(possibleMoves, team);
		}

		public void GetAllPossibleMoves(List<PossibleMove> possibleMoves, int team, ref bool kill)
		{
			Board.GetAllPossibleMoves(possibleMoves, team, ref kill);
		}

		public void GetPossibleMoves(Tile origin, List<PossibleMove> moves, ref bool kill)
		{
			Board.GetPossibleMoves(origin, moves, ref kill);
		}

		#endregion
	}
}