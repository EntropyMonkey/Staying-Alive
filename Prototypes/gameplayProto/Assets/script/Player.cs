using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	public float jumpSpeed = 20;
	public float movementSpeed;

	void Awake()
	{
	}

	// Use this for initialization
	void Start () 
	{
	}

	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKey(KeyCode.A))
		{
			rigidbody.AddForce(Vector3.left * movementSpeed * Time.deltaTime, ForceMode.Impulse);
		}
		else if (Input.GetKey(KeyCode.D))
		{
			rigidbody.AddForce(Vector3.right * movementSpeed * Time.deltaTime, ForceMode.Impulse);
		}

		if (Input.GetKey(KeyCode.W))
		{
			rigidbody.AddForce(Vector3.up * jumpSpeed * Time.deltaTime, ForceMode.Impulse);
		}
	}

	void OnCollisionEnter(Collision collision)
	{
	}
}
