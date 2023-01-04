using UnityEngine;
using UnityEngine.UIElements;

public class Test2 : MonoBehaviour
{
    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Label victoryLabel = root.Q<Label>("Victory");
        Label defeateLabel = root.Q<Label>("Defeat");
        Label defeateDraw = root.Q<Label>("Draw");
        victoryLabel.style.display = DisplayStyle.None;
        defeateLabel.style.display = DisplayStyle.None;
        defeateDraw.style.display = DisplayStyle.None;
        victoryLabel.style.display = DisplayStyle.Flex;
    }
}
