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
		StartCoroutine(whatthisdoes(30));
	}

 	private IEnumerator whatthisdoes(int lifeSpan) {
  		yield return new WaitForSeconds(lifeSpan);
    		destroy(gameObject);
      		yield return;
  	}
}
