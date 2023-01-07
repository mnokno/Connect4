using System.Collections.Generic;
using UnityEngine;

public class GameBoard
{
    /// <summary>
    /// The board is a 2D array of tiles
    /// </summary>
    private TileState[,] gameBoard = new TileState[7,6];
    /// <summary>
    /// Keeps track of the next player to move
    /// </summary>
    private Player nextPlayerToMove;
    /// <summary>
    /// Keeps track of the game state
    /// </summary>
    private GameState gameState;
    /// <summary>
    /// Keeps track of the number of moves made, if 42 and the game is still ON_GOING, it's a draw
    /// </summary>
    private int moveCount;

    /// <summary>
    /// Constructor for GameBoard, returns a new GameBoard with all trackers set to the new empty game
    /// </summary>
    public GameBoard()
    {
        // Initialize the board to empty
        for (int x = 0; x < 7; x++)
        {
            for (int y = 0; y < 6; y++)
            {
                gameBoard[x, y] = TileState.EMPTY;
            }
        }
        // Yellow always start the game
        nextPlayerToMove = Player.YELLOW;
        // Initially the game is on going
        gameState = GameState.ON_GOING;
        // No moves have been made
        moveCount = 0;
    }

    /// <summary>
    /// Make a move on the board (only if the move is legal) updating all trackers
    /// </summary>
    /// 
    /// <param name="x">X position of where we want to make the move</param>
    /// <param name="y">Y position of where we want to make the move</param>
    /// <returns>True if the move was played, false otherwise</returns>
    public bool MakeMove(int x, int y)
    {
        if (IsMoveLegal(x, y))
        {
            // Plays the move
            gameBoard[x, y] = (TileState)(int)nextPlayerToMove;
            // Updates next play to move
            nextPlayerToMove = nextPlayerToMove == Player.YELLOW ? Player.RED : Player.YELLOW;
            // Updates move count
            moveCount++;
            // Updates game state
            UpdateGameState();
            // The move was successfully played
            return true;
        }
        else
        {
            // The move could not be played become its not legal
            return false;
        }
    }

    /// <summary>
    /// Checks if the given move is legal
    /// </summary>
    /// 
    /// <param name="x">X position of the move</param>
    /// <param name="y">Y position of the move</param>
    /// <returns>True if the move is legal, false otherwise</returns>
    public bool IsMoveLegal(int x, int y)
    {
        if (gameState != GameState.ON_GOING)
        {
            return false;
        }
        
        if (gameBoard[x,y] != TileState.EMPTY)
        {   
            // if this tile is not empty than this move cant be legal
            return false;
        }
        else
        {
            // if the tile below this one is not the floor or empty than this move can't be legal, otherwise it has to be legal
            if (y == 0 || gameBoard[x, y -1] != TileState.EMPTY)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Updates game state brute-forcing all wining possibilities, this is not very efficient but this class
    /// will only by used to mange and keep track of the game for UI peruses not heavy calculation
    /// </summary>
    private void UpdateGameState()
    {
        // Checks vertical lines
        for (int x = 0; x < 7; x++)
        {
            TileState lineType = TileState.EMPTY;
            int count = 0;
            for (int y = 0; y < 6; y++)
            {
                if (gameBoard[x, y] == lineType)
                {
                    count++;
                }
                else
                {
                    lineType = gameBoard[x, y];
                    count = 1;
                }

                if (count == 4 && lineType != TileState.EMPTY)
                {
                    gameState = (GameState)(int)lineType;
                    return;
                }
            }
        }
        //Debug.Log("Vertical lines checked");
        // Checks horizontal lines
        for (int y = 0; y < 6; y++)
        {
            TileState lineType = TileState.EMPTY;
            int count = 0;
            for (int x = 0; x < 7; x++)
            {
                if (gameBoard[x, y] == lineType)
                {
                    count++;
                }
                else
                {
                    lineType = gameBoard[x, y];
                    count = 1;
                }

                if (count == 4 && lineType != TileState.EMPTY)
                {
                    gameState = (GameState)(int)lineType;
                    return;
                }
            }
        }
        //Debug.Log("Horizontal lines checked");
        // Checks diagonal line with positive gradient
        for (int x = 0; x < 7; x++)
        {
            TileState lineType = TileState.EMPTY;
            int count = 0;
            int currentX = x;
            int currentY = 0;          
            while (currentX < 7 && currentY < 6)
            {
                if (gameBoard[currentX, currentY] == lineType)
                {
                    count++;
                }
                else
                {
                    lineType = gameBoard[currentX, currentY];
                    count = 1;
                }
                if (count == 4 && lineType != TileState.EMPTY)
                {
                    gameState = (GameState)(int)lineType;
                    return;
                }
                currentX++;
                currentY++;
            }
        }
        for (int y = 0; y < 6; y++)
        {
            TileState lineType = TileState.EMPTY;
            int count = 0;
            int currentX = 0;
            int currentY = y;
            while (currentX < 7 && currentY < 6)
            {
                if (gameBoard[currentX, currentY] == lineType)
                {
                    count++;
                }
                else
                {
                    lineType = gameBoard[currentX, currentY];
                    count = 1;
                }
                if (count == 4 && lineType != TileState.EMPTY)
                {
                    gameState = (GameState)(int)lineType;
                    return;
                }
                currentX++;
                currentY++;
            }
        }
        //Debug.Log("Diagonal lines with positive gradient checked");
        // Checks diagonal line with negative gradient
        for (int x = 0; x < 7; x++)
        {
            TileState lineType = TileState.EMPTY;
            int count = 0;
            int currentX = x;
            int currentY = 5;
            while (currentX < 7 && currentY >= 0)
            {
                if (gameBoard[currentX, currentY] == lineType)
                {
                    count++;
                }
                else
                {
                    lineType = gameBoard[currentX, currentY];
                    count = 1;
                }
                if (count == 4 && lineType != TileState.EMPTY)
                {
                    gameState = (GameState)(int)lineType;
                    return;
                }
                currentX++;
                currentY--;
            }
        }
        for (int y = 5; y <= 0; y--)
        {
            TileState lineType = TileState.EMPTY;
            int count = 0;
            int currentX = 0;
            int currentY = y;
            while (currentX < 7 && currentY >= 0)
            {
                if (gameBoard[currentX, currentY] == lineType)
                {
                    count++;
                }
                else
                {
                    lineType = gameBoard[currentX, currentY];
                    count = 1;
                }
                if (count == 4 && lineType != TileState.EMPTY)
                {
                    gameState = (GameState)(int)lineType;
                    return;
                }
                currentX++;
                currentY--;
            }
        }
        //Debug.Log("Diagonal lines with negative gradient checked");
        // Check if its a draw
        if (moveCount == 42)
        {
            gameState = GameState.DRAW;
            return;
        }
    }
    
    /// <summary>
    /// Returns a list containing all wining lines in the position
    /// <returns></returns>
    public List<WinigLine> GetWiningLines()
    {
        List<WinigLine> winingLines = new List<WinigLine>();
        List<Vector2> currentLine = new List<Vector2>();

        // Checks vertical lines
        for (int x = 0; x < 7; x++)
        {
            TileState lineType = TileState.EMPTY;
            
            int count = 0;

            for (int y = 0; y < 6; y++)
            {
                if (gameBoard[x, y] == lineType)
                {
                    currentLine.Add(new Vector2(x, y));
                    count++;
                }
                else
                {
                    if (count >= 4 && lineType != TileState.EMPTY)
                    {
                        winingLines.Add(new WinigLine(currentLine.ToArray()));
                    }
                    currentLine.Clear();
                    currentLine.Add(new Vector2(x, y));
                    
                    lineType = gameBoard[x, y];
                    count = 1;
                }
            }
            
            if (count >= 4 && lineType != TileState.EMPTY)
            {
                winingLines.Add(new WinigLine(currentLine.ToArray()));
            }
            currentLine.Clear();
        }
        //Debug.Log("Vertical lines checked");
        // Checks horizontal lines
        for (int y = 0; y < 6; y++)
        {
            TileState lineType = TileState.EMPTY;
            int count = 0;
            for (int x = 0; x < 7; x++)
            {
                if (gameBoard[x, y] == lineType)
                {
                    currentLine.Add(new Vector2(x, y));
                    count++;
                }
                else
                {
                    if (count >= 4 && lineType != TileState.EMPTY)
                    {
                        winingLines.Add(new WinigLine(currentLine.ToArray()));
                    }
                    currentLine.Clear();
                    currentLine.Add(new Vector2(x, y));
                    lineType = gameBoard[x, y];
                    count = 1;
                }
            }
            if (count >= 4 && lineType != TileState.EMPTY)
            {
                winingLines.Add(new WinigLine(currentLine.ToArray()));
            }
            currentLine.Clear();
        }
        //Debug.Log("Horizontal lines checked");
        // Checks diagonal line with positive gradient
        for (int x = 0; x < 7; x++)
        {
            TileState lineType = TileState.EMPTY;
            int count = 0;
            int currentX = x;
            int currentY = 0;
            while (currentX < 7 && currentY < 6)
            {
                if (gameBoard[currentX, currentY] == lineType)
                {
                    currentLine.Add(new Vector2(currentX, currentY));
                    count++;
                }
                else
                {
                    if (count >= 4 && lineType != TileState.EMPTY)
                    {
                        winingLines.Add(new WinigLine(currentLine.ToArray()));
                    }
                    currentLine.Clear();
                    currentLine.Add(new Vector2(currentX, currentY));
                    lineType = gameBoard[currentX, currentY];
                    count = 1;
                }
                currentX++;
                currentY++;
            }
            if (count >= 4 && lineType != TileState.EMPTY)
            {
                winingLines.Add(new WinigLine(currentLine.ToArray()));
            }
            currentLine.Clear();
        }
        for (int y = 0; y < 6; y++)
        {
            TileState lineType = TileState.EMPTY;
            int count = 0;
            int currentX = 0;
            int currentY = y;
            while (currentX < 7 && currentY < 6)
            {
                if (gameBoard[currentX, currentY] == lineType)
                {
                    currentLine.Add(new Vector2(currentX, currentY));
                    count++;
                }
                else
                {
                    if (count >= 4 && lineType != TileState.EMPTY)
                    {
                        winingLines.Add(new WinigLine(currentLine.ToArray()));
                    }
                    currentLine.Clear();
                    currentLine.Add(new Vector2(currentX, currentY));
                    lineType = gameBoard[currentX, currentY];
                    count = 1;
                }
                currentX++;
                currentY++;              
            }
            if (count >= 4 && lineType != TileState.EMPTY)
            {
                winingLines.Add(new WinigLine(currentLine.ToArray()));
            }
            currentLine.Clear();
        }
        //Debug.Log("Diagonal lines with positive gradient checked");
        // Checks diagonal line with negative gradient
        for (int x = 0; x < 7; x++)
        {
            TileState lineType = TileState.EMPTY;
            int count = 0;
            int currentX = x;
            int currentY = 5;
            while (currentX < 7 && currentY >= 0)
            {
                if (gameBoard[currentX, currentY] == lineType)
                {
                    currentLine.Add(new Vector2(currentX, currentY));
                    count++;
                }
                else
                {
                    if (count >= 4 && lineType != TileState.EMPTY)
                    {
                        winingLines.Add(new WinigLine(currentLine.ToArray()));
                    }
                    currentLine.Clear();
                    currentLine.Add(new Vector2(currentX, currentY));
                    lineType = gameBoard[currentX, currentY];
                    count = 1;
                }
                currentX++;
                currentY--;
            }
            if (count >= 4 && lineType != TileState.EMPTY)
            {
                winingLines.Add(new WinigLine(currentLine.ToArray()));
            }
            currentLine.Clear();
        }
        for (int y = 5; y <= 0; y--)
        {
            TileState lineType = TileState.EMPTY;
            int count = 0;
            int currentX = 0;
            int currentY = y;
            while (currentX < 7 && currentY >= 0)
            {
                if (gameBoard[currentX, currentY] == lineType)
                {
                    currentLine.Add(new Vector2(currentX, currentY));
                    count++;
                }
                else
                {
                    if (count >= 4 && lineType != TileState.EMPTY)
                    {
                        winingLines.Add(new WinigLine(currentLine.ToArray()));
                    }
                    currentLine.Clear();
                    currentLine.Add(new Vector2(currentX, currentY));
                    lineType = gameBoard[currentX, currentY];
                    count = 1;
                }
                currentX++;
                currentY--;
            }
            if (count >= 4 && lineType != TileState.EMPTY)
            {
                winingLines.Add(new WinigLine(currentLine.ToArray()));
            }
            currentLine.Clear();
        }

        return winingLines;
    }

    #region Getters and Setters

    /// <summary>
    /// Getter for the game board
    /// </summary>
    /// <returns>gameBoard</returns>
    public TileState[,] GetGameBoard()
    {
        return gameBoard;
    }

    /// <summary>
    /// Getter for the next player to move
    /// </summary>
    /// <returns>nextPlayerToMove</returns>
    public Player GetNextPlayerToMove()
    {
        return nextPlayerToMove;
    }

    /// <summary>
    /// Getter for the game state
    /// </summary>
    /// <returns>gameState</returns>
    public GameState GetGameState()
    {
        return gameState;
    }

    /// <summary>
    /// Getter for the move count
    /// </summary>
    /// <returns>moveCount</returns>
    public int GetMoveCount()
    {
        return moveCount;
    }

    #endregion

    #region Structures

    /// <summary>
    /// Structure used to store a wining line
    /// </summary>
    public struct WinigLine
    {
        /// <summary>
        /// Array storing the coordinates of tiles in the wining line
        /// </summary>
        private Vector2[] coordinates;

        /// <summary>
        /// Constructor for the WinigLine
        /// </summary>
        /// <param name="coordinates">Array storing the coordinates of tiles in the wining line</param>
        public WinigLine(Vector2[] coordinates)
        {
            this.coordinates = coordinates;
        }

        /// <summary>
        /// Getter for the coordinates
        /// </summary>
        /// <returns>coordinates</returns>
        public Vector2[] GetCordinates()
        {
            return coordinates;
        }
    }
    
    #endregion
}