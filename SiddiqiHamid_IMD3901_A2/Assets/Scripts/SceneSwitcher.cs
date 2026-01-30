using UnityEngine;
using UnityEngine.InputSystem;

public class SceneSwitcher : MonoBehaviour
{
    [Header("Player Controllers")]
    public GameObject desktopPlayer;
    public GameObject hmdPlayer;

    [Header("Start Menu Image")]
    // Drag the parent object of your Start Screen here
    public GameObject startMenu;

    [Header("You Win Graphic")]
    public GameObject winGraphic;

    private bool gameStarted = false;

    void Start()
    {
        // Keep both players disabled until a choice is made
        desktopPlayer.SetActive(false);
        hmdPlayer.SetActive(false);

        // Ensure the menu is visible at the very start
        if (startMenu != null) { 
        
        startMenu.SetActive(true);
        }

        if (winGraphic != null)
        {

            winGraphic.SetActive(false);
        }

        // Unlock cursor so they can interact with the menu if needed
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        // Press 1 for Desktop
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            EnableDesktop();
        }

        // Press 2 for HMD
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            EnableHMD();
        }
    }

    void EnableDesktop()
    {
        desktopPlayer.SetActive(true);
        hmdPlayer.SetActive(false);

        //if (winGraphic != null) { 
        //    winGraphic.SetActive(false);
        //} 

        StartGame();

        // Standard Desktop locking
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void EnableHMD()
    {
        hmdPlayer.SetActive(true);
        desktopPlayer.SetActive(false);

        StartGame();

        // VR usually needs a visible/free cursor for simulators
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void StartGame()
    {
        // Hide the start menu only once
        if (!gameStarted && startMenu != null)
        {
            startMenu.SetActive(false);
            gameStarted = true;
        }
    }
}