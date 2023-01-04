using UnityEngine;
using UnityEngine.UIElements;
using Unity.Netcode;

namespace C4UI
{
    public class UI_GameOver : MonoBehaviour
    {
        /// <summary>
        /// Reference to UIManager 
        /// </summary>
        private UIManager uiManager;
        /// <summary>
        /// Set to true if IP is displayed
        /// </summary>
        private bool isIPShown = false;

        /// <summary>
        /// Start is called before the first frame update
        /// </summary>
        private void Start()
        {
            uiManager = FindObjectOfType<UIManager>();

            VisualElement root = GetComponent<UIDocument>().rootVisualElement;

            Button playAgainBtn = root.Q<Button>("PlayAgainBtn");
            Button homeBtn = root.Q<Button>("HomeBtn");

            uiManager.gameOverPage.rootVisualElement.Q<Label>("Draw").style.display = DisplayStyle.None;
            uiManager.gameOverPage.rootVisualElement.Q<Label>("Victory").style.display = DisplayStyle.None;
            uiManager.gameOverPage.rootVisualElement.Q<Label>("Defeat").style.display = DisplayStyle.None;

            playAgainBtn.clicked += () => PlayAgainBtnClicked();
            homeBtn.clicked += () => HomeBtnClicked();
        }

        /// <summary>
        /// Called when the play again IP button is clicked
        /// </summary>
        private void PlayAgainBtnClicked()
        {
            
        }

        /// <summary>
        /// Called when the home button is clicked
        /// </summary>
        private void HomeBtnClicked()
        {
            //TODO reset game UI
            uiManager.gameUI.SetActive(false);
            NetworkManager.Singleton.Shutdown();
            uiManager.gameOverPage.rootVisualElement.style.display = DisplayStyle.None;
            uiManager.homePage.rootVisualElement.style.display = DisplayStyle.Flex;
        }

        /// <summary>
        /// Displays game result on screen
        /// </summary>
        /// <param name="gameResult">Game result to show</param>
        public void ShowGameResult(GameResult gameResult)
        {
            switch (gameResult)
            {
                case GameResult.DRAW:
                    uiManager.gameOverPage.rootVisualElement.Q<Label>("Draw").style.display = DisplayStyle.Flex;
                    uiManager.gameOverPage.rootVisualElement.Q<Label>("Victory").style.display = DisplayStyle.None;
                    uiManager.gameOverPage.rootVisualElement.Q<Label>("Defeat").style.display = DisplayStyle.None;
                    break;
                case GameResult.WIN:
                    uiManager.gameOverPage.rootVisualElement.Q<Label>("Draw").style.display = DisplayStyle.None;
                    uiManager.gameOverPage.rootVisualElement.Q<Label>("Victory").style.display = DisplayStyle.Flex;
                    uiManager.gameOverPage.rootVisualElement.Q<Label>("Defeat").style.display = DisplayStyle.None;
                    break;
                case GameResult.LOSS:
                    uiManager.gameOverPage.rootVisualElement.Q<Label>("Draw").style.display = DisplayStyle.None;
                    uiManager.gameOverPage.rootVisualElement.Q<Label>("Victory").style.display = DisplayStyle.None;
                    uiManager.gameOverPage.rootVisualElement.Q<Label>("Defeat").style.display = DisplayStyle.Flex;
                    break;
            }
        }
    }
}
