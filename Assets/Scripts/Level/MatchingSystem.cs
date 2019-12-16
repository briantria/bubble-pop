/* author: Brian Tria
 * created: Dec 14, 2019
 * description: Using Offset Coordinates for layout. see: https://www.redblobgames.com/grids/hexagons/
 *				We're using Odd-Offset row coordinates.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchingSystem : MonoBehaviour
{
	#region Member Variables
	// known issue for SerializedField throwing warnings
	// link: https://forum.unity.com/threads/serializefield-warnings.560878/
#pragma warning disable 0649
	[Header("References")]
	[SerializeField] private IntVariable bubbleBulletType;
	[SerializeField] private IntVariable minimumMatchCount;
	[Space]
	[SerializeField] private VectorVariable bubbleSize;
	[SerializeField] private VectorVariable levelMapDimension;
	[SerializeField] private VectorVariable bulletHitPosition;
	[SerializeField] private VectorVariable bubbleHitCoordinates;
	[Space]
	[SerializeField] private GameObjectList bubbleTargetList;

	[Header("Game Events")]
	[SerializeField] private GameEvent onBulletReload;
#pragma warning restore 0649

	int[,,] neighborOffsetArray = new int[,,]
	{
		{
			{1, 0}, {0, -1}, {-1, -1},
			{-1, 0}, {-1, 1}, {0, 1}
		},
		{
			{1, 0}, {1, -1}, {0, -1},
			{-1, 0}, {0, 1}, {1, 1}
		}
	};
	#endregion

	#region Private Methods
	bool HasMissingReference()
	{
		if (bubbleBulletType == null)
		{
			Debug.LogError("Missing reference to bubble bullet type.");
			return true;
		}

		if (minimumMatchCount == null)
		{
			Debug.LogError("Missing reference to minimum match count.");
			return true;
		}

		if (bulletHitPosition == null)
		{
			Debug.LogError("Missing reference to bullet hit position.");
			return true;
		}

		if (bubbleSize == null)
		{
			Debug.LogError("Missing reference to bubble size.");
			return true;
		}

		if (bubbleHitCoordinates == null)
		{
			Debug.LogError("Missing reference to bullet hit coordinates.");
			return true;
		}

		if (bubbleTargetList == null)
		{
			Debug.LogError("Missing reference to active bubble object list.");
			return true;
		}

		if (levelMapDimension == null)
		{
			Debug.LogError("Missing reference to level map dimension.");
			return true;
		}

		return false;
	}

	private List<GameObject> getInactiveNeighbors()
	{
		List<GameObject> inactiveNeighbors = new List<GameObject>();
		Vector2 hitCoordinates = bubbleHitCoordinates.RuntimeValue;

		Debug.Log("hit coord: " + hitCoordinates);

		int columnCount = (int)levelMapDimension.RuntimeValue.x;
		int rowOffsetType = (int)hitCoordinates.y % 2;
		int pairedOffsetCount = neighborOffsetArray.GetLength(1);

		for (int offsetIdx = 0; offsetIdx < pairedOffsetCount; ++offsetIdx)
		{
			int offsetX = (int)(hitCoordinates.x + neighborOffsetArray[rowOffsetType, offsetIdx, 0]);
			int offsetY = (int)(hitCoordinates.y + neighborOffsetArray[rowOffsetType, offsetIdx, 1]);
			int idx = (offsetY * columnCount) + offsetX;

			if (idx < 0 || idx >= bubbleTargetList.Contents.Count)
			{
				continue;
			}

			GameObject bubbleObject = bubbleTargetList.Contents[idx];

			if (!bubbleObject.activeInHierarchy)
			{
				inactiveNeighbors.Add(bubbleObject);
			}
		}

		return inactiveNeighbors;
	}

	private Bubble attachBubbleBullet()
	{
		List<GameObject> inactiveNeighbors = getInactiveNeighbors();
		GameObject bubbleObject = null;

		foreach (GameObject neighbor in inactiveNeighbors)
		{
			float distance = Vector3.Distance(neighbor.transform.position, bulletHitPosition.RuntimeValue);
			if (distance < bubbleSize.RuntimeValue.x)
			{
				bubbleObject = neighbor;
				break;
			}
		}

		if (bubbleObject == null)
		{
			return null;
		}

		Bubble bubble = bubbleObject.GetComponent<Bubble>();

		if (bubble == null)
		{
			Debug.LogError("Missing bubble component");
			return null;
		}

		Debug.Log("<b>hit pos</b>: " + bulletHitPosition.RuntimeValue);
		Debug.Log("pos: " + bubbleObject.transform.position);
		Debug.Log("coord: " + bubble.Coordinates);

		bubble.Type = (BubbleType)bubbleBulletType.RuntimeValue;
		bubbleObject.SetActive(true);

		return bubble;
	}
	#endregion

	public void PopMatchedBubbles()
	{
		if (HasMissingReference())
		{
			return;
		}

		// (1) find bullet attach point
		Bubble bubble = attachBubbleBullet();

		if (onBulletReload != null)
		{
			onBulletReload.Raise();
		}

		if (bubble == null)
		{
			return;
		}

		// TODO: (2) check for matching bubbles; bullet as root node


		// TODO: (3) pop if match count is valid
	}
}
