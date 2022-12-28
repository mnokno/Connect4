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
    
    // Start is called before the first frame update
    void Start()
    {
        gameCircles = new GameCircle[7, 6];
        gameBoard = new GameBoard();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        }
    }
}
