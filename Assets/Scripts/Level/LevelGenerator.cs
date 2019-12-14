/* author: Brian Tria
 * created: Dec 13, 2019
 * description: Using Offset Coordinates for layout. see: https://www.redblobgames.com/grids/hexagons/ 
 *				We're using Odd-Offset row coordinates.
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
	[Header("References")]
	[SerializeField] private GameObject bubblePrefab;
	[SerializeField] private IntVariable bubbleBulletType;
	[SerializeField] private VectorVariable bubbleSize;
	[SerializeField] private VectorVariable levelMapDimension;
	[SerializeField] private IntVariableList levelDataMap;
	[SerializeField] private GameObjectList activeBubbleObjectList;
	[SerializeField] private GameObjectList inactiveBubbleObjectList;
#pragma warning restore 0649

	private LevelInfo levelInfo;
	#endregion

	void Awake()
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

		if (activeBubbleObjectList == null)
		{
			Debug.LogError("Missing reference to active bubble object list.");
			return true;
		}

		if (inactiveBubbleObjectList == null)
		{
			Debug.LogError("Missing reference to inactive bubble object list.");
			return true;
		}

		if (levelDataMap == null)
		{
			Debug.LogError("Missing reference to level data map.");
			return true;
		}

		if (levelMapDimension == null)
		{
			Debug.LogError("Missing reference to level map dimension.");
			return true;
		}

		return false;
	}

	void LoadLevel()
	{
		levelInfo = LevelInfo.CreateFromJsonFileForLevel(1);
		levelMapDimension.RuntimeValue = new Vector3(levelInfo.columnCount, levelInfo.rowCount, 0);
		activeBubbleObjectList.Contents = new List<GameObject>();
		inactiveBubbleObjectList.Contents = new List<GameObject>();
		levelDataMap.RuntimeContents = new List<int>();
	}

	void InstantiateBubblePool()
	{
		for (int idx = 0; idx < TOTAL_BUBBLE_TARGET; ++idx)
		{
			GameObject bubble = Instantiate(bubblePrefab, transform.position, Quaternion.identity, transform);
			bubble.SetActive(false);
			inactiveBubbleObjectList.Contents.Add(bubble);
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

		// starting from bottom row (closest to player)
		for (int rowIdx = 0; rowIdx < rowCount; ++rowIdx)
		{
			List<int> columns = rows[rowIdx].columns;
			int columnCount = columns.Count;

			float offsetX = -(columnCount / 2) * bubbleSize.RuntimeValue.x;
			float offsetY = rowIdx * bubbleSize.RuntimeValue.y;

			// offset odd-indexed rows 
			if (rowIdx % 2 > 0)
			{
				offsetX += bubbleSize.RuntimeValue.x * 0.5f;
			}

			Vector3 bubblePosition = new Vector3(offsetX, offsetY, 0);

			// from left to right...
			for (int columnIdx = 0; columnIdx < columnCount; ++columnIdx)
			{
				int levelTileValue = columns[columnIdx];
				levelDataMap.RuntimeContents.Add(levelTileValue);

				if (levelTileValue == 0)
				{
					// skip position; keeping a blank space
					continue;
				}

				GameObject bubbleObject = inactiveBubbleObjectList.Contents[0];
				activeBubbleObjectList.Contents.Add(bubbleObject);
				inactiveBubbleObjectList.Contents.RemoveAt(0);

				bubblePosition.x = offsetX + (columnIdx * bubbleSize.RuntimeValue.x);
				bubbleObject.transform.localPosition = bubblePosition;

				Bubble bubble = bubbleObject.GetComponent<Bubble>();
				if (bubble == null)
				{
					Debug.LogError("Missing bubble component.");
					continue;
				}

				bubble.Type = (BubbleType)levelTileValue;
				bubble.Coordinates = new Vector2(columnIdx, rowIdx);
				bubbleObject.SetActive(true);
			}
		}
	}
	#endregion

	#region Public Methods


	#endregion
}
