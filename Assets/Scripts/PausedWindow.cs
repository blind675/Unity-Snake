using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class PausedWindow : MonoBehaviour {

	private static PausedWindow instance;

	private void Awake ()
	{
		instance = this;

		transform.GetComponent<RectTransform> ().anchoredPosition = Vector2.zero;
		transform.GetComponent<RectTransform> ().sizeDelta = Vector2.zero;

		transform.Find ("ResumeButton").GetComponent<Button_UI> ().ClickFunc = () => {
			GameHandler.ResumeGame ();
		};
		transform.Find ("ResumeButton").GetComponent<Button_UI> ().AddButtonSound ();

		transform.Find ("MainMenuButton").GetComponent<Button_UI> ().ClickFunc = () => {
			Loader.Load (Loader.Scene.MainMenu);
		};
		transform.Find ("MainMenuButton").GetComponent<Button_UI> ().AddButtonSound ();

		Hide ();
	}

	private void Show ()
	{
		gameObject.SetActive (true);
	}

	private void Hide ()
	{
		gameObject.SetActive (false);
	}

	public static void ShowStatic ()
	{
		instance.Show ();
	}

	public static void HideStatic ()
	{
		instance.Hide ();
	}
}
