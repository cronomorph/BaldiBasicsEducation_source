// StartButton
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
	private Button button;

	private void Start()
	{
		button = base.GetComponent<Button>();
		button.onClick.AddListener(StartGame);
	}

	private void StartGame()
	{
		switch (base.name) {
			case "StoryButton":
				PlayerPrefs.SetString("CurrentMode", "story");
			break;
			
			case "EndlessButton":
				PlayerPrefs.SetString("CurrentMode", "endless");
			break;
		}
		SceneManager.LoadScene("School");
	}
}

