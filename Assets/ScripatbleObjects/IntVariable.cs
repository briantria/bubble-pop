/* author: Brian Tria
 * created: Dec 14, 2019
 * description: 
 */

using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFloatVariable", menuName = "ScriptableObject/Data/Int Variable", order = 51)]
public class IntVariable : ScriptableObject
{
	#region Properties
	public int InitValue;

	[NonSerialized] public int RuntimeValue;
	#endregion

	public void OnAfterDeserialize()
	{
		RuntimeValue = InitValue;
	}
}
