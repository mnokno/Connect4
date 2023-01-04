using UnityEngine;
using UnityEngine.UIElements;

public class UI_Main : MonoBehaviour
{
    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Button button1 = root.Q<Button>("ClientBtn");

        button1.clicked += () => Debug.Log("ClientBtn clicked");
    }
}
