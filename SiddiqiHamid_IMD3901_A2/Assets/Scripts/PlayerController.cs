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

    [Header("Bullet Count Settings")]
    public int bulletCount = 0;
    public TextMeshProUGUI bulletCounterText;
    public GameObject bulletPrefab;
    public float range = 100f; // How far you can shoot

    public float explosionDelay = 0.2f;

    float xRotation = 0f;

    void Start()
    {
        if (shootSound != null) shootSound.playOnAwake = false;
        if (emptySound != null) emptySound.playOnAwake = false;
        if (explosionSound != null) explosionSound.playOnAwake = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        UpdateUI();
    }

    void Update()
    {
        // --- Movement & Look Logic ---
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

        // --- Shooting Interaction ---
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

    //void Shoot()
    //{
    //    if (shootSound != null) shootSound.Play();
    //    bulletCount--;
    //    UpdateUI();

    //    // --- Raycast Logic for Hitting UFO ---
    //    // Creates a ray from the center of the screen
    //    Ray ray = cameraTransform.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
    //    RaycastHit hit;

    //    if (Physics.Raycast(ray, out hit, range))
    //    {
    //        // Check if we hit a UFO
    //        if (hit.collider.CompareTag("UFO"))
    //        {
    //            // Play explosion feedback
    //            if (explosionSound != null) explosionSound.Play();

    //            // Requirement: Destroy/Hide object during runtime
    //            hit.collider.gameObject.SetActive(false);
    //        }
    //    }
    //}

    //void Shoot()
    //{
    //    // 1. Immediate Shooting Feedback
    //    if (shootSound != null) shootSound.Play();
    //    bulletCount--;
    //    UpdateUI();

    //    // 2. Check for Hit
    //    Ray ray = cameraTransform.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
    //    RaycastHit hit;

    //    if (Physics.Raycast(ray, out hit, range))
    //    {
    //        if (hit.collider.CompareTag("UFO"))
    //        {
    //            // Start the delay before the explosion happens
    //            StartCoroutine(DelayedExplosion(hit.collider.gameObject));
    //        }
    //    }
    //}

    void Shoot()
    {
        // 1. Immediate Shooting Feedback
        if (shootSound != null) shootSound.Play();
        bulletCount--;
        UpdateUI();

        // --- SPAWN TWO REPLACEMENT BULLETS FURTHER AWAY ---
        if (bulletPrefab != null)
        {
            for (int i = 0; i < 2; i++) // This loop runs twice to spawn 2 bullets
            {
                // Calculate a position between 10 and 20 units away
                float spawnDist = Random.Range(10f, 20f);
                Vector2 randomDir = Random.insideUnitCircle.normalized * spawnDist;

                Vector3 spawnPosition = new Vector3(
                    transform.position.x + randomDir.x,
                    0.5f, // Keeps bullets at floor level
                    transform.position.z + randomDir.y
                );

                Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
            }
        }

        // 2. Raycast Logic for Hitting UFO (Remains the same)
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

    // This handles the timing gap between the shot and the impact
    IEnumerator DelayedExplosion(GameObject target)
    {
        // Wait for the specified time (simulating bullet travel)
        yield return new WaitForSeconds(explosionDelay);

        // 3. Delayed Explosion Feedback
        if (explosionSound != null) explosionSound.Play();

        // Requirement: Hide/Destroy object during runtime
        target.SetActive(false);
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
    }
}