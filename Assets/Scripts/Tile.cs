using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	private Color color = Color.white;
	private bool goal = false;
	private bool checkPerformed = true;

	private Crate crateAbove = null;
	private Grid grid;

	// Use this for initialization
	void Start () {
		this.grid = this.transform.GetComponentInParent<Grid>();
	}
	
	// Update is called once per frame
	void Update () {
		if (this.goal && this.crateAbove != null) {
			this.crateAbove.light.enabled = true;
			if (!this.checkPerformed
				&& this.crateAbove.transform.position.x == this.transform.position.x
			    && this.crateAbove.transform.position.z == this.transform.position.z) {

				grid.CheckCompleted ();
				this.checkPerformed = true;
			}
		}
	}
	
	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.tag == "Player") {
			Player player = collider.GetComponent<Player>();
			player.Moved();
			if (GameManager.instance.IsTileLightEnabled())
				this.renderer.material.SetColor("_Color", Color.green);
		}

		if (collider.gameObject.tag == "Crate") {
			this.crateAbove = collider.GetComponent<Crate>();
			this.checkPerformed = false;
			if (GameManager.instance.IsTileLightEnabled())
				this.renderer.material.SetColor("_Color", Color.red);
		}
	}

	void OnTriggerExit(Collider collider) {
		if (collider.gameObject.tag == "Crate") {
			if (this.goal)
				this.crateAbove.light.enabled = false;
			this.crateAbove = null;
		}
		this.renderer.material.SetColor("_Color", this.color);
	}

	public void SetCrateAbove(Crate crate) {
		this.crateAbove = crate;
		this.crateAbove.light.enabled = true;
	}

	public bool isCovered() {
		return this.crateAbove != null;
	}

	public void SetAsGoal() {
		this.goal = true;
		this.color = Color.blue;
		this.renderer.material.SetColor ("_Color", this.color);
	}
}
