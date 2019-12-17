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
	#region Member Variables
	// known issue for SerializedField throwing warnings
	// link: https://forum.unity.com/threads/serializefield-warnings.560878/
#pragma warning disable 0649
	[Header("References")]
	[SerializeField] private GameObject bubblePrefab;
	[Space]
	[SerializeField] private IntVariable bubbleBulletType;
	[SerializeField] private IntVariable currentLevel;
	[Space]
	[SerializeField] private VectorVariable bubbleSize;
	[SerializeField] private VectorVariable levelMapDimension;
	[Space]
	[SerializeField] private GameObjectList bubbleTargetList;
#pragma warning restore 0649

	private LevelInfo levelInfo;
	#endregion

	#region LifeCycle
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

	void OnDestroy()
	{
		if (bubbleTargetList != null)
		{
			bubbleTargetList.Contents = null;
		}
	}
	#endregion

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

		if (bubbleTargetList == null)
		{
			Debug.LogError("Missing reference to bubble target object list.");
			return true;
		}

		if (levelMapDimension == null)
		{
			Debug.LogError("Missing reference to level map dimension.");
			return true;
		}

		if (currentLevel == null)
		{
			Debug.LogError("Missing reference to current level.");
			return true;
		}

		return false;
	}

	void LoadLevel()
	{
		levelInfo = LevelInfo.CreateFromJsonFileForLevel(currentLevel.InitValue);
		levelMapDimension.RuntimeValue = new Vector3(levelInfo.ColumnCount, levelInfo.RowCount, 0);
		bubbleTargetList.Contents = new List<GameObject>();
	}

	void InstantiateBubblePool()
	{
		int maxBubbleCount = levelInfo.ColumnCount * levelInfo.RowCount;
		for (int idx = 0; idx < maxBubbleCount; ++idx)
		{
			GameObject bubble = Instantiate(bubblePrefab, transform.position, Quaternion.identity, transform);
			bubble.SetActive(false);
			bubbleTargetList.Contents.Add(bubble);
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

		List<Row> rows = levelInfo.Rows;
		int rowCount = levelInfo.RowCount;
		int columnCount = levelInfo.ColumnCount;

		// starting from bottom row (closest to player)
		for (int rowIdx = 0; rowIdx < rowCount; ++rowIdx)
		{
			List<int> columns = rows[rowIdx].Columns;

			float offsetX = bubbleSize.RuntimeValue.x * (-columnCount / 2);
			float offsetY = bubbleSize.RuntimeValue.y * rowIdx;

			// offset odd-indexed rows 
			if (rowIdx % 2 > 0)
			{
				offsetX += bubbleSize.RuntimeValue.x * 0.5f;
			}

			// from left to right...
			for (int columnIdx = 0; columnIdx < columnCount; ++columnIdx)
			{
				int bubbleTypeIntValue = columns[columnIdx];
				int bubbleTargetIdx = (rowIdx * columnCount) + columnIdx;

				if (bubbleTargetIdx < 0 || bubbleTargetIdx >= bubbleTargetList.Contents.Count)
				{
					Debug.LogError("Invalid bubble target index: " + bubbleTargetIdx +
					" (" + columnIdx + ", " + rowIdx + ")");
					break;
				}

				GameObject bubbleObject = bubbleTargetList.Contents[bubbleTargetIdx];
				Bubble bubble = bubbleObject.GetComponent<Bubble>();

				if (bubble == null)
				{
					Debug.LogError("Missing bubble component. " + "(" + columnIdx + "," + rowIdx + ")");
					break;
				}

				Vector3 bubblePosition = new Vector3(offsetX, offsetY, 0);
				bubblePosition.x = offsetX + (columnIdx * bubbleSize.RuntimeValue.x);

				bubble.Type = (BubbleType)bubbleTypeIntValue;
				bubble.Coordinates = new Vector2(columnIdx, rowIdx);

				bubbleObject.transform.localPosition = bubblePosition;

				bubbleObject.SetActive(bubbleTypeIntValue != 0);
			}
		}
	}
	#endregion
}
