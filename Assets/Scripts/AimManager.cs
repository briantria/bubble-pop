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

	[Header("References")]
	[SerializeField] private Transform particleTransform;

#pragma warning restore 0649
	#endregion

	#region LifeCycle
	void Start()
	{

	}
	#endregion

	bool HasMissingReference()
	{
		if (aimTrailLength == null)
		{
			Debug.LogError("Missing reference to aim trail length.");
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

	}
}
