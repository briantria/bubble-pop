/* author: Brian Tria
 * created: Dec 12, 2019
 * description: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	#region Properties
	[System.NonSerialized] public BubbleType Type;
	#endregion

	#region Member Variables
	// known issue for SerializedField throwing warnings
	// link: https://forum.unity.com/threads/serializefield-warnings.560878/
#pragma warning disable 0649
	[Header("Settings")]
	[SerializeField] private FloatVariable shootingSpeed;

	[Header("Game Perimeter")]
	[SerializeField] private VectorVariable bottomLeftPerimeterPoint;
	[SerializeField] private VectorVariable topRightPerimeterPoint;

	[Header("References")]
	[SerializeField] private IntVariable bubbleBulletType;
	[SerializeField] private BubbleTypeInfoList bubbleTypeInfoList;
	[SerializeField] private VectorVariable bubbleSize;
	[SerializeField] private VectorVariable targetPoint;
	[SerializeField] private VectorVariable bulletPosition;
	[SerializeField] private GameObjectList activeBubbleObjectList;
	[SerializeField] private GameObjectList inactiveBubbleObjectList;

	[Header("Game Events")]
	[SerializeField] private GameEvent onUpdateBulletPosition;
	[SerializeField] private SpriteRenderer tintRenderer;
#pragma warning restore 0649

	private bool shouldMove = false;
	private Vector3 initialPosition;
	private Vector3 currDirection;
	#endregion

	#region LifeCycle
	void Start()
	{
		if (HasMissingReference())
		{
			return;
		}

		initialPosition = transform.position;
		Reload();
	}

	void Update()
	{
		if (!shouldMove)
		{
			return;
		}

		if (HasMissingReference())
		{
			return;
		}

		Vector3 currPosition = transform.position;
		float halfHeight = bubbleSize.RuntimeValue.y * 0.5f;
		float halfWidth = bubbleSize.RuntimeValue.x * 0.5f;

		// on reach top of game perimeter 
		if (currPosition.y >= topRightPerimeterPoint.RuntimeValue.y - halfHeight)
		{
			Reload();
			return;
		}

		if (currPosition.x <= bottomLeftPerimeterPoint.RuntimeValue.x + halfWidth)
		{
			currDirection = Vector3.Reflect(currDirection, Vector3.right);
		}

		if (currPosition.x >= topRightPerimeterPoint.RuntimeValue.x - halfWidth)
		{
			currDirection = Vector3.Reflect(currDirection, Vector3.left);
		}

		Vector3 deltaPosition = currDirection * shootingSpeed.InitValue * Time.deltaTime;
		currPosition += deltaPosition;
		bulletPosition.RuntimeValue = currPosition;
		transform.position = currPosition;

		if (onUpdateBulletPosition != null)
		{
			onUpdateBulletPosition.Raise();
		}
	}
	#endregion

	#region Private Methods
	bool HasMissingReference()
	{
		if (shootingSpeed == null)
		{
			Debug.LogError("Missing reference to shooting speed.");
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

		if (bubbleSize == null)
		{
			Debug.LogError("Missing reference to bubble size.");
			return true;
		}

		if (tintRenderer == null)
		{
			Debug.LogError("Missing reference to tint renderer.");
			return true;
		}

		if (bulletPosition == null)
		{
			Debug.LogError("Missing reference to bullet position.");
			return true;
		}

		if (targetPoint == null)
		{
			Debug.LogError("Missing reference to target point.");
			return true;
		}

		if (bubbleBulletType == null)
		{
			Debug.LogError("Missing reference to bubble bullet type.");
			return true;
		}

		if (bubbleTypeInfoList == null)
		{
			Debug.LogError("Missing reference to bubble type info list object.");
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

		return false;
	}

	void ChooseNewBulletType()
	{
		List<BubbleType> activeBubbleTypes = new List<BubbleType>();

		foreach (GameObject bubbleObject in activeBubbleObjectList.Contents)
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
	}
	#endregion

	#region Public Methods
	public void BubbleShoot()
	{
		shouldMove = true;

		Vector3 targetPosition = targetPoint.RuntimeValue;
		targetPosition.z = 0;

		Vector3 currPosition = transform.position;
		currPosition.z = 0;

		currDirection = (targetPosition - currPosition).normalized;
	}

	public void Reload()
	{
		ChooseNewBulletType();
		shouldMove = false;
		Type = (BubbleType)bubbleBulletType.RuntimeValue;
		transform.position = initialPosition;

		bool hasValidType = false;
		foreach (BubbleTypeInfo bubbleTypeInfo in bubbleTypeInfoList.Contents)
		{
			hasValidType = Type == bubbleTypeInfo.MatchType;

			if (hasValidType)
			{
				tintRenderer.color = bubbleTypeInfo.InitColorValue;
				break;
			}
		}

		if (!hasValidType)
		{
			Debug.LogError("Unknown bubble type: " + Type);
		}
	}
	#endregion
}