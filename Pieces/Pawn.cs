using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
   public override List<Vector2Int> SelectSquares() {
       possibleMoves.Clear();
       Vector2Int direction = color == TeamColor.white ? Vector2Int.up : Vector2Int.down;
       int range = hasMoved ? 1 : 2;
       for(int i = 1; i<=range; i++){
           Vector2Int newCoords = currentSquare + direction * i;
           Piece piece = board.GetPieceOnSquare(newCoords);
           if(!board.CoordsAreOnBoard(newCoords))
              break;
           if(piece == null)
              AddToMoves(newCoords);
           else
              break;
       }
       Vector2Int[] diagonalDirections = new Vector2Int[] {new Vector2Int(1, direction.y), new Vector2Int(-1, direction.y)};
       for(int i = 0; i<diagonalDirections.Length; i++) {
           Vector2Int newCoords = currentSquare + diagonalDirections[i];
           Piece piece = board.GetPieceOnSquare(newCoords);
           if(!board.CoordsAreOnBoard(newCoords))
              break;
           if(piece != null && !piece.IsSameTeam(this))
             AddToMoves(newCoords);
       }
       return possibleMoves;
   }

   public override void MovePiece(Vector2Int coords) {
       base.MovePiece(coords);
       int oppositeSideCoord = color ==  TeamColor.white ? Board.BOARD_SIZE -1 : 0;
       if(oppositeSideCoord == currentSquare.y)
         board.PromoteQueen(this);
   }

}
