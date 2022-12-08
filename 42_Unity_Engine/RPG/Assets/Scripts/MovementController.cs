using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
//1
	public float movementSpeed = 3.0f;
	// 2
	Vector2 movement = new Vector2();
	// 3
	Rigidbody2D rb2D;
	private void Start()
	{
	// 4
		rb2D = GetComponent<Rigidbody2D>();
	}
	private void Update()
	{
	// Keep this empty for now
	}
	// 5
		void FixedUpdate()
	{
	// 6
		movement.x = Input.GetAxisRaw("Horizontal");
		movement.y = Input.GetAxisRaw("Vertical");
	// 7
		movement.Normalize();
	// 8
		rb2D.velocity = movement * movementSpeed;
	}
}
