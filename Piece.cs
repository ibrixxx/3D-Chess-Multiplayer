using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IObjectTweener))]
[RequireComponent(typeof(MaterialSetter))]
public abstract class Piece : MonoBehaviour
{
   [SerializeField] private MaterialSetter materialSetter;
   private IObjectTweener middleware;

   public Board board {protected get; set;}
   public Vector2Int currentSquare {get; set;}
   public TeamColor color {get; set;}
   public bool hasMoved {get; private set;}
   public List<Vector2Int> possibleMoves;
   public abstract List<Vector2Int> SelectSquares();

   private void Awake() {
       possibleMoves = new List<Vector2Int>();
       middleware = GetComponent<IObjectTweener>();
       materialSetter = GetComponent<MaterialSetter>();
       hasMoved = false;
   }

   public void SetMaterial(Material material) {
       if(materialSetter == null)
          materialSetter = GetComponent<MaterialSetter>();
       materialSetter.SetSingleMaterial(material);
   }

   public bool IsSameTeam(Piece piece) {
       return color == piece.color;
   }

   public bool CanMoveTo(Vector2Int coords) {
       return possibleMoves.Contains(coords);
   }

   public void SetData(Vector2Int coords, TeamColor color, Board board) {
       this.color = color;
       currentSquare = coords;
       this.board = board;
       transform.position = board.CalculatePosition(coords);
   }

   public virtual void MovePiece(Vector2Int coords) {
       Vector3 targetPosition = board.CalculatePosition(coords);
       currentSquare = coords;
       middleware.MoveTo(transform, targetPosition);
       hasMoved = true;
   }

   protected void AddToMoves(Vector2Int coords) {
       possibleMoves.Add(coords);
   }

   public bool IsAttackingPieceOfType<T>() where T : Piece {
       foreach (var item in possibleMoves)
       {
           if(board.GetPieceOnSquare(item) is T);
              return true;
       }
       return false;
   }

   protected Piece GetPieceInDirection<T>(TeamColor color, Vector2Int direction) where T:Piece {
       for(int i=1; i<Board.BOARD_SIZE; i++) {
           Vector2Int nextCoords = currentSquare + direction * i;
           Piece piece = board.GetPieceOnSquare(nextCoords);
           if(!board.CoordsAreOnBoard(nextCoords))
                return null;
           if(piece != null) {
               if(piece.color != color || !(piece is T))
                    return null;
               else if(piece.color == color && piece is T)
                    return piece;
           }
       }
       return null;
   }
}
