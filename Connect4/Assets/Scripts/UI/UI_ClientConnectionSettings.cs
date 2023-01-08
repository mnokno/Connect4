using UnityEngine;
using UnityEngine.UIElements;
using Unity.Netcode;
using System;
using Unity.Netcode.Transports.UTP;
using C4Audio;

namespace C4UI
{
    public class UI_ClientConnectionSettings : MonoBehaviour
    {
        /// <summary>
        /// Reference to UIManager 
        /// </summary>
        private UIManager uiManager;

        /// <summary>
        /// Start is called before the first frame update
        /// </summary>
        void Start()
        {
            uiManager = FindObjectOfType<UIManager>();

            VisualElement root = GetComponent<UIDocument>().rootVisualElement;

            Button connectBtn = root.Q<Button>("ConnectBtn");
            Button backBtn = root.Q<Button>("BackBtn");
            Button quitBtn = root.Q<Button>("QuitBtn");

            connectBtn.clicked += () => ConnectBtnClicked();
            backBtn.clicked += () => BackBtnCliced();
            quitBtn.clicked += () => QuitBtnCliced();

            // Quit is for non-windows platforms while back is for window platforms
            if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
            {
                quitBtn.style.display = DisplayStyle.None;               
            }
            else
            {
                backBtn.style.display = DisplayStyle.None;
            }
        }

        /// <summary>
        /// Update is called once per frame
        /// </summary>
        private void Update()
        {
            if (NetworkManager.Singleton.IsClient && NetworkManager.Singleton.IsConnectedClient)
            {
                uiManager.gameUI.SetActive(true);
                uiManager.clientConnectPage.rootVisualElement.style.display = DisplayStyle.None;
            }
        }

        /// <summary>
        /// Called when the connect button is clicked
        /// </summary>
        private void ConnectBtnClicked()
        {
            AudioManager.instance.Play("Click");
            try
            {
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
                    GetComponent<UIDocument>().rootVisualElement.Q<TextField>("Input").value, 7777);
                NetworkManager.Singleton.StartClient();
            }
            catch (Exception e)
            {
                NetworkManager.Singleton.Shutdown();
                Debug.LogError(e.Message);
            }
        }

        /// <summary>
        /// Called when the back button is clicked
        /// </summary>
        private void BackBtnCliced()
        {
            AudioManager.instance.Play("Click");
            NetworkManager.Singleton.Shutdown();
            uiManager.clientConnectPage.rootVisualElement.style.display = DisplayStyle.None;
            uiManager.homePage.rootVisualElement.style.display = DisplayStyle.Flex;
        }

        /// <summary>
        /// Called when the quit button is clicked
        /// </summary>
        private void QuitBtnCliced()
        {
            AudioManager.instance.Play("Click");
            Application.Quit();
        }
    }
}
