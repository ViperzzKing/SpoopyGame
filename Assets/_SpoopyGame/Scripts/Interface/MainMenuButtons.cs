using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private AudioMixer masterMixer;

    [SerializeField] private int gameScene;

    private void Awake()
    {
        if (optionsPanel == null) Debug.LogWarning("Missing Options Panel");
        if (masterMixer == null) Debug.LogWarning("Missing Master Mixer");
    }

    //sets master volume
    public void SetMasterVolume(float sliderValue)
    {
        // makes it so its 0-1
        float db = Mathf.Log10(Mathf.Max(sliderValue, 0.0001f)) * 20f;
        masterMixer.SetFloat("MasterVolume", db);
    }

    public void Play()
    {
        SceneManager.LoadScene(gameScene); // Loads Game
    }
    
    public void Options()
    {
        optionsPanel.SetActive(!optionsPanel.activeSelf); // toggles Options
    }

    public void FullScreenToggle()
    {
        Screen.fullScreen = !Screen.fullScreen; // Toggles fullscreen to opisate
    }
    
    public void Quit()
    {
        //quits game
        Application.Quit();
        Debug.Log("quit");
    }
}
