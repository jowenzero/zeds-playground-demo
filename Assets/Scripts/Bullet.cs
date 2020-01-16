using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 50f;
    public int damage = 20;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * speed;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        EbotScript emilibot = hitInfo.GetComponent<EbotScript>();
        DDScript dummyDrone = hitInfo.GetComponent<DDScript>();
        FDScript flyingDrone = hitInfo.GetComponent<FDScript>();
		FDScriptAI flyingDroneAI = hitInfo.GetComponent<FDScriptAI>();
		WoodenDummyScript woodenDummy = hitInfo.GetComponent<WoodenDummyScript>();

		// detect entity hit and apply damage
		if (emilibot != null)
        {
            emilibot.TakeDamage(damage);
        }
        else if (dummyDrone != null)
        {
            dummyDrone.TakeDamage(damage);
        }
        else if (flyingDrone != null)
        {
            flyingDrone.TakeDamage(damage);
        }
		else if (flyingDroneAI != null)
		{
			flyingDroneAI.TakeDamage(damage);
		}
		else if (woodenDummy != null)
		{
			woodenDummy.TakeDamage(damage);
		}

		// despawn bullet
		Destroy(gameObject);
    }
}
