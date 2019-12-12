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

	[Header("Game Perimeter")]
	[SerializeField] private VectorVariable bottomLeftPerimeterPoint;
	[SerializeField] private VectorVariable topRightPerimeterPoint;

	[Header("References")]
	[SerializeField] private Transform particleTransform;
#pragma warning restore 0649

	private List<Transform> trailParticleList = new List<Transform>();
	#endregion

	#region LifeCycle
	void Start()
	{
		if (HasMissingReference())
		{
			return;
		}

		InstantiateParticlePool();
	}
	#endregion

	#region Private Methods
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

	void InstantiateParticlePool()
	{
		trailParticleList.Add(particleTransform);
		for (int idx = 1; idx < aimTrailLength.InitValue; ++idx)
		{
			Transform particle = Instantiate(particleTransform, transform.position, Quaternion.identity, transform);
			trailParticleList.Add(particle);
		}
	}

	void ShowTrailParticles(bool isActive)
	{
		for (int idx = 0; idx < trailParticleList.Count; ++idx)
		{
			trailParticleList[idx].gameObject.SetActive(isActive);
		}
	}
	#endregion

	public void UpdateAimDirection()
	{
		if (HasMissingReference())
		{
			return;
		}

		Vector3 targetPosition = targetPoint.RuntimeValue;
		Vector3 position = transform.position;

		if (targetPosition.y <= position.y)
		{
			ShowTrailParticles(false);
			return;
		}

		ShowTrailParticles(true);

		// TODO: handle multiple trail particles
		targetPosition.z = 0;
		position.z = 0;
		Vector3 aimDirection = (targetPosition - position).normalized;
		// Debug.Log("target direction: " + aimDirection);

		aimDirection = aimDirection * aimTrailSpacing.InitValue;
		// Debug.Log("spacing: " + aimTrailSpacing.InitValue);
		// Debug.Log("adjusted target direction: " + aimDirection);

		particleTransform.localPosition = aimDirection;
	}
}
