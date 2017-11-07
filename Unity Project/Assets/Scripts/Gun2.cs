using UnityEngine;
using System.Collections;

public class Gun2 : MonoBehaviour
{
	public Rigidbody2D rocket;				// Prefab of the rocket.
	public float speed = 20f;				// The speed the rocket will fire at.
    public int ammo = 20;

    private PlayerControl2 playerCtrl;		// Reference to the PlayerControl script.
	private Animator anim;                  // Reference to the Animator component.
    private SpriteRenderer ammobar2;
    private Vector3 ammoscale;
    public static bool h = false;

    void Awake()
	{
		// Setting up the references.
		anim = transform.root.gameObject.GetComponent<Animator>();
		playerCtrl = transform.root.GetComponent<PlayerControl2>();
        ammobar2 = GameObject.Find("AmmoBar2").GetComponent<SpriteRenderer>();
        ammoscale = ammobar2.transform.localScale;
    }


	void Update ()
	{
		// If the fire button is pressed...
		if(h&&ammo>0)
		{
            // ... set the animator Shoot trigger parameter and play the audioclip.
            ammo--;
            UpdateAmmoBar();
			anim.SetTrigger("Shoot");
			GetComponent<AudioSource>().Play();

			// If the player is facing right...
			if(playerCtrl.facingRight)
			{
				// ... instantiate the rocket facing right and set it's velocity to the right. 
				Rigidbody2D bulletInstance = Instantiate(rocket, transform.position, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
				bulletInstance.velocity = new Vector2(speed, 0);
			}
			else
			{
				// Otherwise instantiate the rocket facing left and set it's velocity to the left.
				Rigidbody2D bulletInstance = Instantiate(rocket, transform.position, Quaternion.Euler(new Vector3(0,0,180f))) as Rigidbody2D;
				bulletInstance.velocity = new Vector2(-speed, 0);
			}
            h = false;
        }
	}
    public void UpdateAmmoBar()
    {
        ammobar2.transform.localScale = new Vector3(ammoscale.x * ammo * 0.05f, 3, 1);
    }
}
