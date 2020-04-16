using System;
using UnityEngine;

public static class Score {

	public static event EventHandler OnHighscoreChanged;

	private static int score;

	public static void InitializeStatic ()
	{
		score = 0;
		OnHighscoreChanged = null;
	}

	public static int GetScore ()
	{
		return score;
	}

	public static void AddScore ()
	{
		score += 100;
	}

	public static int GetHighscore ()
	{
		return PlayerPrefs.GetInt ("highscore", 0);
	}

	public static bool TrySetNewHighscore ()
	{
		return TrySetNewHighscore (score);
	}

	public static bool TrySetNewHighscore (int score)
	{
		int higscore = GetHighscore ();

		if (score > higscore) {
			PlayerPrefs.SetInt ("highscore", score);
			PlayerPrefs.Save ();

			OnHighscoreChanged?.Invoke (null, EventArgs.Empty);
			return true;
		}

		return false;
	}
}
