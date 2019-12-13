/* author		: Brian Tria
 * created		: Dec 10, 2019
 * description	: Handles player controls
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	// TODO: preve next bubble type

	#region Member Variables
	// known issue for SerializedField throwing warnings
	// link: https://forum.unity.com/threads/serializefield-warnings.560878/
#pragma warning disable 0649
	[Header("Game Events")]
	[SerializeField] private GameEvent onBubbleShoot;

	[Header("References")]
	[SerializeField] private VectorVariable bubbleSize;
	[SerializeField] private VectorVariable targetPoint;
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

		if (targetPoint == null)
		{
			Debug.LogError("Missing reference to target point.");
			return true;
		}

		if (bubbleSize == null)
		{
			Debug.LogError("Missing reference to bubble bubble size.");
			return true;
		}

		return false;
	}

	public void StartAim()
	{
		if (HasMissingReference())
		{
			return;
		}

		aimManager.SetActive(true);
	}

	public void AttemptBubbleShoot()
	{
		if (HasMissingReference())
		{
			return;
		}

		aimManager.SetActive(false);

		if (transform.position.y + bubbleSize.RuntimeValue.y >= targetPoint.RuntimeValue.y)
		{
			return;
		}

		if (onBubbleShoot != null)
		{
			onBubbleShoot.Raise();
		}
	}
}
