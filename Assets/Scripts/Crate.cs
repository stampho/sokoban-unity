using UnityEngine;
using System.Collections;

public class Crate : MonoBehaviour {

	private Vector3 targetPosition;
	private bool moving = false;

	// Use this for initialization
	void Start () {
		this.light.enabled = false;
		this.targetPosition = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		// Free Fall
		if (!this.rigidbody.isKinematic)
			return;

		if (this.transform.position == this.targetPosition) {
			this.moving = false;
			return;
		}

		if (this.moving) {
			float step = 3.5f * Time.deltaTime;
			this.transform.position = Vector3.MoveTowards (this.transform.position, this.targetPosition, step);
		}
	}

	public void Push(Vector3 direction) {
		//Debug.DrawRay (this.transform.position, direction, Color.red, 1, true);
		//Debug.Log ("Push " + direction + " power: " + power);

		if (this.moving)
			return;

		// Don't move if there is a crate or wall in the given direction
		if (Physics.Raycast (this.transform.position, direction, 0.5f)) {
			this.moving = false;
			return;
		}

		this.moving = true;
		this.targetPosition = transform.position + direction;
	}
}
