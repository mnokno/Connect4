using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCircle : MonoBehaviour
{
    /// <summary>
    /// Reference to yellow color to use
    /// </summary>
    [SerializeField] Color yellowColor;
    /// <summary>
    /// Reference to red color to use
    /// </summary>
    [SerializeField] Color redColor;
    /// <summary>
    /// Reference to the image of this circle, used to update appearance
    /// </summary>
    private Image image;
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
        image.alphaHitTestMinimumThreshold = 0.5f;
    }

    /// <summary>
    /// On clicked event handler
    /// </summary>
    public void Clicked()
    {
        image.color = Random.value > 0.5f ? yellowColor : redColor;
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
