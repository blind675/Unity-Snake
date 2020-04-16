using UnityEngine;

public class GameHandler : MonoBehaviour {

	private static GameHandler instance;

	[SerializeField] private Snake snake;
	private LevelGrid levelGrid;

	private void Awake ()
	{
		instance = this;
		Score.InitializeStatic ();
	}

	private void Start ()
	{

		levelGrid = new LevelGrid (20, 20);

		snake.Setup (levelGrid);
		levelGrid.Setup (snake);
		Time.timeScale = 1f;
	}

	private void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (IsGamePaused ()) {
				GameHandler.ResumeGame ();
			} else {
				GameHandler.PauseGame ();
			}
		}
	}

	public static void SnakeDied ()
	{
		bool isNewHighScore = Score.TrySetNewHighscore ();
		GameOverWindow.ShowStatic (isNewHighScore);
		ScoreWindow.HideStatic ();
	}

	public static void ResumeGame ()
	{
		PausedWindow.HideStatic ();
		Time.timeScale = 1f;
	}

	public static void PauseGame ()
	{
		PausedWindow.ShowStatic ();
		Time.timeScale = 0f;
	}

	public static bool IsGamePaused ()
	{
		return Time.timeScale == 0f;
	}
}
