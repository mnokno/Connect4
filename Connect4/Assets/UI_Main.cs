using UnityEngine;
using UnityEngine.UIElements;

public class UI_Main : MonoBehaviour
{
    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Button button = root.Q<Button>("ClientBtn");
        
        button.clicked += () => Debug.Log("ClientBtn clicked");
    }
}
