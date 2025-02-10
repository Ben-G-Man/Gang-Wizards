using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleScreenController : MonoBehaviour
{
    public InputAction anyKeyPressed;
    public string startSceneName;

    void OnEnable()
    {
        anyKeyPressed.Enable();
        anyKeyPressed.performed += StartGame;
    }
    
    void OnDisable()
    {
        anyKeyPressed.Disable();
        anyKeyPressed.performed -= StartGame;
    }

    void StartGame(InputAction.CallbackContext context)
    {
        GameEventManager.LoadScene(startSceneName);
    }
}
