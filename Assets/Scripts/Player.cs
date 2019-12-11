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

	[Header("Game Events")]
	[SerializeField] private GameEvent onBubbleShoot;
	[SerializeField] private GameEvent onCancelBubbleShoot;

	[Header("Prefab References")]
	[SerializeField] private GameObject bubbleBulletPrefab;
	[SerializeField] private GameObject aimManagerPrefab;
#pragma warning restore 0649

	private GameObject bubbleBullet;
	private GameObject aimManager;
	#endregion

	#region LifeCycle
	void Start()
	{
		if (HasMissingReference())
		{
			return;
		}

		bubbleBullet = Instantiate(bubbleBulletPrefab, transform.position, Quaternion.identity);
		aimManager = Instantiate(aimManagerPrefab, transform.position, Quaternion.identity);

		aimManager.SetActive(false);
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

	bool HasMissingReference()
	{
		if (shootingSpeed == null)
		{
			Debug.LogError("Missing reference to shooting speed.");
			return true;
		}

		if (bubbleBulletPrefab == null)
		{
			Debug.LogError("Missing reference to bubble bullet prefab.");
			return true;
		}

		if (aimManagerPrefab == null)
		{
			Debug.LogError("Missing reference to aim trail prefab.");
			return true;
		}

		return false;
	}

	public void StartAim()
	{
		aimManager.SetActive(true);
	}

	public void AttemptBubbleShoot()
	{
		aimManager.SetActive(false);
	}

	public void BubbleShoot()
	{

	}

	public void CancelBubbleShoot()
	{

	}
}
