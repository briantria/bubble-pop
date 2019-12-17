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

	[Header("Prefabs")]
	[SerializeField] private GameObject bubblePrefab;

	[Header("Game Events")]
	[SerializeField] private GameEvent onBulletReload;
#pragma warning restore 0649

	private List<GameObject> ragdollBubbleList = new List<GameObject>();

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

	#region LifeCycle
	void Start()
	{
		if (HasMissingReference())
		{
			return;
		}

		int ragdollCount = (int)(levelMapDimension.RuntimeValue.x * levelMapDimension.RuntimeValue.y);
		for (int idx = 0; idx < ragdollCount; ++idx)
		{
			GameObject ragdollBubble = Instantiate(bubblePrefab, transform.position, Quaternion.identity, transform);
			ragdollBubble.name = "Ragdoll Bubble";
			ragdollBubble.SetActive(false);
			ragdollBubbleList.Add(ragdollBubble);
		}
	}

	void Update()
	{
		if (HasMissingReference())
		{
			return;
		}

		foreach (GameObject ragdollBubble in ragdollBubbleList)
		{
			if (ragdollBubble.transform.position.y < -7)
			{
				ragdollBubble.transform.position = Vector3.zero;
				ragdollBubble.SetActive(false);
			}
		}
	}
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

		if (bubblePrefab == null)
		{
			Debug.LogError("Missing bubble prefab.");
			return true;
		}

		return false;
	}

	private List<GameObject> GetNeighbors(Vector2 coordinates, NeighborType neighborType)
	{
		List<GameObject> neighbors = new List<GameObject>();

		int rowCount = (int)levelMapDimension.RuntimeValue.y;
		int columnCount = (int)levelMapDimension.RuntimeValue.x;
		int rowOffsetType = (int)coordinates.y % 2;
		int pairedOffsetCount = neighborOffsetArray.GetLength(1);

		for (int offsetIdx = 0; offsetIdx < pairedOffsetCount; ++offsetIdx)
		{
			int offsetX = (int)(coordinates.x + neighborOffsetArray[rowOffsetType, offsetIdx, 0]);
			int offsetY = (int)(coordinates.y + neighborOffsetArray[rowOffsetType, offsetIdx, 1]);

			if (offsetX < 0 || offsetX >= columnCount ||
				offsetY < 0 || offsetY >= rowCount)
			{
				continue;
			}

			int idx = (offsetY * columnCount) + offsetX;

			if (idx < 0 || idx >= bubbleTargetList.Contents.Count)
			{
				continue;
			}

			GameObject bubbleObject = bubbleTargetList.Contents[idx];

			switch (neighborType)
			{
				case NeighborType.Inactive:
					{
						if (!bubbleObject.activeInHierarchy)
						{
							neighbors.Add(bubbleObject);
						}
						continue;
					}
				case NeighborType.Active:
					{
						if (bubbleObject.activeInHierarchy)
						{
							neighbors.Add(bubbleObject);
						}
						continue;
					}
				default:
					{
						neighbors.Add(bubbleObject);
						continue;
					}
			}
		}

		return neighbors;
	}

	private Bubble AttachBubbleBullet()
	{
		List<GameObject> inactiveNeighbors = GetNeighbors(bubbleHitCoordinates.RuntimeValue, NeighborType.Inactive);
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

		bubble.Type = (BubbleType)bubbleBulletType.RuntimeValue;
		bubbleObject.SetActive(true);

		return bubble;
	}

	// reference: http://rembound.com/articles/bubble-shooter-game-tutorial-with-html5-and-javascript
	private List<GameObject> GetPopList(Bubble bubble, bool ignoreMatchType)
	{
		List<GameObject> popList = new List<GameObject>();
		List<GameObject> pendingBubbleList = new List<GameObject>();
		List<GameObject> checkedList = new List<GameObject>();

		pendingBubbleList.Add(bubble.gameObject);
		checkedList.Add(bubble.gameObject);

		while (pendingBubbleList.Count > 0)
		{
			GameObject currentBubbleObject = pendingBubbleList[0];
			Bubble currentBubble = currentBubbleObject.GetComponent<Bubble>();
			pendingBubbleList.RemoveAt(0);

			if (currentBubble == null)
			{
				Debug.LogError("Missing bubble component.");
				break;
			}

			if (ignoreMatchType || currentBubble.Type == bubble.Type)
			{
				popList.Add(currentBubbleObject);
				List<GameObject> activeNeighbors = GetNeighbors(currentBubble.Coordinates, NeighborType.Active);

				foreach (GameObject neighbor in activeNeighbors)
				{
					if (!checkedList.Contains(neighbor))
					{
						pendingBubbleList.Add(neighbor);
						checkedList.Add(neighbor);
					}
				}
			}
		}

		return popList;
	}

	private List<GameObject> GetFloatingPopList()
	{
		List<GameObject> floatingPopList = new List<GameObject>();
		List<GameObject> checkedList = new List<GameObject>();

		foreach (GameObject bubbleObject in bubbleTargetList.Contents)
		{
			if (checkedList.Contains(bubbleObject))
			{
				continue;
			}

			Bubble bubble = bubbleObject.GetComponent<Bubble>();

			if (bubble == null)
			{
				Debug.LogError("Missing bubble component");
				break;
			}

			List<GameObject> popList = GetPopList(bubble, true);
			checkedList.AddRange(popList);

			if (popList.Count <= 0)
			{
				continue;
			}

			bool isFloating = true;

			foreach (GameObject bubbleInCheck in popList)
			{
				bubble = bubbleInCheck.GetComponent<Bubble>();
				if (bubble.Coordinates.y == levelMapDimension.RuntimeValue.y - 1)
				{
					isFloating = false;
					break;
				}
			}

			if (isFloating)
			{
				floatingPopList.AddRange(popList);
			}
		}

		return floatingPopList;
	}

	private void PopBubbles(List<GameObject> popList)
	{
		if (popList.Count < minimumMatchCount.InitValue)
		{
			return;
		}

		foreach (GameObject matchedBubbleObject in popList)
		{
			Bubble matchedBubble = matchedBubbleObject.GetComponent<Bubble>();
			if (matchedBubbleObject == null)
			{
				Debug.LogError("Missing bubble component.");
				break;
			}

			ActivateRagdollBubble(matchedBubbleObject);

			matchedBubble.Type = BubbleType.None;
			matchedBubbleObject.SetActive(false);
		}
	}

	private void ActivateRagdollBubble(GameObject bubbleObject)
	{
		if (ragdollBubbleList.Count == 0)
		{
			Debug.Log("rigid body list not set");
			return;
		}

		Bubble bubble = bubbleObject.GetComponent<Bubble>();
		if (bubble == null)
		{
			return;
		}

		Vector2 coordinates = bubble.Coordinates;
		int ragdollIdx = (int)((coordinates.y * levelMapDimension.RuntimeValue.x) + coordinates.x);
		if (ragdollIdx < 0 || ragdollIdx >= ragdollBubbleList.Count)
		{
			return;
		}

		Rigidbody2D bubbleRigidBody = ragdollBubbleList[ragdollIdx].GetComponent<Rigidbody2D>();
		if (bubbleRigidBody == null)
		{
			return;
		}

		Bubble ragdollBubble = ragdollBubbleList[ragdollIdx].GetComponent<Bubble>();
		if (ragdollBubble == null)
		{
			return;
		}

		ragdollBubble.Type = bubble.Type;

		Vector3 ragdollPosition = bubbleObject.transform.position;
		ragdollPosition.z -= 2;
		ragdollBubbleList[ragdollIdx].transform.position = ragdollPosition;

		ragdollBubbleList[ragdollIdx].SetActive(true);

		Vector2 forceDirection = Vector2.zero;
		forceDirection.x = Random.Range(-1.5f, 1.5f);
		forceDirection.y = Random.Range(1.5f, 1.8f);
		bubbleRigidBody.AddForce(forceDirection, ForceMode2D.Impulse);
	}
	#endregion

	public void PopMatchedBubbles()
	{
		if (HasMissingReference())
		{
			return;
		}

		Bubble bubble = AttachBubbleBullet();

		if (bubble == null)
		{
			if (onBulletReload != null)
			{
				onBulletReload.Raise();
			}

			return;
		}

		PopBubbles(GetPopList(bubble, false));
		PopBubbles(GetFloatingPopList());

		// TODO: animations

		if (onBulletReload != null)
		{
			onBulletReload.Raise();
		}

	}
}
