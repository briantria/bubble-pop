﻿/* author: Brian Tria
 * created: Dec 13, 2019
 * description: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
	#region Properties
	public Vector2 Coordinates;
	#endregion

	#region Member Variables
	// known issue for SerializedField throwing warnings
	// link: https://forum.unity.com/threads/serializefield-warnings.560878/
#pragma warning disable 0649
	[SerializeField] private VectorVariable bubbleSize;
	[SerializeField] private VectorVariable bulletPosition;
	[SerializeField] private VectorVariable bulletHitPosition;
	[SerializeField] private VectorVariable bubbleHitCoordinates;
	[SerializeField] private SpriteRenderer tintRenderer;

	[Header("Game Events")]
	[SerializeField] private GameEvent onBulletHit;
#pragma warning restore 0649

	private bool shouldTriggerBulletHit = true;
	#endregion

	void Start()
	{
		if (HasMissingReference())
		{
			return;
		}

		// TODO: buble info setup
		// - match type, color, ...
	}

	#region Private Methods
	bool HasMissingReference()
	{
		if (bulletPosition == null)
		{
			Debug.LogError("Missing reference to bullet position.");
			return true;
		}

		if (bulletHitPosition == null)
		{
			Debug.LogError("Missing reference to bullet hit position.");
			return true;
		}

		if (bubbleHitCoordinates == null)
		{
			Debug.LogError("Missing reference to bubble hit coordinates.");
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

		return false;
	}
	#endregion

	#region Public Methods
	public void CheckBulletHit()
	{
		if (!shouldTriggerBulletHit || HasMissingReference())
		{
			return;
		}

		Vector3 currPosition = transform.position;
		currPosition.z = 0;

		Vector3 currBulletPosition = bulletPosition.RuntimeValue;
		currBulletPosition.z = 0;

		float sqrBulletDistance = (currPosition - currBulletPosition).sqrMagnitude;
		float collisionTreshold = bubbleSize.RuntimeValue.x + 0.1f;

		if (sqrBulletDistance > collisionTreshold)
		{
			return;
		}

		bulletHitPosition.RuntimeValue = bulletPosition.RuntimeValue;
		bubbleHitCoordinates.RuntimeValue = new Vector3(Coordinates.x, Coordinates.y, 0);

		Debug.Log("hit coords: " + bubbleHitCoordinates.RuntimeValue);

		if (onBulletHit != null)
		{
			onBulletHit.Raise();
		}
	}

	public void StopBulletHitCheck()
	{
		shouldTriggerBulletHit = false;
	}

	public void StartBulletHitCheck()
	{
		shouldTriggerBulletHit = true;
	}
	#endregion
}
