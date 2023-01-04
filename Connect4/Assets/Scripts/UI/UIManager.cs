using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace C4UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] public GameObject gameUI;
        [SerializeField] public UIDocument homePage;
        [SerializeField] public UIDocument clientConnectPage;
        [SerializeField] public UIDocument serverWaitingPage;
        [SerializeField] public UIDocument gameOverPage;

        /// <summary>
        /// Start is called before the first frame update
        /// </summary>
        private void Start()
        {
            homePage.rootVisualElement.style.display = DisplayStyle.Flex;
            clientConnectPage.rootVisualElement.style.display = DisplayStyle.None;
            serverWaitingPage.rootVisualElement.style.display = DisplayStyle.None;
            gameOverPage.rootVisualElement.style.display = DisplayStyle.None;
        }
    }
}
