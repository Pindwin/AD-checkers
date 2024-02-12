using System.Collections.Generic;
using NUnit.Framework;
using pindwin.Board;
using pindwin.Moves;

public class CheckersBoard_Tests
{
    private CheckersBoard _board;
    private List<PossibleMove> _possibleMoves;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _possibleMoves = new List<PossibleMove>();
    }
    
    [SetUp]
    public void CreateBoard()
    {
        _board = new CheckersBoard(new TileState[64]);
        _possibleMoves.Clear();
    }
    
    [Test]
    public void kill_moves_are_selected_over_simple_moves()
    {
        //add 2 pawns that can kill each other
        _board.ForceState(new Tile(2, 2), TileState.Pawn | TileState.White);
        _board.ForceState(new Tile(3, 3), TileState.Pawn);
        
        _board.GetAllPossibleMoves(_possibleMoves, TileState.White.Team());
        
        Assert.AreEqual(1, _possibleMoves.Count);
        
        _possibleMoves.Clear();
        _board.GetAllPossibleMoves(_possibleMoves, TileState.White.Team() * -1);
        
        Assert.AreEqual(1, _possibleMoves.Count);
    }
    
    [Test]
    public void queen_can_kill_from_a_distance_but_pawn_cannot()
    {
        //add queen that can kill pawn from a distance and pawn that can just move as usual
        _board.ForceState(new Tile(1, 1), TileState.Pawn | TileState.White);
        _board.ForceState(new Tile(4, 4), TileState.Pawn | TileState.Promoted);
        
        bool canKill = false;
        _board.GetAllPossibleMoves(_possibleMoves, TileState.White.Team(), ref canKill);
        
        Assert.AreEqual(2, _possibleMoves.Count);
        Assert.False(canKill);

        canKill = false;
        _possibleMoves.Clear();
        _board.GetAllPossibleMoves(_possibleMoves, TileState.White.Team() * -1, ref canKill);
        
        Assert.AreEqual(1, _possibleMoves.Count);
        Assert.True(canKill);
    }
    
    [Test]
    public void pawns_can_move_only_forward_when_not_attacking()
    {
        //set up lonely pawn
        _board.ForceState(new Tile(2, 2), TileState.Pawn | TileState.White);
        
        _board.GetAllPossibleMoves(_possibleMoves, TileState.White.Team());
        
        Assert.AreEqual(2, _possibleMoves.Count);
    }

    [Test]
    public void pawns_can_move_forward_and_backward_when_attacking()
    {
        //set up pawn surrounded by enemies
        _board.ForceState(new Tile(2, 2), TileState.Pawn | TileState.White);
        _board.ForceState(new Tile(1, 1), TileState.Pawn);
        _board.ForceState(new Tile(3, 1), TileState.Pawn);
        _board.ForceState(new Tile(3, 3), TileState.Pawn);
        _board.ForceState(new Tile(1, 3), TileState.Pawn);
        
        _board.GetAllPossibleMoves(_possibleMoves, TileState.White.Team());
        
        Assert.AreEqual(4, _possibleMoves.Count);
    }

    [Test]
    public void queen_can_jump_long_distance_over_pawns()
    {
        _board.ForceState(new Tile(0, 0), TileState.Pawn | TileState.Promoted | TileState.White);
        _board.ForceState(new Tile(1, 1), TileState.Pawn);
        
        _board.GetAllPossibleMoves(_possibleMoves, TileState.White.Team());
        
        Assert.AreEqual(6, _possibleMoves.Count);
    }
    
    

    [Test]
    public void queen_cannot_jump_over_double_pawns()
    {
        _board.ForceState(new Tile(0, 0), TileState.Pawn | TileState.Promoted | TileState.White);
        _board.ForceState(new Tile(1, 1), TileState.Pawn);
        _board.ForceState(new Tile(2, 2), TileState.Pawn);
        
        _board.GetAllPossibleMoves(_possibleMoves, TileState.White.Team());
        
        Assert.AreEqual(0, _possibleMoves.Count);
    }
}
