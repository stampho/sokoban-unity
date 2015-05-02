using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.tag == "Player") {
			this.renderer.material.SetColor("_Color", Color.green);
		}

		if (collider.gameObject.tag == "Crate") {
			this.renderer.material.SetColor ("_Color", Color.red);
			Crate crate = collider.GetComponent<Crate>();
			crate.SetTarget(this.transform.position);
		}
	}

	void OnTriggerExit(Collider collider) {
		if (collider.gameObject.tag == "Player") {
			this.renderer.material.SetColor("_Color", Color.white);
		}

		if (collider.gameObject.tag == "Crate") {
			this.renderer.material.SetColor("_Color", Color.white);
		}
	}
}
