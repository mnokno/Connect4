using UnityEngine;
using UnityEngine.UIElements;
using Unity.Netcode;
using C4Audio;

namespace C4UI
{
    public class UI_GameOver : MonoBehaviour
    {
        /// <summary>
        /// Reference to UIManager 
        /// </summary>
        private UIManager uiManager;

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
            AudioManager.instance.Play("Click");
            FindObjectOfType<GameManager>().ResetGame();
            FindObjectOfType<GameManager>().NewGame();
            uiManager.gameUIEventSystem.SetActive(true);
            uiManager.gameOverPage.rootVisualElement.style.display = DisplayStyle.None;
        }

        /// <summary>
        /// Called when the home button is clicked
        /// </summary>
        private void HomeBtnClicked()
        {
            AudioManager.instance.Play("Click");
            FindObjectOfType<NetworkGate>().EndSesionServerRpc();
            FindObjectOfType<GameManager>().ResetGame();
            ShowAllButtons();
            uiManager.gameUIEventSystem.SetActive(true);
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
            HideButtonsIfServer();
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

        /// <summary>
        /// Hides new game and go home buttons if its the server
        /// </summary>
        private void HideButtonsIfServer()
        {
            if (NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsHost)
            {
                uiManager.gameOverPage.rootVisualElement.Q<Button>("PlayAgainBtn").style.display = DisplayStyle.None;
                uiManager.gameOverPage.rootVisualElement.Q<Button>("HomeBtn").style.display = DisplayStyle.None;
            }
        }

        /// <summary>
        /// Shows new game and go home buttons in case they where hidden by HideButtonsIfServer
        /// </summary>
        private void ShowAllButtons()
        {
            uiManager.gameOverPage.rootVisualElement.Q<Button>("PlayAgainBtn").style.display = DisplayStyle.Flex;
            uiManager.gameOverPage.rootVisualElement.Q<Button>("HomeBtn").style.display = DisplayStyle.Flex;
        }
    }
}
