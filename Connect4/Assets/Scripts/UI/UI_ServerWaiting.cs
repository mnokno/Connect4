using UnityEngine;
using UnityEngine.UIElements;
using Unity.Netcode;
using System.Net;
using System;
using C4Audio;

namespace C4UI
{
    public class UI_ServerWaiting : MonoBehaviour
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

            Button toggleIPBtn = root.Q<Button>("ToggleIPBtn");
            Button backBtn = root.Q<Button>("BackBtn");


            toggleIPBtn.clicked += () => ToggleIPCliced();
            backBtn.clicked += () => BackBtnCliced();
        }

        /// <summary>
        /// Update is called once per frame
        /// </summary>
        private void Update()
        {
            if (NetworkManager.Singleton.IsServer && NetworkManager.Singleton.ConnectedClients.Count == 1)
            {
                uiManager.gameUI.SetActive(true);
                uiManager.serverWaitingPage.rootVisualElement.style.display = DisplayStyle.None;
            }
        }

        /// <summary>
        /// Called when the toggle IP button is clicked
        /// </summary>
        private void ToggleIPCliced()
        {
            AudioManager.instance.Play("Click");
            if (isIPShown)
            {
                HideIP();
            }
            else
            {
                ShowIP();
            }
            GetComponent<UIDocument>().rootVisualElement.Q<Button>("ToggleIPBtn").text = isIPShown ? "Show IP" : "Hide IP";
            isIPShown = !isIPShown;
        }

        /// <summary>
        /// Shows IP on the UI
        /// </summary>
        private void ShowIP()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            string ipV4 = "IPv4 not detected.";
            string ipV6 = "IPv6 not detected.";

            foreach (IPAddress iPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (iPAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    ipV4 = iPAddress.ToString();
                }
                else if (iPAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                {
                    ipV6 = iPAddress.ToString();
                }
            }

            root.Q<Label>("IPv4Lbl").text = ipV4;
            root.Q<Label>("IPv6Lbl").text = ipV6;
        }

        /// <summary>
        /// Hides IP on the UI
        /// </summary>
        private void HideIP()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            root.Q<Label>("IPv4Lbl").text = "***************";
            root.Q<Label>("IPv6Lbl").text = "************************************************";
        }

        /// <summary>
        /// Called when the back button is clicked
        /// </summary>
        private void BackBtnCliced()
        {
            AudioManager.instance.Play("Click");
            NetworkManager.Singleton.Shutdown();
            uiManager.serverWaitingPage.rootVisualElement.style.display = DisplayStyle.None;
            uiManager.homePage.rootVisualElement.style.display = DisplayStyle.Flex;
        }
    }
}
