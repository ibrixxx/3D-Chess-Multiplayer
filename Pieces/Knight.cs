using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
   private Vector2Int[] offsets = new Vector2Int[] {
       new Vector2Int(2,1),
       new Vector2Int(-2,1),
       new Vector2Int(2,-1),
       new Vector2Int(-2,-1),
       new Vector2Int(1,2),
       new Vector2Int(1,-2),
       new Vector2Int(-1,2),
       new Vector2Int(-1,-2),
   };

   public override List<Vector2Int> SelectSquares() {
       possibleMoves.Clear();
       foreach (var offset in offsets)
       {
           Vector2Int newCoords = currentSquare + offset;
           Piece piece = board.GetPieceOnSquare(newCoords);
           if(!board.CoordsAreOnBoard(newCoords))
             continue;
           if(piece == null || !piece.IsSameTeam(this))
             AddToMoves(newCoords);
       }
       return possibleMoves;
   }
}
