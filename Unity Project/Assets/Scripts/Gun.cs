using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
	public Rigidbody2D rocket;				// Prefab of the rocket.
	public float speed = 20f;				// The speed the rocket will fire at.
    public int ammo = 20;

	private PlayerControl playerCtrl;		// Reference to the PlayerControl script.
	private Animator anim;					// Reference to the Animator component.
    public static bool h = false;
    private SpriteRenderer ammobar;
    private Vector3 ammoscale;


    void Awake()
	{
		// Setting up the references.
		anim = transform.root.gameObject.GetComponent<Animator>();
		playerCtrl = transform.root.GetComponent<PlayerControl>();
        ammobar = GameObject.Find("AmmoBar").GetComponent<SpriteRenderer>();
        ammoscale = ammobar.transform.localScale;
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
        ammobar.transform.localScale = new Vector3(ammoscale.x * ammo * 0.05f, 3, 1);
    }
}
