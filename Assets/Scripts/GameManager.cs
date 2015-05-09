using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class GameManager : MonoBehaviour {

	private static GameManager _instance;

	private Dictionary<int, string> levels;

	public static GameManager instance {
		get {
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType<GameManager> ();
			}

			return _instance;
		}
	}

	void Awake() {
		if (GameManager.instance != this) {
			DestroyImmediate (this.gameObject);
			return;
		}
		DontDestroyOnLoad (this);
		CollectLevels ();
	}

	private void CollectLevels() {
		this.levels = new Dictionary<int, string> ();
		string[] guids = AssetDatabase.FindAssets ("*", new string[1] {"Assets/_Scenes"});
		foreach (string guid in guids) {
			string levelPath = AssetDatabase.GUIDToAssetPath(guid);
			Regex regex = new Regex(@"^.+/(level(\d+))\.unity");
			Match match = regex.Match(levelPath);
			if (match.Success) {
				this.levels.Add(Convert.ToInt32 (match.Groups[2].Value.ToString()), match.Groups[1].Value);
			}
		}
	}

	private void loadLevel(int level) {
		if (!this.levels.ContainsKey (level))
			level = 1;

		Application.LoadLevel (this.levels [level]);
	}

	public void Restart() {
		Application.LoadLevel (Application.loadedLevelName);
	}

	public void UpdateMoveCounter(int counter) {
		Text moveCounter = GameObject.Find ("MoveCounter").GetComponent<Text>();
		moveCounter.text = counter.ToString();
	}

	public void LevelCompleted() {
		int next = Application.loadedLevel + 1;
		Application.LoadLevel (next);
	}
}
