using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PieceCreator))]//dobavlja kao import
public class ChessGame : MonoBehaviour
{
    private enum GameState {Init, Play, Finished}
    [SerializeField] private BoardLayout startingBoardLayout;
    [SerializeField] private Board board;
    private PieceCreator pieceCreator;
    private Player whitePlayer;
    private Player blackPlayer;
    private Player activePlayer;
    private GameState state;

    
    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }

    private void StartGame() {
        SetGameState(GameState.Init);
        board.SetDependencies(this);
        CreatePieceFromLayout(startingBoardLayout);
        activePlayer = whitePlayer;
        AllPossibleMoves(activePlayer);
        SetGameState(GameState.Play);
    }

    private void SetGameState(GameState state) {
        this.state = state;
    }

    public bool GameInProgress() {
        return state == GameState.Play;
    }

    private void Awake() {
        SetDependencies();
        CreatePlayers();
    }

    private void SetDependencies() {
        pieceCreator = GetComponent<PieceCreator>();
    }

    private void CreatePlayers() {
        whitePlayer = new Player(TeamColor.white, board);
        blackPlayer = new Player(TeamColor.black, board);
    }

    public bool IsColorTurn(TeamColor color) {
        return activePlayer.color == color;
    }

    private void CreatePieceFromLayout(BoardLayout layout) {
        for(int i = 0; i<layout.getNumberOfPieces(); i++) {
            Vector2Int squareCoordinates = layout.getSquareCoordinates(i);
            TeamColor color = layout.getPieceColor(i);
            string name = layout.getPieceName(i);
            InitPiece(squareCoordinates, color, name);
        }
    }

    public void InitPiece(Vector2Int coords, TeamColor color, string typeName) {
        //Type tip = Type.GetType(typeName);
        Piece newPiece = pieceCreator.CreatePiece(typeName).GetComponent<Piece>();//dobavlja komponentu figura
        newPiece.SetData(coords, color, board);
        Material teamMaterial = pieceCreator.GetTeamMaterial(color);
        newPiece.SetMaterial(teamMaterial);

        board.setPiecOnBoard(coords, newPiece);
        Player currentPlayer = color == TeamColor.white ? whitePlayer : blackPlayer;
        currentPlayer.AddPiece(newPiece);
    } 

    private Player GetNextActivePlayer(Player player) {
        return player == whitePlayer ? blackPlayer : whitePlayer;
    } 

    private void ChangeActivePlayer() {
        activePlayer = GetNextActivePlayer(activePlayer);
    }

    private void AllPossibleMoves(Player player) {
        player.GeneratePossibleMoves();
    }

    public void EndTurn() {
        AllPossibleMoves(activePlayer);
        AllPossibleMoves(GetNextActivePlayer(activePlayer));
        /*if(GameIsFinished())
            EndGame();
        else*/
            ChangeActivePlayer();
    }

    private bool GameIsFinished() {
        Piece[] givingCheck = activePlayer.GetPiecesGivingCheck<King>();
        if(givingCheck.Length > 0) {
            Player opponent = GetNextActivePlayer(activePlayer);
            Piece checkedKing = opponent.GetPieceOfType<King>().FirstOrDefault();
            opponent.RemoveAttackingMoves<King>(activePlayer, checkedKing);

            int availableKingMoves = checkedKing.possibleMoves.Count;
            if(availableKingMoves == 0){
                bool canCoverKing = opponent.CanCoverPiece<King>(activePlayer);
                if(!canCoverKing)
                    return true;
            }
        }
        return false;
    }

    private void EndGame() {
        Piece[] givingCheck = activePlayer.GetPiecesGivingCheck<King>();
        Debug.Log(givingCheck.Length);
        SetGameState(GameState.Finished);
    }

    public void RemoveMovesThatExposePiece<T>(Piece piece) where T : Piece {
        activePlayer.RemoveAttackingMoves<T>(GetNextActivePlayer(activePlayer), piece);
    }  

    public void OnPieceRemoved(Piece piece) {
        Player pieceOwner = (piece.color == TeamColor.white) ? whitePlayer : blackPlayer;
        pieceOwner.RemovePiece(piece);
        Destroy(piece.gameObject);
    }
}
