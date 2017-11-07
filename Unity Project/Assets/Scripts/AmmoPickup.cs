using UnityEngine;
using System.Collections;

public class AmmoPickup : MonoBehaviour
{
                   // How much health the crate gives the player.
    public AudioClip collect;               // The sound of the crate being collected.


    private PickupSpawner pickupSpawner;    // Reference to the pickup spawner.
    private Animator anim;                  // Reference to the animator component.
    private bool landed;                    // Whether or not the crate has landed.
    private Gun gun1;
    private Gun2 gun2;

    void Awake()
    {
        // Setting up the references.
        pickupSpawner = GameObject.Find("pickupManager").GetComponent<PickupSpawner>();
        anim = transform.root.GetComponent<Animator>();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        // If the player enters the trigger zone...
        if (other.tag == "Player")
        {
            // Get a reference to the player health script.
            gun1=GameObject.FindGameObjectWithTag("Gun1").GetComponent<Gun>();

            gun1.ammo += 20;
            gun1.UpdateAmmoBar();
            // Update the health bar.
            //playerHealth.UpdateHealthBar();

            // Trigger a new delivery.
            pickupSpawner.StartCoroutine(pickupSpawner.DeliverPickup());

            // Play the collection sound.
            AudioSource.PlayClipAtPoint(collect, transform.position);

            // Destroy the crate.
            Destroy(transform.root.gameObject);
        }
        if (other.tag == "Player2")
        {
            // Get a reference to the player health script.
            gun2 = GameObject.FindGameObjectWithTag("Gun2").GetComponent<Gun2>();

            gun2.ammo += 20;
            gun2.UpdateAmmoBar();
            // Update the health bar.
            //playerHealth.UpdateHealthBar();

            // Trigger a new delivery.
            pickupSpawner.StartCoroutine(pickupSpawner.DeliverPickup());

            // Play the collection sound.
            AudioSource.PlayClipAtPoint(collect, transform.position);

            // Destroy the crate.
            Destroy(transform.root.gameObject);
        }
        // Otherwise if the crate hits the ground...
        else if (other.tag == "ground" && !landed)
        {
            // ... set the Land animator trigger parameter.
            anim.SetTrigger("Land");

            transform.parent = null;
            gameObject.AddComponent<Rigidbody2D>();
            landed = true;
        }
    }
}
