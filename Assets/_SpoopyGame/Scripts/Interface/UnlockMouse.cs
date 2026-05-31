using System;
using UnityEngine;

public class UnlockMouse : MonoBehaviour
{
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
