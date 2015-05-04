using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	private Color color = Color.white;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.tag == "Player") {
			Player player = collider.GetComponent<Player>();
			player.Moved();
			this.renderer.material.SetColor("_Color", Color.green);
		}

		if (collider.gameObject.tag == "Crate") {
			Crate crate = collider.GetComponent<Crate>();
			crate.SetTarget(this.transform.position);
			this.renderer.material.SetColor("_Color", Color.red);
		}
	}

	void OnTriggerExit(Collider collider) {
		this.renderer.material.SetColor("_Color", this.color);
	}

	public void SetAsGoal() {
		this.color = Color.blue;
		this.renderer.material.SetColor ("_Color", this.color);
	}
}
