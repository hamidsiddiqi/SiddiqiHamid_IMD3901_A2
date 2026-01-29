using UnityEngine;

public class BulletPickup : MonoBehaviour
{
    public int bulletAmount = 1;
    public AudioClip pickupSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Try to find the Desktop script
            PlayerController desktopPlayer = other.GetComponent<PlayerController>();
            if (desktopPlayer != null)
            {
                desktopPlayer.AddBullet();
                FinalizePickup();
                return;
            }

            // Try to find the VR script
            HMDController vrPlayer = other.GetComponent<HMDController>();
            if (vrPlayer != null)
            {
                vrPlayer.CollectBulletVR();
                FinalizePickup();
            }
        }
    }

    void FinalizePickup()
    {
        if (pickupSound != null) AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        Destroy(gameObject);
    }
}