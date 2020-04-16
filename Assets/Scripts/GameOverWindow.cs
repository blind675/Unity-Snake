
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class GameOverWindow : MonoBehaviour {

	private static GameOverWindow instance;

	private void Awake ()
	{
		instance = this;

		transform.Find ("RetryButton").GetComponent<Button_UI> ().ClickFunc = () => {
			Loader.Load (Loader.Scene.GameScene);
		};
		transform.Find ("RetryButton").GetComponent<Button_UI> ().AddButtonSound ();

		Hide ();
	}

	private void Show (bool isNewHighScore)
	{
		gameObject.SetActive (true);

		transform.Find ("NewHighScoreText").gameObject.SetActive (isNewHighScore);

		transform.Find ("ScoreText").GetComponent<Text> ().text = Score.GetScore ().ToString ();

		if (isNewHighScore) {
			transform.Find ("HighScoreText").gameObject.SetActive (false);
		} else {
			transform.Find ("HighScoreText").GetComponent<Text> ().text = "HIGHSCORE " + Score.GetHighscore ();
		}

	}

	private void Hide ()
	{
		gameObject.SetActive (false);
	}

	public static void ShowStatic (bool isNewHighScore)
	{
		instance.Show (isNewHighScore);
	}
}
