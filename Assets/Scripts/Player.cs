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

	}

	// Update is called once per frame
	void Update()
	{

	}
}
