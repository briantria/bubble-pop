/* source: https://unity3d.com/how-to/architect-with-scriptable-objects
 */

using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Float Variable")]
public class FloatVariable : ScriptableObject
{
	#region Properties
	public float InitValue;

	[NonSerialized]
	public float RuntimeValue;
	#endregion

	public void OnAfterDeserialize()
	{
		RuntimeValue = InitValue;
	}

	public void OnBeforSerialize()
	{

	}
}
