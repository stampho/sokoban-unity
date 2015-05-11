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
		string[] lines = levelDescriptor.text.Split('\n');
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

				if ("twpcgq".IndexOf(c) == -1) {
					Debug.Log("Unknown character in Level Descriptor: " + c);
					continue;
				}

				Tile newTile = (Tile)Instantiate(tilePrefab, new Vector3(transform.position.x + xOffset, transform.position.y, transform.position.z + zOffset), transform.rotation);
				newTile.transform.parent = this.transform;

				switch(c) {
				case 't':
					break;
				case 'w':
					Instantiate (wallPrefab, this.calcPos(wallPrefab.gameObject, newTile), transform.rotation);
					break;
				case 'p':
					player.SetPosition(this.calcPos (player.gameObject, newTile));
					break;
				case 'c': {
					Crate crate = (Crate)Instantiate (cratePrefab, this.calcPos (cratePrefab.gameObject, newTile), transform.rotation);
					this.crateList.Add (crate);
					newTile.SetCrateAbove (crate);
					break;
				}
				case 'g':
					this.goalTileList.Add(newTile);
					newTile.SetAsGoal ();
					break;
				case 'q':{
					Crate crate = (Crate)Instantiate (cratePrefab, this.calcPos (cratePrefab.gameObject, newTile), transform.rotation);
					this.crateList.Add (crate);

					this.goalTileList.Add(newTile);
					newTile.SetAsGoal ();
					newTile.SetCrateAbove (crate);
					break;
				}
				}
			}

			xOffset = 0.0f;
			zOffset += distanceBetweenTiles;
		}
	}

	public bool CheckCompleted () {
		foreach (Tile tile in this.goalTileList) {
			if (!tile.isCovered ())
				return false;
		}

		foreach (Tile tile in this.goalTileList) {
			tile.rigidbody.isKinematic = false;
			tile.rigidbody.constraints = RigidbodyConstraints.None;
			tile.rigidbody.AddForce (new Vector3(0.0f, -1.0f, 0.0f));
		}

		foreach (Crate crate in this.crateList) {
			crate.rigidbody.isKinematic = false;
			crate.rigidbody.constraints = RigidbodyConstraints.None;
		}

		GameManager.instance.LevelCompleted (player.GetCounter());
		return true;
	}

	private Vector3 calcPos(GameObject obj, Tile tile) {
		float objYPos = (obj.renderer.bounds.size.y / 2) + (tile.renderer.bounds.size.y / 2);
		return new Vector3(tile.transform.position.x, objYPos, tile.transform.position.z);
	}
}
