using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI : MonoBehaviour {

	private GameObject menuContainer;

	void Start() {
		this.menuContainer = GameObject.Find ("MenuContainer");

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

	public void HideButtonPushed() {
		HideMenu ();
	}

	public void QuitButtonPushed() {
		Application.Quit ();
	}
}
