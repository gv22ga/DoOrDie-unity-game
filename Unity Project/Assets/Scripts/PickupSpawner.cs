using UnityEngine;
using System.Collections;

public class PickupSpawner : MonoBehaviour
{
	public GameObject[] pickups;				// Array of pickup prefabs with the bomb pickup first and health second.
	public float pickupDeliveryTime = 5f;		// Delay on delivery.
	public float dropRangeLeft;					// Smallest value of x in world coordinates the delivery can happen at.
	public float dropRangeRight;				// Largest value of x in world coordinates the delivery can happen at.
	public float highHealthThreshold = 75f;		// The health of the player, above which only bomb crates will be delivered.
	public float lowHealthThreshold = 25f;		// The health of the player, below which only health crates will be delivered.


	private PlayerHealth playerHealth;			// Reference to the PlayerHealth script.
    private PlayerHealth2 playerHealth2;
    private Gun gun1;
    private Gun2 gun2;

    void Awake ()
	{
		// Setting up the reference.
		playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        playerHealth2 = GameObject.FindGameObjectWithTag("Player2").GetComponent<PlayerHealth2>();
        gun1 = GameObject.FindGameObjectWithTag("Gun1").GetComponent<Gun>();
        gun2 = GameObject.FindGameObjectWithTag("Gun2").GetComponent<Gun2>();
    }


	void Start ()
	{
		// Start the first delivery.
		StartCoroutine(DeliverPickup());
	}


    public IEnumerator DeliverPickup()
    {
        // Wait for the delivery delay.
        yield return new WaitForSeconds(pickupDeliveryTime);

        // Create a random x coordinate for the delivery in the drop range.
        float dropPosX = Random.Range(dropRangeLeft, dropRangeRight);
        float dropPosX2 = Random.Range(dropRangeLeft, dropRangeRight);
        float dropPosX3 = Random.Range(dropRangeLeft, dropRangeRight);
        float dropPosX4 = Random.Range(dropRangeLeft, dropRangeRight);

        // Create a position with the random x coordinate.
        Vector3 dropPos = new Vector3(dropPosX, 15f, 1f);
        Vector3 dropPos2 = new Vector3(dropPosX2, 15f, 1f);
        Vector3 dropPos3 = new Vector3(dropPosX3, 15f, 1f);
        Vector3 dropPos4 = new Vector3(dropPosX4, 15f, 1f);

        bool fallen = false;

        if (gun1.ammo <= 5 || gun2.ammo <= 5)
        {
            Instantiate(pickups[2], dropPos4, Quaternion.identity);
            fallen = true;
        }
        if (playerHealth.health <= lowHealthThreshold || playerHealth2.health <= lowHealthThreshold)
        {
            Instantiate(pickups[1], dropPos, Quaternion.identity);
            fallen = true;
        }
        if (playerHealth.health >= highHealthThreshold && playerHealth2.health >= highHealthThreshold) { 
            Instantiate(pickups[0], dropPos2, Quaternion.identity);
            fallen = true;
        }
        if (fallen==false)
        {
            int pickupIndex = Random.Range(0, 2);
            Instantiate(pickups[pickupIndex], dropPos3, Quaternion.identity);
            
        }
        // If the player's health is above the high threshold...
        /*if (playerHealth.health >= highHealthThreshold)
			// ... instantiate a bomb pickup at the drop position.
			Instantiate(pickups[0], dropPos, Quaternion.identity);
		// Otherwise if the player's health is below the low threshold...
		else if(playerHealth.health <= lowHealthThreshold)
			// ... instantiate a health pickup at the drop position.
			Instantiate(pickups[1], dropPos, Quaternion.identity);
		// Otherwise...
		else
		{
			// ... instantiate a random pickup at the drop position.
			int pickupIndex = Random.Range(0, pickups.Length);
			Instantiate(pickups[pickupIndex], dropPos, Quaternion.identity);
		}*/
    }
}
