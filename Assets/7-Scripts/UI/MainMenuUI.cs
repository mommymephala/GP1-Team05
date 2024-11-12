using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class MainMenuUI : MonoBehaviour
{
    private Button playButton;
    private Button settingsButton;
    private Button exitButton;

    private VisualElement settingsParent;

    void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        playButton = root.Q<Button>("Play");
        settingsButton = root.Q<Button>("Settings");
        exitButton = root.Q<Button>("Exit");
        settingsParent = root.Q<VisualElement>("SettingsParent");

        playButton.RegisterCallback<ClickEvent>(OnPlayButtonClicked);
        settingsButton.RegisterCallback<ClickEvent>(OnSettingsButtonClicked);
        exitButton.RegisterCallback<ClickEvent>(OnExitButtonClicked);

        settingsParent.style.display = DisplayStyle.None;

    }

    private void OnPlayButtonClicked(ClickEvent clickEvent)
    {
        SceneManager.LoadScene("BetaShowcase");
    }
    private void OnSettingsButtonClicked(ClickEvent clickEvent)
    {
        settingsParent.style.display = DisplayStyle.Flex;
    }
    private void OnExitButtonClicked(ClickEvent clickEvent)
    {
        settingsParent.style.display = DisplayStyle.None;
    }
}
