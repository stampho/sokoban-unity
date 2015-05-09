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
	private int currentLevel = 1;
	private GameObject menuContainer;
	private bool inGame;

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

		menuContainer = GameObject.Find ("MenuContainer");
		CollectLevels ();
		UpdateLevelLabel ();
		UpdateLevelButtons ();
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

	private void UpdateLevelLabel() {
		Text levelLabel = GameObject.Find ("LevelLabel").GetComponent<Text> ();
		levelLabel.text = levels [currentLevel];
	}

	private void LoadLevel(int level) {
		if (!HasLevel (level))
			return;

		Application.LoadLevel (this.levels [level]);
	}

	private void UpdateLevelButtons() {
		Button nextButton = GameObject.Find ("NextButton").GetComponent<Button>();
		nextButton.interactable = HasLevel (currentLevel + 1);

		Button prevButton = GameObject.Find ("PrevButton").GetComponent<Button>();
		prevButton.interactable = HasLevel (currentLevel - 1);
	}

	private bool HasLevel(int level) {
		return this.levels.ContainsKey (level);
	}

	public void ShowMenu() {
		menuContainer.SetActive (true);
		Button hideButton = GameObject.Find ("HideButton").GetComponent<Button> ();
		hideButton.interactable = inGame;
	}

	public void HideMenu() {
		menuContainer.SetActive (false);
	}

	public bool IsMenuVisible() {
		return menuContainer.activeSelf;
	}

	public void Restart() {
		LoadLevel (currentLevel);
	}

	public void UpdateMoveCounter(int counter) {
		Text moveCounter = GameObject.Find ("MoveCounter").GetComponent<Text>();
		moveCounter.text = counter.ToString();
	}

	public void LevelCompleted() {
		inGame = false;
		ShowMenu ();
	}

	public void NewGame() {
		Restart ();
		HideMenu ();
		inGame = true;
	}

	public void Quit() {
		Application.Quit ();
	}

	public void NextLevel() {
		if (!HasLevel (currentLevel + 1))
		    return;

		currentLevel++;
		UpdateLevelLabel ();
		UpdateLevelButtons ();
		LoadLevel (currentLevel);
	}

	public void PrevLevel() {
		if (!HasLevel (currentLevel - 1))
			return;

		currentLevel--;
		UpdateLevelLabel ();
		UpdateLevelButtons ();
		LoadLevel (currentLevel);
	}
}
