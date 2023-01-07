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
    /// Flag used to decide who start the game
    /// </summary>
    [SerializeField] private bool aiStarts = false;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        gameCircles = new GameCircle[7, 6];
        gameBoard = new GameBoard();
    }

    /// <summary>
    /// Resets the game to initial state
    /// </summary>
    public void ResetGame()
    {
        // Resets visuals
        foreach (GameCircle gameCircle in gameCircles)
        {
            gameCircle.ResetCircle();
        }
        // Resets logical state
        gameBoard = new GameBoard();
        // Tells the server to reset the game
        FindObjectOfType<NetworkGate>().NewGameServerRpc();
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
                ShowWiningLines();
                Debug.Log(gameBoard.GetGameState().ToString());
                StartCoroutine(ShowGameOverUIAfter(0.5f));
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

    /// <summary>
    /// Displays wining lines on the board
    /// </summary>
    private void ShowWiningLines()
    {
        foreach (GameBoard.WinigLine winigLine in gameBoard.GetWiningLines())
        {
            foreach (Vector2 pos in winigLine.GetCordinates()) 
            {
                gameCircles[(int)pos.x, (int)pos.y].ShowIsWining();
            }
        }
    }

    /// <summary>
    /// Show the game over UI after the given amount of time
    /// </summary>
    /// <param name="time">Time to wait before showing the UI</param>
    private IEnumerator ShowGameOverUIAfter(float time)
    {
        yield return new WaitForSecondsRealtime(time);
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
}
