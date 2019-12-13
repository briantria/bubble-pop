/* author: Brian Tria
 * created: Dec 12, 2019
 * description: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetup : MonoBehaviour
{
	#region Member Variables
	// known issue for SerializedField throwing warnings
	// link: https://forum.unity.com/threads/serializefield-warnings.560878/
#pragma warning disable 0649
	[SerializeField] private VectorVariable bubbleSize;

	[Header("References")]
	[SerializeField] private SpriteRenderer bubbleSpriteRenderer;

	[Header("Game Perimeter")]
	[SerializeField] private VectorVariable bottomLeftPerimeterPoint;
	[SerializeField] private VectorVariable topRightPerimeterPoint;
#pragma warning restore 0649
	#endregion

	void Awake()
	{
		if (HasMissingReference())
		{
			return;
		}

		InstantiateGameBounds();
		InstantiateBubbleSize();
	}

	void InstantiateGameBounds()
	{
		Vector3 bottomLeftScreenCoordinates = new Vector3(0, 0, 0);
		Vector3 topRightScreenCoordinates = new Vector3(Screen.width, Screen.height, 0);

		bottomLeftPerimeterPoint.RuntimeValue = Camera.main.ScreenToWorldPoint(bottomLeftScreenCoordinates);
		topRightPerimeterPoint.RuntimeValue = Camera.main.ScreenToWorldPoint(topRightScreenCoordinates);
	}

	void InstantiateBubbleSize()
	{
		bubbleSize.RuntimeValue = bubbleSpriteRenderer.sprite.bounds.size;
	}

	bool HasMissingReference()
	{
		if (bubbleSize == null)
		{
			Debug.LogError("Missing reference to bubble size.");
			return true;
		}

		if (bubbleSpriteRenderer == null)
		{
			Debug.LogError("Missing reference to bubble sprite renderer.");
			return true;
		}

		if (bottomLeftPerimeterPoint == null)
		{
			Debug.LogError("Missing reference to bottom left perimeter point.");
			return true;
		}

		if (topRightPerimeterPoint == null)
		{
			Debug.LogError("Missing reference to top right perimeter point.");
			return true;
		}

		return false;
	}
}
