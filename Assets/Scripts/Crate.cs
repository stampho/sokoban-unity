using UnityEngine;
using System.Collections;

public class Crate : MonoBehaviour {

	private Rigidbody body;
	private Vector3 target;
	private Vector3 initialPosition;
	
	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody> ();
		initialPosition = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		float magnitude = this.body.velocity.magnitude;
		if (magnitude > 0.1f) {
			float dist = Vector2.Distance (new Vector2(this.transform.position.x, this.transform.position.z),
			                               new Vector2(this.target.x, this.target.z));

			// FIXME(pvarga): Crate doesn't stop: keep moving the object then push it again
			if (dist <= 0.001f && initialPosition != this.transform.position) {
				this.body.velocity = Vector3.zero;
			}
		}
	}

	public void SetTarget(Vector3 target) {
		this.target = target;
	}

	public void Push(Vector3 direction, float power) {
		this.initialPosition = this.transform.position;
		this.body.velocity = direction * power;
	}
}
