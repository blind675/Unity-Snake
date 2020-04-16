using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader {

	public enum Scene {
		GameScene,
		Loading,
		MainMenu
	}

	private static Action loaderCallbackAction;

	public static void Load (Scene scene)
	{
		SceneManager.LoadScene (Scene.Loading.ToString ());
		loaderCallbackAction = () => {
			SceneManager.LoadScene (scene.ToString ());
		};
	}

	public static void LoaderCallback ()
	{
		loaderCallbackAction?.Invoke ();
		loaderCallbackAction = null;
	}
}
