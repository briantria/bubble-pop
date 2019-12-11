/* author		: Brian Tria
 * created		: Dec 10, 2019
 * description	: Handles player controls
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private FloatVariable shootingSpeed;
	[SerializeField] private FloatVariable aimTrailLength;

	[Header("Prefab References")]
	[SerializeField] private GameObject bubbleBulletPrefab;
	[SerializeField] private GameObject aimTrailPrefab;

	// Start is called before the first frame update
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
