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
            MakeAIMove(-1);
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
            MakeAIMove(x);
        }
    }

    void MakeAIMove(int file)
    {
        int move = marlinClient.GetMove(file, 1000);
        int x = move % 7;
        int y = (move - x) / 7;

        if (gameBoard.IsMoveLegal(x, y))
        {
            gameBoard.MakeMove(x, y);
            gameCircles[x,y].ChangeColor((CircleColor)(int)gameBoard.GetGameBoard()[x, y]);
            if (gameBoard.GetGameState() != GameState.ON_GOING)
            {
                Debug.Log(gameBoard.GetGameState().ToString());
            }
        }
    }
}
