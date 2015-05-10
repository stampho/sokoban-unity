using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI : MonoBehaviour {

	private GameObject menuContainer;
	private GameObject menu;
	private GameObject winDialog;

	void Start() {
		this.menuContainer = GameObject.Find ("MenuContainer");
		this.menu = GameObject.Find ("Menu");
		this.winDialog = GameObject.Find ("WinDialog");

		this.winDialog.SetActive (false);

		UpdateLevelLabel (GameManager.instance.GetCurrentLevelName());
		UpdateLevelButtons ();
	}

	private void UpdateLevelLabel(string levelName) {
		Text levelLabel = GameObject.Find ("LevelLabel").GetComponent<Text> ();
		levelLabel.text = levelName;
	}

	private void UpdateLevelButtons() {
		int currentLevel = GameManager.instance.GetCurrentLevel ();

		Button nextButton = GameObject.Find ("NextButton").GetComponent<Button>();
		nextButton.interactable = GameManager.instance.HasLevel (currentLevel + 1);

		Button prevButton = GameObject.Find ("PrevButton").GetComponent<Button>();
		prevButton.interactable = GameManager.instance.HasLevel (currentLevel - 1);
	}

	private void ShowMenu() {
		menuContainer.SetActive (true);
		this.winDialog.SetActive (false);
		this.menu.SetActive (true);
		Button hideButton = GameObject.Find ("HideButton").GetComponent<Button> ();
		hideButton.interactable = GameManager.instance.IsInGame ();
	}

	private void HideMenu() {
		menuContainer.SetActive (false);
	}

	public bool IsMenuVisible() {
		return menuContainer.activeSelf;
	}

	public void UpdateMoveCounter(int counter) {
		Text moveCounter = GameObject.Find ("MoveCounter").GetComponent<Text>();
		moveCounter.text = counter.ToString();
	}

	public void ShowWinDialog(int playerScore, int bestScore) {
		menuContainer.SetActive (true);
		this.menu.SetActive (false);
		this.winDialog.SetActive (true);

		Text playerScoreText = GameObject.Find ("PlayerScore").GetComponent<Text> ();
		playerScoreText.text = playerScore.ToString ();
		Text bestScoreText = GameObject.Find ("BestScore").GetComponent<Text> ();
		bestScoreText.text = bestScore.ToString ();

		Text messageText = GameObject.Find ("MessageText").GetComponent<Text> ();
		if (playerScore < bestScore) {
			messageText.text = "Congratulations!\nThe new best score is yours.";
		} else {
			messageText.text = "";
		}
	}

	public void RestartButtonPushed() {
		GameManager.instance.Restart ();
	}

	public void MenuButtonPushed() {
		ShowMenu ();
	}

	public void NewGameButtonPushed() {
		HideMenu ();
		GameManager.instance.Restart ();
	}

	public void PrevButtonPushed() {
		string levelName = GameManager.instance.PrevLevel ();
		UpdateLevelLabel (levelName);
		UpdateLevelButtons ();
	}

	public void NextButtonPushed() {
		string levelName = GameManager.instance.NextLevel ();
		UpdateLevelLabel (levelName);
		UpdateLevelButtons ();
	}

	public void TileLightValueChanged(bool value) {
		GameManager.instance.EnableTileLight (value);
	}

	public void ShadowValueChanged(bool value) {
		GameManager.instance.EnableShadow (value);
	}

	public void HideButtonPushed() {
		HideMenu ();
	}

	public void QuitButtonPushed() {
		Application.Quit ();
	}
}
