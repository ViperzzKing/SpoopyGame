using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public AudioMixer masterMixer;
    [SerializeField] private CameraControls cameraControls;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private CamCorderPositions camCorder;

    [SerializeField] private GameObject pauseButtons;
    [SerializeField] private GameObject optionButtons;
    
    public bool paused;

    private void Awake()
    {
        cameraControls = Camera.main.GetComponent<CameraControls>();
        
        if (cameraControls == null) Debug.LogWarning("Missing Camera Controls");
        if (pauseMenu == null) Debug.LogWarning("Missing Pause Menu");
        if (pauseButtons == null) Debug.LogWarning("Missing Pause Buttons");
        if (optionButtons == null) Debug.LogWarning("Missing Options Buttons");
        if (masterMixer == null) Debug.LogWarning("Missing Audio Mixer");
    }

    private void Update()
    {
        bool pauseCameraKeybind = Input.GetKeyDown(KeyCode.Escape);
        
        
        if (pauseCameraKeybind)
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            Debug.Log("Readying Pause");
        }
        else if (camCorder.PauseReady && Input.GetMouseButtonDown(1) || camCorder.PauseReady && Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }

        if (camCorder.PauseReady && Input.GetMouseButtonDown(0))
        {
            SetPaused(true);
        }
        else if (camCorder.NoCameraKeybind && paused)
        {
            SetPaused(false);
        }

    }

    private void SetPaused(bool isPaused)
    {
        paused = isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        cameraControls.enabled = !isPaused;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        pauseMenu.SetActive(isPaused);
    }
    
    public void Resume()
    {
        SetPaused(false);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0); // loads main menu
    }

    public void Options()
    {
        // changes buttons
        optionButtons.SetActive(true);
        pauseButtons.SetActive(false);
    }

    public void Return()
    {
        //brings pause buttons back
        optionButtons.SetActive(false);
        pauseButtons.SetActive(true);
    }
    
    public void SetMasterVolume(float sliderValue)
    {
        //changes volume
        float db = Mathf.Log10(Mathf.Max(sliderValue, 0.0001f)) * 20f;
        masterMixer.SetFloat("MasterVolume", db);
    }
    
    public void FullScreenToggle()
    {
        //switches to oppisate
        Screen.fullScreen = !Screen.fullScreen;
    }
}
