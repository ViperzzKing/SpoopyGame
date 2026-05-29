using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private GameObject options;
    [SerializeField] private AudioMixer masterMixer;
    
    //sets master volume
    public void SetMasterVolume(float sliderValue)
    {
        // makes it so its 0-1
        float db = Mathf.Log10(Mathf.Max(sliderValue, 0.0001f)) * 20f;
        masterMixer.SetFloat("MasterVolume", db);
    }

    public void Play()
    {
        SceneManager.LoadScene(1); // Loads Game
    }
    
    public void Options()
    {
        options.SetActive(!options.activeSelf); // toggles Options
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
