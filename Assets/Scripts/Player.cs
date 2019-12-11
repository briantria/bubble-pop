/* author		: Brian Tria
 * created		: Dec 10, 2019
 * description	: Handles player controls
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	#region Member Variables

	// known issue for SerializedField throwing warnings
	// link: https://forum.unity.com/threads/serializefield-warnings.560878/
#pragma warning disable 0649

	[Header("Settings")]
	[SerializeField] private FloatVariable shootingSpeed;
	[SerializeField] private FloatVariable aimTrailLength;

	[Header("Prefab References")]
	[SerializeField] private GameObject bubbleBulletPrefab;
	[SerializeField] private GameObject aimTrailPrefab;

#pragma warning restore 0649

	#endregion

	#region LifeCycle
	void Start()
	{
		if (HasMissingReference())
		{
			return;
		}

		Instantiate(bubbleBulletPrefab, transform.position, Quaternion.identity);
	}

	// Update is called once per frame
	void Update()
	{
		if (HasMissingReference())
		{
			return;
		}
	}
	#endregion

	private bool HasMissingReference()
	{
		if (bubbleBulletPrefab == null)
		{
			Debug.LogError("Missing reference to bubble bullet prefab.");
			return true;
		}

		if (aimTrailPrefab == null)
		{
			Debug.LogError("Missing reference to aim trail prefab.");
			return true;
		}

		return false;
	}
}
