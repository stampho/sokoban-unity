﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	private CharacterController controller;
	private int counter;

	// Use this for initialization
	void Start () {
		this.controller = GetComponent<CharacterController>();
		this.counter = -1;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 moveDirection = Vector3.zero;
		float speed = 2.5f;

		moveDirection = new Vector3 (Input.GetAxis ("Horizontal"), 0.0f, Input.GetAxis ("Vertical"));
		moveDirection = this.transform.TransformDirection (moveDirection);
		moveDirection *= speed;

		controller.Move (moveDirection * Time.deltaTime);
	}

	public void OnControllerColliderHit(ControllerColliderHit hit) {
		if (hit.gameObject.tag == "Crate") {
			Crate crate = hit.gameObject.GetComponent<Crate> ();

			float xDirection = hit.moveDirection.x;
			float zDirection = hit.moveDirection.z;
			
			if (xDirection == 1.0f || xDirection == -1.0f) {
				zDirection = 0.0f;
			} else if (zDirection == 1.0f || zDirection == -1.0f) {
				xDirection = 0.0f;
			} else {
				xDirection = 0.0f;
				zDirection = 0.0f;
			}

			Vector3 direction = new Vector3 (xDirection, 0.0f, zDirection);
			crate.Push (direction);
		}
	}

	public void OnGUI() {
		GUI.Label (new Rect (0, 0, 100, 100), "Moves: " + counter);
	}

	public void SetPosition(Vector3 position) {
		this.transform.position = position;
	}

	public void Moved() {
		this.counter++;
	}
}
