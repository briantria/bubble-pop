/* author: Brian Tria
 * created: Dec 14, 2019
 * description: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchingSystem : MonoBehaviour
{
	#region Member Variables
	// known issue for SerializedField throwing warnings
	// link: https://forum.unity.com/threads/serializefield-warnings.560878/
#pragma warning disable 0649
	[Header("References")]
	[SerializeField] private GameObjectList activeBubbleObjectList;
	[SerializeField] private GameObjectList inactiveBubbleObjectList;

	[Header("Game Events")]
	[SerializeField] private GameEvent onBulletReload;
#pragma warning restore 0649
	#endregion

	#region Private Methods
	bool HasMissingReference()
	{
		if (onBulletReload == null)
		{
			Debug.LogError("Missing reference to bullet reload.");
			return true;
		}

		if (activeBubbleObjectList == null)
		{
			Debug.LogError("Missing reference to active bubble object list.");
			return true;
		}

		if (inactiveBubbleObjectList == null)
		{
			Debug.LogError("Missing reference to inactive bubble object list.");
			return true;
		}

		return false;
	}
	#endregion

	public void PopMatchedBubbles()
	{
		// TODO: check matches
		// TODO: recursive call to pop neighboring bubbles

		if (onBulletReload != null)
		{
			onBulletReload.Raise();
		}
	}
}
