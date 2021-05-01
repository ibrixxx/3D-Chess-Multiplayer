using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
   private Vector2Int[] directions = new Vector2Int[] {
       new Vector2Int(1, 1),
       new Vector2Int(1, -1),
       new Vector2Int(-1, 1),
       new Vector2Int(-1, -1),
   };

   
   public override List<Vector2Int> SelectSquares() {
       possibleMoves.Clear();
       foreach (var dir in directions)  
       {
           for(int i = 1; i<=8; i++){
               Vector2Int newCoords = currentSquare + dir * i;
               Piece piece = board.GetPieceOnSquare(newCoords);
               if(!board.CoordsAreOnBoard(newCoords))
                  break;
               if(piece == null) 
                  AddToMoves(newCoords);
               else if(!piece.IsSameTeam(this)){
                  AddToMoves(newCoords);
                  break;
               } 
               else if(piece.IsSameTeam(this))
                   break;
           }
       }
       return possibleMoves;
   }
}
