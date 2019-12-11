/* author: Brian Tria
 * created: Dec 11, 2019
 * description: Handles user inputs
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
	#region Member Variables
	// known issue for SerializedField throwing warnings
	// link: https://forum.unity.com/threads/serializefield-warnings.560878/
#pragma warning disable 0649
	// [Header("Settings")]
	[SerializeField] private VectorVariable targetPoint;

	[Header("Game Events")]
	[SerializeField] private GameEvent onStartAim;
	[SerializeField] private GameEvent onUpdateAimDirection;
	[SerializeField] private GameEvent onAttemptBubbleShoot;
#pragma warning restore 0649
	#endregion

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (onStartAim != null)
			{
				onStartAim.Raise();
			}
		}

		if (Input.GetMouseButton(0))
		{
			if (targetPoint != null)
			{
				targetPoint.RuntimeValue = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				//Debug.Log("target pos: " + targetPoint.RuntimeValue);
			}

			if (onUpdateAimDirection != null)
			{
				onUpdateAimDirection.Raise();
			}
		}

		if (Input.GetMouseButtonUp(0))
		{
			if (onAttemptBubbleShoot != null)
			{
				onAttemptBubbleShoot.Raise();
			}
		}
	}
}
