using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class Snake : MonoBehaviour {

	private enum Direction {
		Left,
		Right,
		Up,
		Down
	}

	private enum State {
		Alive,
		Dead,
	}

	private Direction gridMoveDirection;
	private Vector2Int gridPosition;

	private float gridMoveTimer;
	private float gridMoveTimerMax;

	private LevelGrid levelGrid;
	private State state;

	private int snakeBodySize;
	private List<SnakeMovePosition> snakeMovePositionList;
	private List<SnakeBodyParts> snakeBodyPartsList;

	public void Setup (LevelGrid levelGrid)
	{
		this.levelGrid = levelGrid;
	}

	private void Awake ()
	{
		gridPosition = new Vector2Int (10, 10);
		gridMoveTimerMax = gridMoveTimer = .5f;
		gridMoveDirection = Direction.Up;
		state = State.Alive;

		snakeMovePositionList = new List<SnakeMovePosition> ();
		snakeBodySize = 0;

		snakeBodyPartsList = new List<SnakeBodyParts> ();
	}

	private void Update ()
	{
		switch (state) {
		case State.Alive:
			HandleInput ();
			HandleGridMovement ();
			break;
		case State.Dead:
			break;
		}

	}

	private void HandleInput ()
	{

		if (Input.GetKeyDown (KeyCode.UpArrow) && gridMoveDirection != Direction.Down) {
			gridMoveDirection = Direction.Up;
		}

		if (Input.GetKeyDown (KeyCode.DownArrow) && gridMoveDirection != Direction.Up) {
			gridMoveDirection = Direction.Down;
		}

		if (Input.GetKeyDown (KeyCode.RightArrow) && gridMoveDirection != Direction.Left) {
			gridMoveDirection = Direction.Right;
		}

		if (Input.GetKeyDown (KeyCode.LeftArrow) && gridMoveDirection != Direction.Right) {
			gridMoveDirection = Direction.Left;
		}

	}

	private void HandleGridMovement ()
	{

		gridMoveTimer += Time.deltaTime;
		if (gridMoveTimer >= gridMoveTimerMax) {
			gridMoveTimer -= gridMoveTimerMax;

			SoundManager.PlaySound (SoundManager.Sound.SnakeMove);


			SnakeMovePosition prevoiusSnakeMovePosition = null;
			if (snakeMovePositionList.Count > 0) {
				prevoiusSnakeMovePosition = snakeMovePositionList [0];
			}

			SnakeMovePosition snakeMovePosition = new SnakeMovePosition (prevoiusSnakeMovePosition, gridPosition, gridMoveDirection);
			snakeMovePositionList.Insert (0, snakeMovePosition);

			Vector2Int gridMoveDirectionVector;
			switch (gridMoveDirection) {
			default:
			case Direction.Up: gridMoveDirectionVector = new Vector2Int (0, 1); break;
			case Direction.Right: gridMoveDirectionVector = new Vector2Int (1, 0); break;
			case Direction.Down: gridMoveDirectionVector = new Vector2Int (0, -1); break;
			case Direction.Left: gridMoveDirectionVector = new Vector2Int (-1, 0); break;
			}

			gridPosition += gridMoveDirectionVector;
			gridPosition = levelGrid.ValidateGridPosition (gridPosition);

			bool snakeAteFood = levelGrid.TrySnakeEatFood (gridPosition);
			if (snakeAteFood) {
				snakeBodySize++;
				CreateSnakeBodyPart ();
				gridMoveTimerMax -= .01f;
				SoundManager.PlaySound (SoundManager.Sound.SnakeEat);
				Score.AddScore ();
			};

			if (snakeMovePositionList.Count >= snakeBodySize + 1) {
				snakeMovePositionList.RemoveAt (snakeMovePositionList.Count - 1);
			}

			foreach (SnakeBodyParts snakeBodyPart in snakeBodyPartsList) {
				Vector2Int snakeBodyPartGridPosition = snakeBodyPart.GetGridPosition ();
				if (gridPosition == snakeBodyPartGridPosition) {
					// Game Ever
					CMDebug.TextPopup ("DEAD!", transform.position);
					state = State.Dead;
					GameHandler.SnakeDied ();
					SoundManager.PlaySound (SoundManager.Sound.SnakeDie);
				}
			}

			transform.position = new Vector3 (gridPosition.x + .5f, gridPosition.y + .5f);
			transform.eulerAngles = new Vector3 (0, 0, GetAngleFromVector (gridMoveDirectionVector) - 90);

			UpdateSnakeBodyParts ();
		}
	}

	private void CreateSnakeBodyPart ()
	{
		snakeBodyPartsList.Add (new SnakeBodyParts (snakeBodyPartsList.Count));
	}

	private void UpdateSnakeBodyParts ()
	{
		for (int i = 0; i < snakeBodyPartsList.Count; i++) {
			snakeBodyPartsList [i].SetSnakeMovePostion (snakeMovePositionList [i]);
		}
	}

	private float GetAngleFromVector (Vector2Int dir)
	{
		float n = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
		if (n < 0) n += 360;
		return n;
	}

	public Vector2Int GetGridPosition ()
	{
		return gridPosition;
	}

	public List<Vector2Int> GetFullSnakeGridPositionList ()
	{
		List<Vector2Int> gridPositionList = new List<Vector2Int> () { gridPosition };
		foreach (SnakeMovePosition snakeMovePosition in snakeMovePositionList) {
			gridPositionList.Add (snakeMovePosition.GetGridPosition ());
		}

		return gridPositionList;
	}


	/*
	 * Handle a Single Snake Body Part
	 */
	private class SnakeBodyParts {

		private SnakeMovePosition snakeMovePosition;
		private Transform transform;

		public SnakeBodyParts (int bodyIndex)
		{
			GameObject snakeBodyGameObject = new GameObject ("SnakeBody", typeof (SpriteRenderer));
			snakeBodyGameObject.GetComponent<SpriteRenderer> ().sprite = GameAssets.instance.snakeBodySprite;
			transform = snakeBodyGameObject.transform;
			snakeBodyGameObject.GetComponent<SpriteRenderer> ().sortingOrder = -bodyIndex;
		}

		public Vector2Int GetGridPosition ()
		{
			if (snakeMovePosition != null) {
				return snakeMovePosition.GetGridPosition ();
			}

			return new Vector2Int (0, 0);
		}

		public void SetSnakeMovePostion (SnakeMovePosition snakeMovePosition)
		{
			this.snakeMovePosition = snakeMovePosition;
			transform.position = new Vector3 (snakeMovePosition.GetGridPosition ().x + 0.5f, snakeMovePosition.GetGridPosition ().y + 0.5f);

			float angle;
			switch (snakeMovePosition.GetDirection ()) {

			default:
			case Direction.Up:
				switch (snakeMovePosition.GetPreviousDirection ()) {
				default:
					angle = 0;
					break;
				case Direction.Left:
					angle = 45;
					break;
				case Direction.Right:
					angle = -45;
					break;
				}
				break;
			case Direction.Right:
				switch (snakeMovePosition.GetPreviousDirection ()) {
				default:
					angle = 90;
					break;
				case Direction.Down:
					angle = 45 - 180;
					break;
				case Direction.Up:
					angle = -45 + 180;
					break;
				}
				break;
			case Direction.Down:
				switch (snakeMovePosition.GetPreviousDirection ()) {
				default:
					angle = 180;
					break;
				case Direction.Left:
					angle = 180 - 45;
					break;
				case Direction.Right:
					angle = 180 + 45;
					break;
				}
				break;
			case Direction.Left:
				switch (snakeMovePosition.GetPreviousDirection ()) {
				default:
					angle = -90;
					break;
				case Direction.Down:
					angle = -45;
					break;
				case Direction.Up:
					angle = 45;
					break;
				}
				break;
			}
			transform.eulerAngles = new Vector3 (0, 0, angle);
		}
	}


	/*
	 * Handles one Move Position from the Snake 
	 * */
	private class SnakeMovePosition {

		private SnakeMovePosition previousSnakeMovePosition;
		private Vector2Int gridPosition;
		private Direction direction;

		public SnakeMovePosition (SnakeMovePosition previousSnakeMovePosition, Vector2Int gridPosition, Direction direction)
		{
			this.previousSnakeMovePosition = previousSnakeMovePosition;
			this.gridPosition = gridPosition;
			this.direction = direction;
		}

		public Vector2Int GetGridPosition ()
		{
			return gridPosition;
		}

		public Direction GetDirection ()
		{
			return direction;
		}

		public Direction GetPreviousDirection ()
		{
			if (previousSnakeMovePosition == null) {
				return Direction.Up;
			}

			return previousSnakeMovePosition.direction;
		}

	}
}
