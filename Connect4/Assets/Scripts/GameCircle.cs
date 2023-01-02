using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCircle : MonoBehaviour
{
    /// <summary>
    /// Reference to yellow color to use
    /// </summary>
    [SerializeField] private Color yellowColor;
    /// <summary>
    /// Reference to red color to use
    /// </summary>
    [SerializeField] private Color redColor;
    /// <summary>
    /// Reference to the image of this circle, used to update appearance
    /// </summary>
    private Image image;
    /// <summary>
    /// Reference to the GameManager
    /// </summary>
    private GameManager gameManager;
    /// <summary>
    /// Reference to the InputManager
    /// </summary>
    private InputManager inputManager;
    /// <summary>
    /// X position of this circle on the game board
    /// </summary>
    private int x;
    /// <summary>
    /// Y position of this circle on the game board
    /// </summary>
    private int y;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponentInParent<Image>();
        gameManager = FindObjectOfType<GameManager>();
        inputManager = FindObjectOfType<InputManager>();
        gameManager.RegisterGameCircle(this);
        image.alphaHitTestMinimumThreshold = 0.5f;
    }

    /// <summary>
    /// On clicked event handler
    /// </summary>
    public void Clicked()
    {
        inputManager.HandleClicked(this);
    }

    /// <summary>
    /// Changes color of this circle
    /// </summary>
    /// <param name="color">Color to change it to</param>
    public void ChangeColor(CircleColor circle)
    {
        image.color = circle == CircleColor.YELLOW ? yellowColor : redColor;
    }

    #region Getters and Setters
    
    /// <summary>
    /// Setter for x and y
    /// </summary>
    /// <param name="x">new x vale</param>
    /// <param name="y">new y bale</param>
    public void SetGamePos(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    /// <summary>
    /// Getter for x
    /// </summary>
    /// <returns>x</returns>
    public int GetX()
    {
        return x;
    }

    /// <summary>
    /// Getter for y
    /// </summary>
    /// <returns>y</returns>
    public int GetY()
    {
        return y;
    }

    #endregion
}
