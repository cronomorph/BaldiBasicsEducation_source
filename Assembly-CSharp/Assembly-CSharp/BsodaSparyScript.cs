// BsodaSparyScript
using UnityEngine;

public class BsodaSparyScript : MonoBehaviour
{
	public float speed;

	private float lifeSpan;

	private Rigidbody rb;

	private void Start()
	{
		rb = base.GetComponent<Rigidbody>();
		rb.velocity = base.transform.forward * speed;
		lifeSpan = 30f;
	}

	private void FixedUpdate() // So it updates based off the physics engine
	{
		lifeSpan -= Time.deltaTime;
		if (lifeSpan <= 0f)
		{
			Destroy(gameObject);
		}
	}
}
