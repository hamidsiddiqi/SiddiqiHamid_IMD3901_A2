using UnityEngine;

public class BulletPickup : MonoBehaviour
{
    public int bulletAmount = 1;
    public AudioClip pickupSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // For Desktop
            PlayerController desktopPlayer = other.GetComponent<PlayerController>();
            if (desktopPlayer != null)
            {
                desktopPlayer.AddBullet();
                FinalizePickup();
                return;
            }

            // For HMD
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