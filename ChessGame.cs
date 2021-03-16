using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PieceCreator))]//dobavlja kao import
public class ChessGame : MonoBehaviour
{
    [SerializeField] private BoardLayout startingBoardLayout;
    [SerializeField] private Board board;
    private PieceCreator pieceCreator;

    
    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }

    private void StartGame() {
        CreatePieceFromLayout(startingBoardLayout);
    }

    private void Awake() {
        SetDependencies();
    }

    private void SetDependencies() {
        pieceCreator = GetComponent<PieceCreator>();
    }

    private void CreatePieceFromLayout(BoardLayout layout) {
        for(int i = 0; i<layout.getNumberOfPieces(); i++) {
            Vector2Int squareCoordinates = layout.getSquareCoordinates(i);
            TeamColor color = layout.getPieceColor(i);
            string name = layout.getPieceName(i);
            InitPiece(squareCoordinates, color, name);
        }
    }

    private void InitPiece(Vector2Int coords, TeamColor color, string typeName) {
        //Type tip = Type.GetType(typeName);
        Piece newPiece = pieceCreator.CreatePiece(typeName).GetComponent<Piece>();//dobavlja komponentu figura
        newPiece.SetData(coords, color, board);
        Material teamMaterial = pieceCreator.GetTeamMaterial(color);
        newPiece.SetMaterial(teamMaterial);
    }    
}
