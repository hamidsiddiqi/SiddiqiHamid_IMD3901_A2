using UnityEngine;
using UnityEngine.InputSystem;

public class SceneSwitcher : MonoBehaviour
{
    [Header("Player Controllers")]
    public GameObject desktopPlayer;
    public GameObject hmdPlayer;

    [Header("Start Menu Graphic")]
    public GameObject startMenu;

    [Header("You Win Graphic")]
    public GameObject winGraphic;

    private bool gameStarted = false;

    void Start()
    {
        desktopPlayer.SetActive(false);
        hmdPlayer.SetActive(false);

        if (startMenu != null) { 
        
        startMenu.SetActive(true);
        }

        if (winGraphic != null)
        {

            winGraphic.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        // For Desktop
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            EnableDesktop();
        }

        // For HMD
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            EnableHMD();
        }
    }

    void EnableDesktop()
    {
        desktopPlayer.SetActive(true);
        hmdPlayer.SetActive(false);

        StartGame();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void EnableHMD()
    {
        hmdPlayer.SetActive(true);
        desktopPlayer.SetActive(false);

        StartGame();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void StartGame()
    {
        if (!gameStarted && startMenu != null)
        {
            startMenu.SetActive(false);
            gameStarted = true;
        }
    }
}