﻿using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class GameManager : MonoBehaviour {

	public UI uiPrefab;
	public Light lightPrefab;

	private static GameManager _instance;

	private UI ui;
	private Light light;

	private Dictionary<int, string> levels;
	private Dictionary<int, int> bestScores;

	private int currentLevel = 1;
	private bool inGame;
	private bool tileLight = false;

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

		this.ui = (UI)Instantiate (this.uiPrefab);
		DontDestroyOnLoad (this.ui);

		this.light = (Light)Instantiate (this.lightPrefab);
		DontDestroyOnLoad (this.light);

		this.levels = CollectLevels();
		this.bestScores = LoadBestScores ();
	}

	private Dictionary<int, string> CollectLevels() {
		Dictionary<int, string> levels = new Dictionary<int, string> ();
		string[] guids = AssetDatabase.FindAssets ("*", new string[1] {"Assets/_Scenes"});
		foreach (string guid in guids) {
			string levelPath = AssetDatabase.GUIDToAssetPath(guid);
			Regex regex = new Regex(@"^.+/(level(\d+))\.unity");
			Match match = regex.Match(levelPath);
			if (match.Success)
				levels.Add(Convert.ToInt32 (match.Groups[2].Value.ToString()), match.Groups[1].Value);
		}

		return levels;
	}

	private Dictionary<int, int> LoadBestScores() {
		Dictionary<int, int> bestScores = new Dictionary<int, int> ();
		foreach (int key in this.levels.Keys) {
			bestScores.Add(key, 999);
		}

		return bestScores;
	}

	private void LoadLevel(int level) {
		if (!HasLevel (level))
			return;

		Application.LoadLevel (this.levels [level]);
	}

	public bool HasLevel(int level) {
		return this.levels.ContainsKey (level);
	}

	public void Restart() {
		inGame = true;
		LoadLevel (currentLevel);
	}

	public void LevelCompleted(int playerScore) {
		inGame = false;
		int bestScore = this.bestScores [currentLevel];
		if (bestScore > playerScore)
			this.bestScores [currentLevel] = playerScore;

		this.ui.ShowWinDialog (playerScore, bestScore);
	}

	public string NextLevel() {
		if (!HasLevel (currentLevel + 1))
		    return levels[currentLevel];

		LoadLevel (++currentLevel);
		return levels[currentLevel];
	}

	public string PrevLevel() {
		if (!HasLevel (currentLevel - 1))
			return levels[currentLevel];

		LoadLevel (--currentLevel);
		return levels[currentLevel];
	}

	public string GetCurrentLevelName() {
		return levels[currentLevel];
	}

	public int GetCurrentLevel() {
		return currentLevel;
	}

	public bool IsInGame() {
		return this.inGame;
	}

	public UI GetUI() {
		return this.ui;
	}

	public void EnableTileLight(bool enable) {
		this.tileLight = enable;
	}

	public bool IsTileLightEnabled() {
		return this.tileLight;
	}

	public void EnableShadow(bool enable) {
		this.light.shadows = enable ? LightShadows.Hard : LightShadows.None;
	}
}
