using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TeamColor{
    white, black
}


public enum PieceType{
    Pawn, Rook, Knight, Bishop, Queen, King
}



[CreateAssetMenu(menuName = "Scriptable Objects/Board/Layout")]
public class BoardLayout : ScriptableObject
{
    [Serializable]
    private class BoardSquare {
        public Vector2Int position;
        public PieceType pieceType;
        public TeamColor teamColor;
    }

    [SerializeField] private BoardSquare[] boardSquares;
    
    public int getNumberOfPieces() {
        return boardSquares.Length;
    }

    public Vector2Int getSquareCoordinates(int index) {
        if(boardSquares.Length <= index || index < 0) {
            Debug.LogError("Index out of range");
            return new Vector2Int(-1, -1);
        }
        return new Vector2Int(boardSquares[index].position.x -1, boardSquares[index].position.y -1);
    }

    public string getPieceName(int index) {
        if(boardSquares.Length <= index || index < 0) {
            Debug.LogError("Index out of range");
            return "";
        }
        return boardSquares[index].pieceType.ToString();
    }

    public TeamColor getPieceColor(int index) {
        if(boardSquares.Length <= index || index < 0) {
            Debug.LogError("Index out of range");
            return TeamColor.black;
        }
        return boardSquares[index].teamColor;
    }
}
