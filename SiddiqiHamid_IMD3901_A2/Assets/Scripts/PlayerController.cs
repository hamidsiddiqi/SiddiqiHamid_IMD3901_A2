using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float mouseSensitivity = 2f;

    public CharacterController controller;
    public Transform cameraTransform;

    [Header("Audio Settings")]
    public AudioSource shootSound;
    public AudioSource emptySound;
    public AudioSource explosionSound;
    public AudioSource music;

    [Header("Bullet Count Settings")]
    public int bulletCount = 0;
    public TextMeshProUGUI bulletCounterText;
    public TextMeshProUGUI ufoCounterText;
    public GameObject bulletPrefab;
    public float range = 100f;

    [Header("You Win")]
    public GameObject winGraphic;
    public AudioSource winSound;
    private bool hasWon = false;

    public float explosionDelay = 0.5f;

    private int ufosRemaining;

    float xRotation = 0f;

    void Start()
    {
        ufosRemaining = GameObject.FindGameObjectsWithTag("UFO").Length;

        if (shootSound != null) { 
            shootSound.playOnAwake = false;
        }

        if (emptySound != null) { 
            emptySound.playOnAwake = false; 
        }

        if (explosionSound != null) { 
            explosionSound.playOnAwake = false; 
        }

        if (music != null)
        {
            music.loop = true;
            music.playOnAwake = true;
            music.Play();
        }

        if (winGraphic != null)
        {
            winGraphic.SetActive(false);
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
            if (bulletCount > 0)
            {
                Shoot();
            }
            else
            {
                if (emptySound != null) emptySound.Play();
            }
        }
    }

    void Shoot()
    {
        if (shootSound != null) shootSound.Play();
        bulletCount--;
        UpdateUI();

        if (bulletPrefab != null)
        {
            for (int i = 0; i < 2; i++)
            {
                float spawnDist = Random.Range(20f, 40f);
                Vector2 randomDir = Random.insideUnitCircle.normalized * spawnDist;

                Vector3 spawnPosition = new Vector3(
                    transform.position.x + randomDir.x,
                    0.5f, 
                    transform.position.z + randomDir.y
                );

                Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
            }
        }

        Ray ray = cameraTransform.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            if (hit.collider.CompareTag("UFO"))
            {
                StartCoroutine(DelayedExplosion(hit.collider.gameObject));
            }
        }
    }

    IEnumerator DelayedExplosion(GameObject target)
    {
        
        yield return new WaitForSeconds(explosionDelay);

 
        if (explosionSound != null) explosionSound.Play();

    
        target.SetActive(false);
        ufosRemaining--;

        UpdateUI();
    }

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

        if (ufoCounterText != null)
        {
            ufoCounterText.text = "UFOs Remaining: " + ufosRemaining;
        }

        if (ufosRemaining <= 0 && !hasWon && Time.timeSinceLevelLoad > 0.5f)
        {
            hasWon = true;
            if (winGraphic != null)
            {

                winGraphic.SetActive(true);

            }
            if (winSound != null)
            {

                winSound.Play();

            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

    }
}