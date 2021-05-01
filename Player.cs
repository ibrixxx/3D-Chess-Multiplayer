using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player 
{
    public TeamColor color {get; set;}
    public Board board {get; set;}
    public List<Piece> activePieces {get; private set;}

    public Player(TeamColor color, Board board) {
        this.color = color;
        this.board = board;
        activePieces = new List<Piece>();
    }

    public void AddPiece(Piece piece) {
        if(!activePieces.Contains(piece))
            activePieces.Add(piece);
    }

    public void RemovePiece(Piece piece) {
        if(activePieces.Contains(piece))
            activePieces.Remove(piece);
    }

    public void GeneratePossibleMoves() {
        foreach (var piece in activePieces)
        {
            if(board.HasPiece(piece))
                piece.SelectSquares();
        }
    }

    public Piece[] GetPiecesGivingCheck<T>() where T : Piece {
        return activePieces.Where(p => p.IsAttackingPieceOfType<T>()).ToArray();
    } 

    public Piece[] GetPieceOfType<T>() where T : Piece {
        return activePieces.Where(p => p is T).ToArray();
    }  

    public void RemoveAttackingMoves<T>(Player opponent, Piece selectedPiece) where T : Piece {
        List<Vector2Int> coordsToRemove = new List<Vector2Int>();
        coordsToRemove.Clear();
        foreach (var item in selectedPiece.possibleMoves)
        {
            Piece pieceOnSquare = board.GetPieceOnSquare(item);
            board.UpdateBoard(item, selectedPiece.currentSquare, selectedPiece, null);
            opponent.GeneratePossibleMoves();
            if(opponent.CheckIfIsAttackingPiece<T>())
                coordsToRemove.Add(item);
            board.UpdateBoard(selectedPiece.currentSquare, item, selectedPiece, pieceOnSquare);
        }
        foreach (var item in coordsToRemove)
        {
            selectedPiece.possibleMoves.Remove(item);
        }
    }

    private bool CheckIfIsAttackingPiece<T>() where T : Piece {
        foreach (var item in activePieces)
        {
            if(board.HasPiece(item) && item.IsAttackingPieceOfType<T>())
                return true;
        }
        return false;
    }

    public bool CanCoverPiece<T>(Player activePlayer) where T : Piece {
        foreach (var piece in activePieces)
        {
            foreach (var coords in piece.possibleMoves)
            {
                Piece pieceOnCoords = board.GetPieceOnSquare(coords);
                board.UpdateBoard(coords, piece.currentSquare, piece, null);
                activePlayer.GeneratePossibleMoves();
                if(!activePlayer.CheckIfIsAttackingPiece<T>()){
                    board.UpdateBoard(piece.currentSquare, coords, piece, pieceOnCoords);
                    return true;
                }
                board.UpdateBoard(piece.currentSquare, coords, piece, pieceOnCoords);
            }
        }
        return false;
    }
}
