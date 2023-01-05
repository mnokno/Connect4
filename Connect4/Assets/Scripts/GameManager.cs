using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using C4UI;
using UnityEngine.UIElements;

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

    // Start is called before the first frame update
    void Start()
    {
        gameCircles = new GameCircle[7, 6];
        gameBoard = new GameBoard();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (aiStarts)
        {
            aiStarts = false;
            //RequestAIMove(-1);
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
    /// Make the given move and returns exit code
    /// </summary>
    /// <param name="x">X position of the move</param>
    /// <param name="y">Y position of the move</param>
    /// <returns>Returns 0 if the move was successfully made, -1 if the give move was illegal</returns>
    public int MakeMove(int x, int y)
    {
        if (IsMoveLegal(x, y))
        {
            // Make the move on the board
            gameBoard.MakeMove(x, y);
            // Updates UI
            gameCircles[x, y].ChangeColor((CircleColor)(int)gameBoard.GetGameBoard()[x, y]);
            // Logs who won if the game has ended
            if (gameBoard.GetGameState() != GameState.ON_GOING)
            {
                Debug.Log(gameBoard.GetGameState().ToString());
                UIManager uIManager = FindObjectOfType<UIManager>();
                uIManager.gameUIEventSystem.SetActive(false);
                uIManager.gameOverPage.rootVisualElement.style.display = DisplayStyle.Flex;
                if (gameBoard.GetGameState() == GameState.DRAW)
                {
                    FindObjectOfType<UI_GameOver>().ShowGameResult(GameResult.DRAW);
                }
                else
                {
                    if (aiStarts)
                    {
                        FindObjectOfType<UI_GameOver>().ShowGameResult(gameBoard.GetGameState() == GameState.YELLOW_WON ? GameResult.LOSS : GameResult.WIN);
                    }
                    else
                    {
                        FindObjectOfType<UI_GameOver>().ShowGameResult(gameBoard.GetGameState() == GameState.YELLOW_WON ? GameResult.WIN : GameResult.LOSS);
                    }
                }
            }
            // Return exit code, move was successfully made
            return 0;
        }
        else
        {
            // Returns exit code, the given move was invalid
            return -1;
        }
    }

    
    /// <summary>
    /// Checks if the given move is legal
    /// </summary>
    /// <param name="x">X position of the move to validate</param>
    /// <param name="y">Y position of the move to validate</param>
    /// <returns>True if the move is legal, false otherwise</returns>
    public bool IsMoveLegal(int x, int y)
    {
        return gameBoard.IsMoveLegal(x, y);
    }
}
