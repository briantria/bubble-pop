/* author: Brian Tria
 * created: Dec 13, 2019
 * description: 
 */

using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBubbleType", menuName = "ScriptableObject/Data/Bubble Type Info", order = 51)]
public class BubbleTypeInfo : ScriptableObject
{
	#region Properties
	public BubbleType MatchType;
	public Color InitColorValue;
	[NonSerialized] public Color RuntimeColorValue;
	#endregion

	public void OnAfterDeserialize()
	{
		RuntimeColorValue = InitColorValue;
	}

	public void OnBeforSerialize()
	{

	}
}
