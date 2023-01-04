using UnityEngine;
using UnityEngine.UIElements;

public class Test : MonoBehaviour
{
    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Button button1 = root.Q<Button>("ShowIP");
        Label label = root.Q<Label>("Label");

        button1.clicked += () => label.text = "127.0.0.1";
    }
}
