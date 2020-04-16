using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class MainMenuWindow : MonoBehaviour {
	private void Awake ()
	{
		transform.Find ("PlayButton").GetComponent<Button_UI> ().ClickFunc = () => {
			Loader.Load (Loader.Scene.GameScene);
		};
		transform.Find ("PlayButton").GetComponent<Button_UI> ().AddButtonSound ();

		transform.Find ("ExitButton").GetComponent<Button_UI> ().ClickFunc = () => {
			Application.Quit ();
		};
		transform.Find ("ExitButton").GetComponent<Button_UI> ().AddButtonSound ();

	}
}
