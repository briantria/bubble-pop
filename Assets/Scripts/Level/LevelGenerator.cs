/* author: Brian Tria
 * created: Dec 13, 2019
 * description: Using Offset Coordinates for layout. see: https://www.redblobgames.com/grids/hexagons/
 */

using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
	#region Constants
	const int TOTAL_BUBBLE_TARGET = 200;
	#endregion

	#region Member Variables
	// known issue for SerializedField throwing warnings
	// link: https://forum.unity.com/threads/serializefield-warnings.560878/
#pragma warning disable 0649
	[Header("Game Perimeter")]
	[SerializeField] private VectorVariable bottomLeftPerimeterPoint;
	[SerializeField] private VectorVariable topRightPerimeterPoint;

	[Header("References")]
	[SerializeField] private IntVariable bubbleBulletType;
	[SerializeField] private GameObject bubblePrefab;
	[SerializeField] private VectorVariable bubbleSize;

	[Header("Game Events")]
	[SerializeField] private GameEvent onBulletReload;
#pragma warning restore 0649

	private LevelInfo levelInfo;
	private List<GameObject> activeBubbleTargets = new List<GameObject>();
	private List<GameObject> inactiveBubbleTargets = new List<GameObject>();
	#endregion

	void Start()
	{
		if (HasMissingReference())
		{
			return;
		}

		LoadLevel();
		InstantiateBubblePool();
		SetupLevel();
	}

	#region Private Methods
	bool HasMissingReference()
	{
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

		if (bubblePrefab == null)
		{
			Debug.LogError("Missing reference to bubble prefab.");
			return true;
		}

		if (bubbleSize == null)
		{
			Debug.LogError("Missing reference to bubble size.");
			return true;
		}

		if (bubbleBulletType == null)
		{
			Debug.LogError("Missing reference to bubble bullet type.");
			return true;
		}

		if (onBulletReload == null)
		{
			Debug.LogError("Missing reference to bullet reload.");
			return true;
		}

		return false;
	}

	void LoadLevel()
	{
		levelInfo = LevelInfo.CreateFromJsonFileForLevel(1);
	}

	void InstantiateBubblePool()
	{
		for (int idx = 0; idx < TOTAL_BUBBLE_TARGET; ++idx)
		{
			GameObject bubble = Instantiate(bubblePrefab, transform.position, Quaternion.identity, transform);
			bubble.SetActive(false);
			inactiveBubbleTargets.Add(bubble);
		}
	}

	void SetupLevel()
	{
		if (levelInfo == null)
		{
			Debug.LogError("Unable to generate level info.");
			return;
		}

		/*	tags:
		 * 		- 0 -> space
		 * 		- 1, 2, 3, ... n -> bubble with given type (color, powerup, etc...)
		 */

		List<Row> rows = levelInfo.rows;
		int rowCount = rows.Count;
		int lastRowIdx = rowCount - 1;

		// starting from bottom row (closest to player)
		for (int rowIdx = lastRowIdx; rowIdx >= 0; --rowIdx)
		{
			List<int> columns = rows[rowIdx].columns;
			int columnCount = columns.Count;

			float offsetX = -(columnCount / 2) * bubbleSize.RuntimeValue.x;
			float offsetY = (lastRowIdx - rowIdx) * bubbleSize.RuntimeValue.y;

			// offset odd-indexed rows 
			if (rowIdx % 2 > 0)
			{
				offsetX += bubbleSize.RuntimeValue.x * 0.5f;
			}

			Vector3 bubblePosition = new Vector3(offsetX, offsetY, 0);

			// from left to right...
			for (int columnIdx = 0; columnIdx < columnCount; ++columnIdx)
			{
				if (columns[columnIdx] == 0)
				{
					// skip position; keeping a blank space
					continue;
				}

				GameObject bubbleObject = inactiveBubbleTargets[0];
				activeBubbleTargets.Add(bubbleObject);
				inactiveBubbleTargets.RemoveAt(0);

				bubblePosition.x = offsetX + (columnIdx * bubbleSize.RuntimeValue.x);
				bubbleObject.transform.localPosition = bubblePosition;

				Bubble bubble = bubbleObject.GetComponent<Bubble>();
				if (bubble == null)
				{
					Debug.LogError("Missing bubble component.");
					continue;
				}

				bubble.Type = (BubbleType)columns[columnIdx];
				bubble.Coordinates = new Vector2(columnIdx, rowIdx);
				bubbleObject.SetActive(true);
			}
		}

		ChooseNewBulletType();
	}
	#endregion

	#region Public Methods
	public void PopHitBubbles()
	{
		// TODO: check matches
		// TODO: recursive call to pop neighboring bubbles

		ChooseNewBulletType();
	}

	public void ChooseNewBulletType()
	{
		List<BubbleType> activeBubbleTypes = new List<BubbleType>();

		foreach (GameObject bubbleObject in activeBubbleTargets)
		{
			Bubble bubble = bubbleObject.GetComponent<Bubble>();

			if (bubble == null)
			{
				Debug.LogError("Missing bubble component.");
				continue;
			}

			BubbleType bubbleType = bubble.Type;
			if (!activeBubbleTypes.Contains(bubbleType))
			{
				activeBubbleTypes.Add(bubbleType);
			}
		}

		int randomIdx = Random.Range(0, activeBubbleTypes.Count);
		bubbleBulletType.RuntimeValue = (int)activeBubbleTypes[randomIdx];

		if (onBulletReload != null)
		{
			onBulletReload.Raise();
		}
	}
	#endregion

	// TODO: On bubble target hit matched, clear up connected matches
	// TODO: update active bubble types
}
