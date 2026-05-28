using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private GameObject options;
    [SerializeField] private AudioMixer masterMixer;
    
    public void SetMasterVolume(float sliderValue)
    {
        float db = Mathf.Log10(Mathf.Max(sliderValue, 0.0001f)) * 20f;
        masterMixer.SetFloat("MasterVolume", db);
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    
    public void Options()
    {
        options.SetActive(!options.activeSelf);
        // Master Volume Toggle
    }

    public void FullScreenToggle()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
    
    public void Quit()
    {
        Application.Quit();
        Debug.Log("quit");
    }
}
