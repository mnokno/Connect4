using UnityEngine;
using UnityEngine.UIElements;
using Unity.Netcode;

public class UI_Home : MonoBehaviour
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

        Button clientBtn = root.Q<Button>("ClientBtn");
        Button hostBtn = root.Q<Button>("HostBtn");
        Button serverBtn = root.Q<Button>("ServerBtn");
        Button quitBtn = root.Q<Button>("QuitBtn");

        clientBtn.clicked += () => ClientBtnCliced();
        hostBtn.clicked += () => HostBtnCliced();
        serverBtn.clicked += () => ServerBtnCliced();
        quitBtn.clicked += () => QuitBtnCliced();
    }

    /// <summary>
    /// Called when the client button is clicked
    /// </summary>
    private void ClientBtnCliced()
    {
        uiManager.homePage.rootVisualElement.style.display = DisplayStyle.None;
        uiManager.clientConnectPage.rootVisualElement.style.display = DisplayStyle.Flex;
    }

    /// <summary>
    /// Called when the host button is clicked
    /// </summary>
    private void HostBtnCliced()
    {
        uiManager.homePage.rootVisualElement.style.display = DisplayStyle.None;
        this.gameObject.SetActive(false);
        NetworkManager.Singleton.StartHost();
    }

    /// <summary>
    /// Called when the server button is clicked
    /// </summary>
    private void ServerBtnCliced()
    {
        uiManager.homePage.rootVisualElement.style.display = DisplayStyle.None;
        uiManager.serverWaitingPage.rootVisualElement.style.display = DisplayStyle.Flex;
        NetworkManager.Singleton.StartServer();
    }

    /// <summary>
    /// Called when the quit button is clicked
    /// </summary>
    private void QuitBtnCliced()
    {
        Application.Quit();
    }
}