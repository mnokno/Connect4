using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCircleGenerator : MonoBehaviour
{
    /// <summary>
    /// Reference to gameCricles container
    /// </summary>
    [SerializeField] private GameObject circlesContainer;
    /// <summary>
    /// Reference to game circle prefab
    /// </summary>
    [SerializeField] private GameObject gameCirclePrefab;
    /// <summary>
    /// Reference to game object contain start legacy UI image component
    /// </summary>
    [SerializeField] private GameObject starImage;

    // Start is called before the first frame update
    void Start()
    {
        GenerateCircles();
    }

    /// <summary>
    /// Generates game circles to correct position
    /// </summary>
    private void GenerateCircles()
    {
        int defaultBoxSize = 50;
        GameCircle.starImage = starImage;

        for (int x = 0; x < 7; x++)
        {
            for (int y = 0; y < 6; y++)
            {
                float xPos = (-2.5f + x) * defaultBoxSize - (defaultBoxSize / 2f);
                float yPos = (-2f + y) * defaultBoxSize - (defaultBoxSize / 2f);
                GameObject newCircle = Instantiate(gameCirclePrefab);
                newCircle.GetComponentInParent<RectTransform>().SetParent(circlesContainer.transform);
                newCircle.GetComponentInParent<RectTransform>().localPosition = new Vector3(xPos, yPos, 0);
                newCircle.GetComponentInParent<RectTransform>().localScale = Vector3.one;
                newCircle.GetComponentInParent<GameCircle>().SetGamePos(x, y);
                newCircle.name = "GameCircle" + x.ToString() + ":" + y.ToString();
            }
        }
    }
}
