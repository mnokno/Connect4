using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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

        clientBtn.clicked += () => ClientBtnCliced();
        hostBtn.clicked += () => HostBtnCliced();
        serverBtn.clicked += () => ServerBtnCliced();
    }

    /// <summary>
    /// Called when the client button is clicked
    /// </summary>
    private void ClientBtnCliced()
    {
        uiManager.clientConnectPage.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// Called when the host button is clicked
    /// </summary>
    private void HostBtnCliced()
    {
        uiManager.gameUI.SetActive(true);
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// Called when the server button is clicked
    /// </summary>
    private void ServerBtnCliced()
    {
        uiManager.serverWaitingPage.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
