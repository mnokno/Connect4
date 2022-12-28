using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCircleGenerator : MonoBehaviour
{
    [SerializeField] private GameObject circlesContainer;
    [SerializeField] private GameObject gameCirclePrefab;

    // Start is called before the first frame update
    void Start()
    {
        GenerateCircles();
    }

    private void GenerateCircles()
    {
        int defaultBoxSize = 50;

        for (int x = 0; x < 7; x++)
        {
            for (int y = 0; y < 6; y++)
            {
                float xPos = (-2.5f + x) * defaultBoxSize - (defaultBoxSize / 2f);
                float yPos = (-2f + y) * defaultBoxSize - (defaultBoxSize / 2f);
                GameObject newCircle = Instantiate(gameCirclePrefab);
                newCircle.GetComponent<RectTransform>().SetParent(circlesContainer.transform);
                newCircle.GetComponent<RectTransform>().localPosition = new Vector3(xPos, yPos, 0);
                newCircle.GetComponent<RectTransform>().localScale = Vector3.one;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
