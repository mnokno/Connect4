using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCircle : MonoBehaviour
{
    [SerializeField] Color yellowColor;
    [SerializeField] Color redColor;
    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponentInParent<Image>();
        image.alphaHitTestMinimumThreshold = 0.5f;
    }

    public void Clicked()
    {
        image.color = Random.value > 0.5f ? yellowColor : redColor;
    }
}
