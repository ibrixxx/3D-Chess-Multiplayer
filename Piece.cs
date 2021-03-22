using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IObject))]
[RequireComponent(typeof(MaterialSetter))]
public abstract class Piece : MonoBehaviour
{
   [SerializeField] private MaterialSetter materialSetter;
   private IObject middleware;

   protected Board board {protected get; set;}
   protected Vector2Int currentSquare {get; set;}
   protected TeamColor color {get; set;}
   public List<Vector2Int> possibleMoves;
   public abstract List<Vector2Int> SelectSquares();

   private void Awake() {
       possibleMoves = new List<Vector2Int>();
       middleware = GetComponent<IObject>();
       materialSetter = GetComponent<MaterialSetter>();
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

   public virtual void MovePiece(Vector2Int coords) {}

   protected void AddToMoves(Vector2Int coords) {
       possibleMoves.Add(coords);
   }
}
