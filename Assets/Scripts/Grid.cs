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
			line.Reverse();
			foreach (char c in line) {
				xOffset += distanceBetweenTiles;

				if (c == '0')
					continue;

				Tile newTile = (Tile)Instantiate(tilePrefab, new Vector3(transform.position.x + xOffset, transform.position.y, transform.position.z + zOffset), transform.rotation);

				switch(c) {
				case 'w':
					// TODO(pvarga): YPos should be factored out
					float wallYPos = (wallPrefab.renderer.bounds.size.y / 2) + (newTile.renderer.bounds.size.y / 2);
					Instantiate (wallPrefab, new Vector3(newTile.transform.position.x, wallYPos, newTile.transform.position.z), transform.rotation);
					break;
				case 'p':
					// TODO(pvarga): YPos should be factored out
					float playerYPos = (player.renderer.bounds.size.y / 2) + (newTile.renderer.bounds.size.y / 2);
					player.SetPosition(new Vector3(newTile.transform.position.x, playerYPos, newTile.transform.position.z));
					break;

				case 'c':
					// TODO(pvarga): YPos should be factored out
					float crateYPos = (player.renderer.bounds.size.y / 2) + (newTile.renderer.bounds.size.y / 2);
					Instantiate (cratePrefab, new Vector3(newTile.transform.position.x, crateYPos, newTile.transform.position.z), transform.rotation);
					break;
				}
			}

			xOffset = 0.0f;
			zOffset += distanceBetweenTiles;
		}
	}

	// Update is called once per frame
	void Update () {

	}
}
