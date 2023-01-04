using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [SerializeField] public GameObject gameUI;
    [SerializeField] public UIDocument homePage;
    [SerializeField] public UIDocument clientConnectPage;
    [SerializeField] public UIDocument serverWaitingPage;
    [SerializeField] public UIDocument gameOverPage;
}
