using UnityEngine;

public class LevelGrid {

	private Snake snake;
	private Vector2Int foodGridPosition;
	private GameObject foodGameObject;
	private int height;
	private int width;

	public LevelGrid (int width, int height)
	{
		this.width = width;
		this.height = height;
	}

	public void Setup (Snake snake)
	{
		this.snake = snake;
		SpawnFood ();
	}

	private void SpawnFood ()
	{
		do {
			foodGridPosition = new Vector2Int (Random.Range (1, width - 1), Random.Range (1, height - 1));
		} while (snake.GetFullSnakeGridPositionList ().IndexOf (foodGridPosition) != -1);

		foodGameObject = new GameObject ("Food", typeof (SpriteRenderer));
		foodGameObject.GetComponent<SpriteRenderer> ().sprite = GameAssets.instance.foodSprite;
		foodGameObject.transform.position = new Vector3 (foodGridPosition.x + .5f, foodGridPosition.y + .5f);
	}

	public bool TrySnakeEatFood (Vector2Int snakeGridPosition)
	{
		if (snakeGridPosition == foodGridPosition) {
			Object.Destroy (foodGameObject);
			SpawnFood ();

			return true;
		}

		return false;
	}

	public Vector2Int ValidateGridPosition (Vector2Int gridPosition)
	{
		if (gridPosition.x < 0) {
			gridPosition.x = width - 1;
		}
		if (gridPosition.x > width - 1) {
			gridPosition.x = 0;
		}
		if (gridPosition.y < 0) {
			gridPosition.y = height - 1;
		}
		if (gridPosition.y > height - 1) {
			gridPosition.y = 0;
		}

		return gridPosition;
	}
}
