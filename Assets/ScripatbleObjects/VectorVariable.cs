/* author: Brian Tria
 * created: Dec 12, 2019
 * description: 
 */

using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewVectorVariable", menuName = "ScriptableObject/Variables/Vector", order = 51)]
public class VectorVariable : ScriptableObject
{
	#region Properties
	public Vector3 InitValue;
	[NonSerialized] public Vector3 RuntimeValue;
	#endregion

	public void OnAfterDeserialize()
	{
		RuntimeValue = InitValue;
	}

	public void OnBeforSerialize()
	{

	}
}
