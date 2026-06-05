using System;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 1;
    }
}
