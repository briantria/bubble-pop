/* author: Brian Tria
 * created: Dec 12, 2019
 * description: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
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
	[SerializeField] private VectorVariable targetPoint;

	// TODO: reference to a list of targets
#pragma warning restore 0649

	private bool shouldMove = false;
	private Vector3 initialPosition;
	private Vector3 currDirection;
	private Vector2 dimensions = Vector2.zero;
	#endregion

	#region LifeCycle
	void Start()
	{
		if (HasMissingReference())
		{
			return;
		}

		if (GetComponent<SpriteRenderer>() == null)
		{
			Debug.LogError("Missing sprite renderer component.");
			return;
		}

		initialPosition = transform.position;
		Debug.Log("init pos: " + initialPosition);
		dimensions = GetComponent<SpriteRenderer>().sprite.bounds.size;
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
		float halfHeight = dimensions.y * 0.5f;

		// on reach top of game perimeter 
		if (currPosition.y >= topRightPerimeterPoint.RuntimeValue.y - halfHeight)
		{
			shouldMove = false;
			transform.position = initialPosition;
			return;
		}

		// TODO: on hit a bubble target

		Vector3 deltaPosition = currDirection * shootingSpeed.InitValue * Time.deltaTime;
		transform.position += deltaPosition;
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

		return false;
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
	#endregion
}