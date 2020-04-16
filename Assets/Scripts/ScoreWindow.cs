using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreWindow : MonoBehaviour {

	private static ScoreWindow instance;

	private Text scoreText;
	private Text highScoreText;

	private void Awake ()
	{
		instance = this;

		scoreText = transform.Find ("ScoreText").GetComponent<Text> ();
		highScoreText = transform.Find ("HighScoreText").GetComponent<Text> ();

		Score.OnHighscoreChanged += Score_OnHighscoreChanged;
		UpdateHighscore ();
	}

	private void Score_OnHighscoreChanged (object sender, System.EventArgs e)
	{
		UpdateHighscore ();
	}

	private void Update ()
	{
		scoreText.text = Score.GetScore ().ToString ();
	}

	private void UpdateHighscore ()
	{
		highScoreText.text = Score.GetHighscore ().ToString ();
	}

	public static void HideStatic ()
	{
		instance.gameObject.SetActive (false);
	}
}
