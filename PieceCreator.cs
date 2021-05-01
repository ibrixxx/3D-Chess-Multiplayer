using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceCreator : MonoBehaviour
{
   [SerializeField] private GameObject[] pieceSpecs;
   [SerializeField] private Material blackMaterial;
   [SerializeField] private Material whiteMaterial;

   private Dictionary<string, GameObject> nameToPiece = new Dictionary<string, GameObject>();

   private void Awake() {
       foreach(var piece in pieceSpecs) {
           nameToPiece.Add(piece.GetComponent<Piece>().GetType().ToString(), piece);
       }
   }

   public GameObject CreatePiece(string type) {
       GameObject specs = nameToPiece[type];
       if(specs) {
           GameObject newPiece = Instantiate(specs);//clonira
           return newPiece;
       }
       return null;
   }

   public Material GetTeamMaterial(TeamColor color) {
       return color == TeamColor.white ? whiteMaterial : blackMaterial;
   }
}
