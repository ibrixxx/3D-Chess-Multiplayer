using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
   private Vector2Int[] directions = new Vector2Int[] {
       new Vector2Int(0,1),
       new Vector2Int(1,1),
       new Vector2Int(-1,-1),
       new Vector2Int(1,0),
       new Vector2Int(-1,0),
       new Vector2Int(-1,1),
       new Vector2Int(0,-1),
       new Vector2Int(1,-1),
   };

   private Vector2Int rightCastling;
   private Vector2Int leftCastling;
   private Piece leftRook;
   private Piece rightRook;

   public override List<Vector2Int> SelectSquares() {
       possibleMoves.Clear();
       SetStandardMoves();
       SetCastlingMoves();
       return possibleMoves;
   }
 
   private void SetStandardMoves() {
       foreach (var dir in directions)
       {
           for(int i = 0; i<=Board.BOARD_SIZE; i++){
               Vector2Int newCoords = currentSquare + dir * i;
               Piece piece = board.GetPieceOnSquare(newCoords);
               if(!board.CoordsAreOnBoard(newCoords))
                  break;
               if(piece == null) 
                  AddToMoves(newCoords);
               if(piece != null && !piece.IsSameTeam(this)){
                  AddToMoves(newCoords);
                  break;
               } 
               else
                   break;
           }
       }
   }

   private void SetCastlingMoves() {
        if(hasMoved)
            return;
        leftRook = GetPieceInDirection<Rook>(color, Vector2Int.left);
        if(leftRook && !leftRook.hasMoved){
            leftCastling = currentSquare + Vector2Int.left * 2;
            possibleMoves.Add(leftCastling);
        }

        rightRook = GetPieceInDirection<Rook>(color, Vector2Int.right);
        if(rightRook && !rightRook.hasMoved){
            rightCastling = currentSquare + Vector2Int.right * 2;
            possibleMoves.Add(rightCastling);
        }

   }

   public override void MovePiece(Vector2Int coords) {
       base.MovePiece(coords);
       if(coords == leftCastling){
           board.UpdateBoard(coords + Vector2Int.right, leftRook.currentSquare, leftRook, null);
           leftRook.MovePiece(coords + Vector2Int.right);
       }
       else if(coords == rightCastling){
           board.UpdateBoard(coords + Vector2Int.left, rightRook.currentSquare, rightRook, null);
           rightRook.MovePiece(coords + Vector2Int.left);
       }
   }
}
