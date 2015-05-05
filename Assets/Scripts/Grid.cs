using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

	public TextAsset levelDescriptor;
	public Tile tilePrefab;
	public Wall wallPrefab;
	public Crate cratePrefab;
	public Player player;
	
	private List<ArrayList> levelMap = new List<ArrayList>();
	private List<Tile> goalTileList = new List<Tile>();
	private List<Crate> crateList = new List<Crate>();

	// Use this for initialization
	void Start () {
		// Parse TextAsset
		string[] lines = levelDescriptor.text.Split("\n"[0]);
		foreach (string line in lines) {
			if (!string.IsNullOrEmpty(line)) {
				levelMap.Add(new ArrayList(line.ToCharArray ()));
			}
		}

		// Generate Grid
		float xOffset = 0.0f;
		float zOffset = 0.0f;

		float distanceBetweenTiles = tilePrefab.renderer.bounds.size.x;

		levelMap.Reverse ();

		foreach (ArrayList line in levelMap) {
			//line.Reverse();
			foreach (char c in line) {
				xOffset += distanceBetweenTiles;

				if (c == '0')
					continue;

				if ("twpcg".IndexOf(c) == -1) {
					Debug.Log("Unknown character in Level Descriptor: " + c);
					continue;
				}


				Tile newTile = (Tile)Instantiate(tilePrefab, new Vector3(transform.position.x + xOffset, transform.position.y, transform.position.z + zOffset), transform.rotation);
				newTile.transform.parent = this.transform;

				switch(c) {
				case 't':
					break;
				case 'w':
					// TODO(pvarga): YPos should be factored out
					float wallYPos = (wallPrefab.renderer.bounds.size.y / 2) + (newTile.renderer.bounds.size.y / 2);
					Instantiate (wallPrefab, new Vector3(newTile.transform.position.x, wallYPos, newTile.transform.position.z), transform.rotation);
					newTile.SetAsCovered ();
					break;
				case 'p':
					// TODO(pvarga): YPos should be factored out
					float playerYPos = (player.renderer.bounds.size.y / 2) + (newTile.renderer.bounds.size.y / 2);
					player.SetPosition(new Vector3(newTile.transform.position.x, playerYPos, newTile.transform.position.z));
					break;
				case 'c':
					// TODO(pvarga): YPos should be factored out
					float crateYPos = (player.renderer.bounds.size.y / 2) + (newTile.renderer.bounds.size.y / 2);
					Crate crate = (Crate)Instantiate (cratePrefab, new Vector3(newTile.transform.position.x, crateYPos, newTile.transform.position.z), transform.rotation);
					this.crateList.Add (crate);
					break;
				case 'g':
					this.goalTileList.Add(newTile);
					newTile.SetAsGoal ();
					break;
				}
			}

			xOffset = 0.0f;
			zOffset += distanceBetweenTiles;
		}
	}

	public bool CheckWin () {
		foreach (Tile tile in this.goalTileList) {
			if (!tile.isCovered ())
				return false;
		}

		Debug.Log ("YOU WON!");
		foreach (Tile tile in this.goalTileList) {
			tile.rigidbody.isKinematic = false;
			tile.rigidbody.AddForce (new Vector3(0.0f, -1.0f, 0.0f));
		}

		foreach (Crate crate in this.crateList) {
			crate.EnableFreeFall();
			crate.rigidbody.constraints = RigidbodyConstraints.None;
		}

		return true;
	}

	// Update is called once per frame
	void Update () {

	}
}
