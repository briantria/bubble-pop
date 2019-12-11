/* author: Brian Tria
 * created: Dec 11, 2019
 * description: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimManager : MonoBehaviour
{
	#region Member Variables
	// known issue for SerializedField throwing warnings
	// link: https://forum.unity.com/threads/serializefield-warnings.560878/
#pragma warning disable 0649

	[Header("Settings")]
	[SerializeField] private FloatVariable aimTrailLength;
	[SerializeField] private FloatVariable aimTrailSpacing;
	[SerializeField] private VectorVariable targetPoint;

	[Header("References")]
	[SerializeField] private Transform particleTransform;
#pragma warning restore 0649

	// private Transform trailParticle;
	#endregion

	#region LifeCycle
	void Start()
	{
		// TODO: initiate target particle object pool
	}
	#endregion

	bool HasMissingReference()
	{
		if (aimTrailLength == null)
		{
			Debug.LogError("Missing reference to aim trail length.");
			return true;
		}

		if (aimTrailSpacing == null)
		{
			Debug.LogError("Missing reference to aim trail spacing.");
			return true;
		}

		if (targetPoint == null)
		{
			Debug.LogError("Missing reference to target point.");
			return true;
		}

		if (particleTransform == null)
		{
			Debug.LogError("Missing reference to aim trail prefab.");
			return true;
		}

		return false;
	}

	public void UpdateAimDirection()
	{
		if (HasMissingReference())
		{
			return;
		}

		// TODO: handle multiple trail particles
		Vector3 position = transform.position;
		position.z = 0;

		Vector3 targetPosition = targetPoint.RuntimeValue;
		targetPosition.z = 0;

		Vector3 aimDirection = (targetPosition - position).normalized;
		// Debug.Log("target direction: " + aimDirection);

		aimDirection = aimDirection * aimTrailSpacing.InitValue;
		// Debug.Log("spacing: " + aimTrailSpacing.InitValue);
		// Debug.Log("adjusted target direction: " + aimDirection);

		particleTransform.localPosition = aimDirection;
	}
}
