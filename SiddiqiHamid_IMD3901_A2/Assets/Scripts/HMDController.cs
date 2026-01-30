using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class HMDController : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource shootSound;
    public AudioSource emptySound;
    public AudioSource explosionSound;
    public AudioSource music;

    [Header("UI & Bullet Settings")]
    public int bulletCount = 0;
    public TextMeshProUGUI bulletCounterText;
    public TextMeshProUGUI ufoCounterText;
    public GameObject bulletPrefab;
    public float range = 500f; // Matching your Inspector value

    [Header("Bullet Follow (Manipulation)")]
    public GameObject floatingBullet;
    public Vector3 followOffset = new Vector3(0.5f, -0.4f, 1f);
    public Transform cameraTransform;

    [Header("You Win")]
    public GameObject winGraphic;
    public AudioSource winSound;
    private bool hasWon = false;

    public float explosionDelay = 0.5f; // Matching your Inspector value
    private int ufosRemaining;

    void Start()
    {
        // Finds all objects tagged "UFO" at the start
        ufosRemaining = GameObject.FindGameObjectsWithTag("UFO").Length;

        if (music != null) { music.loop = true; music.Play(); }

        if (winGraphic != null)
        {
            winGraphic.SetActive(false);
        }

        UpdateUI();
    }

    void Update()
    {
        // MANIPULATION: Visual Follow Logic
        if (bulletCount > 0 && floatingBullet != null)
        {
            floatingBullet.SetActive(true);
            floatingBullet.transform.position = cameraTransform.TransformPoint(followOffset);
            floatingBullet.transform.rotation = cameraTransform.rotation;
        }
        //else if (floatingBullet != null)
        //{
        //    floatingBullet.SetActive(false);
        //}

        // SHOOTING: Left Mouse Click for HMD (Simulator mode)
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (bulletCount > 0) Shoot();
            else if (emptySound != null) emptySound.Play();
        }
    }

    // Connect this to your XR Grab Interactable 'Select Entered' event
    public void CollectBulletVR()
    {
        bulletCount++;
        UpdateUI();
    }

    void Shoot()
    {
        if (shootSound != null) shootSound.Play();
        bulletCount--;
        UpdateUI();

        // --- SPAWN TWO REPLACEMENT BULLETS FURTHER AWAY (Runtime Creation) ---
        if (bulletPrefab != null)
        {
            for (int i = 0; i < 2; i++)
            {
                // Calculate a position between 10 and 20 units away
                float spawnDist = Random.Range(20f, 20f);
                Vector2 randomDir = Random.insideUnitCircle.normalized * spawnDist;

                Vector3 spawnPosition = new Vector3(
                    transform.position.x + randomDir.x,
                    0.5f, // Keeps bullets at floor level
                    transform.position.z + randomDir.y
                );

                // Creates the new collectible objects at runtime
                //Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
                // Create the bullet
                GameObject newBullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
                newBullet.SetActive(true);
            }
        }

        // Raycast fires from the camera forward (Head Aim)
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
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

        target.SetActive(false); // Hides the UFO
        ufosRemaining--;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (bulletCounterText != null) { 
        
            bulletCounterText.text = "Bullet Count: " + bulletCount;
        
        }

        if (ufoCounterText != null) { 
        
            ufoCounterText.text = "UFOs Remaining: " + ufosRemaining;
        
        }


        if (ufosRemaining <= 0 && !hasWon && Time.timeSinceLevelLoad > 0.1f)
        {
            hasWon = true;
            if (winGraphic != null) { 
            
            winGraphic.SetActive(true);
            
            }
            if (winSound != null) { 
            
            winSound.Play();
            
            } 
            

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }


    }


}


