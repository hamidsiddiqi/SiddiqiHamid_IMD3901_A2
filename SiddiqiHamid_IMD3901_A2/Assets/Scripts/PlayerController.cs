//using UnityEngine;
//using UnityEngine.InputSystem;
//using TMPro; 

//public class PlayerController : MonoBehaviour
//{
//    public float speed = 5f;
//    public float mouseSensitivity = 2f;

//    public CharacterController controller;
//    public Transform cameraTransform;

//    [Header("Audio Settings")]
//    public AudioSource shootSound; 

//    float xRotation = 0f;

//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {

//        if (shootSound != null)
//        {
//            shootSound.playOnAwake = false;
//        }

//        //Debug.Log("Scene has started!");

//        Cursor.lockState = CursorLockMode.Locked;
//        Cursor.visible = false;

//    }

//    // Update is called once per frame
//    void Update()
//    {
//        //Debug.Log("Scene is updating!");

//        Vector2 moveInput = Keyboard.current != null ? new Vector2
//            (
//                (Keyboard.current.aKey.isPressed ? -1 : 0) + (Keyboard.current.dKey.isPressed ? 1 : 0),
//                (Keyboard.current.sKey.isPressed ? -1 : 0) + (Keyboard.current.wKey.isPressed ? 1 : 0)
//            ) : Vector2.zero;

//        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
//        controller.Move(move * speed * Time.deltaTime);

//        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

//        float mouseX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
//        float mouseY = mouseDelta.y * mouseSensitivity * Time.deltaTime;

//        xRotation -= mouseY;
//        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

//        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
//        transform.Rotate(Vector3.up * mouseX);


//        if (Keyboard.current.spaceKey.wasPressedThisFrame)
//        {
//            if (shootSound != null)
//            {
//                shootSound.Play();
//            }
//        }

//    }
//}

using UnityEngine;
using UnityEngine.InputSystem;
using TMPro; // Needed for the UI counter

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float mouseSensitivity = 2f;

    public CharacterController controller;
    public Transform cameraTransform;

    [Header("Audio Settings")]
    public AudioSource shootSound;

    [Header("Bullet Count Settings")]
    public int bulletCount = 0;
    public TextMeshProUGUI bulletCounterText;
    public GameObject bulletPrefab;

    float xRotation = 0f;

    void Start()
    {
        if (shootSound != null)
        {
            shootSound.playOnAwake = false;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        UpdateUI();
    }

    void Update()
    {
        Vector2 moveInput = Keyboard.current != null ? new Vector2
            (
                (Keyboard.current.aKey.isPressed ? -1 : 0) + (Keyboard.current.dKey.isPressed ? 1 : 0),
                (Keyboard.current.sKey.isPressed ? -1 : 0) + (Keyboard.current.wKey.isPressed ? 1 : 0)
            ) : Vector2.zero;

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * speed * Time.deltaTime);

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        float mouseX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseDelta.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            // Only play shoot sound if we have ammo
            if (shootSound != null && bulletCount > 0)
            {
                shootSound.Play();
                bulletCount--; // Decrease count when shooting
                UpdateUI();
            }
        }
    }

    // This function will be called by the pickup script
    public void AddBullet()
    {
        bulletCount++;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (bulletCounterText != null)
        {
            bulletCounterText.text = "Bullet Count: " + bulletCount;
        }
    }
}