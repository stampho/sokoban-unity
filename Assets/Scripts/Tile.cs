using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	private Color color = Color.white;
	private bool goal = false;
	private bool covered = false;

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
			this.covered = true;
			this.renderer.material.SetColor("_Color", Color.red);

			// TODO(pvarga): Reduce the size of the Trigger to make possible
			// to arrive for the crate to the center of the last goal tile
			if (this.goal) {
				crate.SetHighLight(true);
				Grid grid = this.transform.GetComponentInParent<Grid>();
				grid.CheckWin();
			}
		}
	}

	void OnTriggerExit(Collider collider) {
		if (collider.gameObject.tag == "Crate" && this.goal) {
			Crate crate = collider.GetComponent<Crate>();
			crate.SetHighLight(false);
			this.covered = false;
		}
		this.renderer.material.SetColor("_Color", this.color);
	}

	public void SetAsCovered() {
		this.covered = true;
	}

	public bool isCovered() {
		return this.covered;
	}

	public void SetAsGoal() {
		this.goal = true;
		this.color = Color.blue;
		this.renderer.material.SetColor ("_Color", this.color);
	}
}
