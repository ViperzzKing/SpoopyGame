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

    private void Start()
    {
        cameraControls = Camera.main.GetComponent<CameraControls>();
    }

    private void Update()
    {
        bool pauseCameraKeybind = Input.GetKeyDown(KeyCode.Escape);
        
        
        if (pauseCameraKeybind && camCorder.cameraOut == false)
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            Debug.Log("Readying Pause");
        }

        if (camCorder.pauseReady && Input.GetMouseButtonDown(0))
        {
            paused = !paused; //pause is oppisate
            Time.timeScale = 0; // freeze time
            cameraControls.enabled = !paused; // when paused cam controls is off
            Cursor.lockState = CursorLockMode.None; // bring back cursor
            Debug.Log("Pause");
        }
        else if (camCorder.noCameraKeybind && paused)
        {
            paused = !paused;
            pauseMenu.SetActive(!pauseMenu.activeSelf); // enable pause screen
            Time.timeScale = 1; // resume time
            cameraControls.enabled = !paused;
            Cursor.lockState = CursorLockMode.Locked;
            Debug.Log("Unpause");
        }

        if (camCorder.pauseReady && Input.GetMouseButtonDown(1))
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }
    }

    public void Resume()
    {
        paused = !paused;
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        Time.timeScale = 1;
        cameraControls.enabled = !paused;
        Cursor.lockState = CursorLockMode.Locked;

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
