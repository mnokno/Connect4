using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Stores reference's to gameCircles
    /// </summary>
    private GameCircle[,] gameCircles;
    /// <summary>
    /// Stores reference's to gameBoard (logical structures following game rules)
    /// </summary>
    private GameBoard gameBoard;
    /// <summary>
    /// If true, the AI will make the first move
    /// </summary>
    [SerializeField] bool aiStarts = false;

    private MarlinClient marlinClient;

    private int aiMove = -1;

    // Start is called before the first frame update
    void Start()
    {
        gameCircles = new GameCircle[7, 6];
        gameBoard = new GameBoard();
        marlinClient = new MarlinClient();
        marlinClient.Connect();
        marlinClient.InitGame(5000);


    }
    
    // Update is called once per frame
    void Update()
    {
        if (aiStarts)
        {
            aiStarts = false;
            RequestAIMove(-1);
        }
        if (aiMove != -1)
        {
            MakeAIMove(aiMove);
            aiMove = -1;
        }
    }

    /// <summary>
    /// Adds a gameCircle to the gameCircles array
    /// </summary>
    /// <param name="gameCircle">gameCircle to register</param>
    public void RegisterGameCircle(GameCircle gameCircle)
    {
        gameCircles[gameCircle.GetX(), gameCircle.GetY()] = gameCircle;
    }

    /// <summary>
    /// Handles user input
    /// </summary>
    /// <param name="gameCircle">Clicked circle</param>
    public void HandleClicked(GameCircle gameCircle)
    {
        // Prevent the user form making a move while the engine is thinking/calculating
        if (!marlinClient.IsAwaitingReply())
        {
            int x = gameCircle.GetX();
            int y = gameCircle.GetY();
            if (gameBoard.IsMoveLegal(x, y))
            {
                gameBoard.MakeMove(x, y);
                gameCircle.ChangeColor((CircleColor)(int)gameBoard.GetGameBoard()[x, y]);
                if (gameBoard.GetGameState() != GameState.ON_GOING)
                {
                    Debug.Log(gameBoard.GetGameState().ToString());
                }
                RequestAIMove(x);
            }
        }
    }

    /// <summary>
    /// Make asynchrony request to server for AI move
    /// </summary>
    /// <param name="file"></param>
    private void RequestAIMove(int file)
    {
        marlinClient.GetMoveAsynch(file, 1000, (int result) => aiMove = result);
    }

    /// <summary>
    /// Makes the AI move once the server has replayed
    /// </summary>
    /// <param name="move">Move to make</param>
    private void MakeAIMove(int move)
    {
        int x = move % 7;
        int y = (move - x) / 7;

        if (gameBoard.IsMoveLegal(x, y))
        {
            gameBoard.MakeMove(x, y);
            gameCircles[x, y].ChangeColor((CircleColor)(int)gameBoard.GetGameBoard()[x, y]);
            if (gameBoard.GetGameState() != GameState.ON_GOING)
            {
                Debug.Log(gameBoard.GetGameState().ToString());
            }
        }
    }
}
