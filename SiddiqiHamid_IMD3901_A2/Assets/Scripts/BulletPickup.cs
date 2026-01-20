using UnityEngine;

public class BulletPickup : MonoBehaviour
{
    public int ammoAmount = 1;
    public AudioClip pickupSound; // Drag your .mp3 here directly

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the Player
        if (other.CompareTag("Player"))
        {
            // Connect to your PlayerController script
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
            {
                player.AddBullet(); // Calls the function in your PlayerController

                // Plays sound at the bullet's location
                if (pickupSound != null) AudioSource.PlayClipAtPoint(pickupSound, transform.position);

                // Requirement: Destroy objects during runtime
                Destroy(gameObject);
            }
        }
    }
}

