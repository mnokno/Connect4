using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;
using C4Audio;

namespace C4UI
{
    public class UI_DifficultySelection : MonoBehaviour
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

            Button nextBtn = root.Q<Button>("NextBtn");
            Button backBtn = root.Q<Button>("BackBtn");
            Button quitBtn = root.Q<Button>("QuitBtn");
            SliderInt slider = root.Q<SliderInt>("Slider");

            nextBtn.clicked += () => NextBtnCliced();
            backBtn.clicked += () => BackBtnCliced();
            quitBtn.clicked += () => QuitBtnCliced();

            slider.RegisterValueChangedCallback(OnValueChanged);

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
        /// Called when the next button is clicked
        /// </summary>
        private void NextBtnCliced()
        {
            AudioManager.instance.Play("Click");
            uiManager.difficultySelectionPage.rootVisualElement.style.display = DisplayStyle.None;
            uiManager.clientConnectPage.rootVisualElement.style.display = DisplayStyle.Flex;
        }

        /// <summary>
        /// Called when the back button is clicked
        /// </summary>
        private void BackBtnCliced()
        {
            AudioManager.instance.Play("Click");
            NetworkManager.Singleton.Shutdown();
            uiManager.difficultySelectionPage.rootVisualElement.style.display = DisplayStyle.None;
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

        /// <summary>
        /// Called when slider value is changed
        /// </summary>
        /// <param name="value">New slider value</param>
        private void OnValueChanged(ChangeEvent<int> value)
        {
            GetComponent<UIDocument>().rootVisualElement.Q<Label>("DiffLbl").text = "AI Level: " + value.newValue;
        }
    }
}
