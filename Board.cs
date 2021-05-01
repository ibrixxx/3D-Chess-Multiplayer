using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SquareSelector))]
public class Board : MonoBehaviour
{
    public const int BOARD_SIZE = 8;
    [SerializeField] private Transform bottomLeftSquareTransform;
    [SerializeField] private float squareSize;

    private Piece[,] grid;
    private Piece selectedPiece;
    private ChessGame controller;
    private SquareSelector squareSelector;

    private void Awake() {
        squareSelector = GetComponent<SquareSelector>();
        CreateGrid();    
    }

    public void SetDependencies(ChessGame controller) {
        this.controller = controller;
    }

    private void CreateGrid() {
        grid = new Piece[BOARD_SIZE, BOARD_SIZE];
    }

    public Vector3 CalculatePosition(Vector2Int coords) {
        return bottomLeftSquareTransform.position + new Vector3(coords.x * squareSize, 0f,coords.y * squareSize);
    }

    private Vector2Int CalculateCoords(Vector3 position) {
        int x = Mathf.FloorToInt((position).x / squareSize) + (BOARD_SIZE / 2);
        int y = Mathf.FloorToInt((position).z / squareSize) + (BOARD_SIZE / 2);
        return new Vector2Int(x, y);
    }

    public bool CoordsAreOnBoard(Vector2Int coords) {
        if(coords.x < 0 || coords.y < 0 || coords.x >= BOARD_SIZE || coords.y >= BOARD_SIZE)
            return false;
        return true;
    }

    public void setPiecOnBoard(Vector2Int coords, Piece newPiece) {
        if(CoordsAreOnBoard(coords))
            grid[coords.x, coords.y] = newPiece;
    }
 
    public Piece GetPieceOnSquare(Vector2Int coords) {
        if(CoordsAreOnBoard(coords))
            return grid[coords.x, coords.y];
        return null;
    }

    public void OnSquareSelected(Vector3 position) {
        if(!controller.GameInProgress())
            return;
        Vector2Int coords = CalculateCoords(position);
        Piece piece = GetPieceOnSquare(coords);
        if(selectedPiece) {
            if(piece != null && selectedPiece == piece)
                UnselectPiece();
            else if(piece != null && selectedPiece != piece && controller.IsColorTurn(piece.color))
                SelectPiece(piece);
            else if(selectedPiece.CanMoveTo(coords))
                OnPeaceMove(coords, selectedPiece);
        }
        else {
            if(piece != null && controller.IsColorTurn(piece.color))
                SelectPiece(piece);
        }
    }

    private void OnPeaceMove(Vector2Int coords, Piece selectedPiece) {
        TryToTakePiece(coords);
        UpdateBoard(coords, selectedPiece.currentSquare, selectedPiece, null);
        selectedPiece.MovePiece(coords);
        UnselectPiece();
        EndTurn();
    }

    private void TryToTakePiece(Vector2Int coords) {
        Piece piece = GetPieceOnSquare(coords);
        if(piece != null && !selectedPiece.IsSameTeam(piece))
            TakePiece(piece);
    }

    private void TakePiece(Piece piece) {
        grid[piece.currentSquare.x, piece.currentSquare.y] = null;
        controller.OnPieceRemoved(piece);
    }

    public void UpdateBoard(Vector2Int newCoords, Vector2Int oldCoords, Piece newPiece, Piece oldPiece) {
        grid[oldCoords.x, oldCoords.y] = oldPiece;
        grid[newCoords.x, newCoords.y] = newPiece;
    }

    private void EndTurn() {
        controller.EndTurn();
    }

    private void SelectPiece(Piece piece) {
        //if(piece.hasMoved)
          //  controller.RemoveMovesThatExposePiece<King>(piece);
        selectedPiece = piece;
        List<Vector2Int> selection = selectedPiece.possibleMoves;
        ShowSelectionSquares(selection);
    }

    private void ShowSelectionSquares(List<Vector2Int> selection) {
        Dictionary<Vector3, bool> squareData = new Dictionary<Vector3, bool>();
        for(int i = 0; i<selection.Count; i++) {
            Vector3 position = CalculatePosition(selection[i]);
            bool isFree = GetPieceOnSquare(selection[i]) == null;
            squareData.Add(position, isFree);
        }
        squareSelector.ShowSelection(squareData);
    }

    private void UnselectPiece() {
        selectedPiece = null;
        squareSelector.ClearSelection();
    }

    public bool HasPiece(Piece piece) {
        for(int i = 0; i<BOARD_SIZE; i++)
            for(int j = 0; j< BOARD_SIZE; j++)
                if(grid[i,j] == piece)
                    return true;
        return false;
    }

    public void PromoteQueen(Piece piece) {
        TakePiece(piece);
        controller.InitPiece(piece.currentSquare, piece.color, "Queen");
    }
}
