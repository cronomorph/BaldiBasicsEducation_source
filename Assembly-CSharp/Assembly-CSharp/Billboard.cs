using UnityEngine;

public class Billboard : MonoBehaviour
{
	private Camera m_Camera;

	private void Start()
	{
		m_Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
	}

	private void Update()
	{
		base.transform.rotation = Quaternion.Euler(Camera.current.transform.rotation.eulerAngles.x, m_Camera.transform.rotation.eulerAngles.y, 0f);
	}
}
