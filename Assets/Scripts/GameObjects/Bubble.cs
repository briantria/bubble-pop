/* author: Brian Tria
 * created: Dec 13, 2019
 * description: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
	#region Member Variables
	// known issue for SerializedField throwing warnings
	// link: https://forum.unity.com/threads/serializefield-warnings.560878/
#pragma warning disable 0649
	[SerializeField] private VectorVariable bubbleSize;
#pragma warning restore 0649
	#endregion

	void Start()
	{
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
		if (spriteRenderer == null)
		{
			Debug.LogError("Missing bubble sprite renderer.");
			return;
		}
	}
}
