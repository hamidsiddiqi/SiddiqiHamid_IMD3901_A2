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
    public float range = 100f;

    [Header("Bullet Follow (Manipulation)")]
    public GameObject floatingBullet;
    public Vector3 followOffset = new Vector3(0.5f, -0.4f, 1f);
    public Transform cameraTransform; // Drag Main Camera here

    public float explosionDelay = 0.2f;
    private int ufosRemaining;

    void Start()
    {
        ufosRemaining = GameObject.FindGameObjectsWithTag("UFO").Length;
        if (music != null) { music.loop = true; music.Play(); }
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
        else if (floatingBullet != null)
        {
            floatingBullet.SetActive(false);
        }

        // SHOOTING: Left Mouse Click for HMD
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (bulletCount > 0) Shoot();
            else if (emptySound != null) emptySound.Play();
        }
    }

    // Call this function from your XR Grab Interactable Events
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

        // Raycast fires from the camera forward (VR Head Aim)
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
        target.SetActive(false);
        ufosRemaining--;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (bulletCounterText != null) bulletCounterText.text = "Bullet Count: " + bulletCount;
        if (ufoCounterText != null) ufoCounterText.text = "UFOs Remaining: " + ufosRemaining;
    }
}