/* author: Brian Tria
 * created: Dec 14, 2019
 * description: 
 */

using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameObjectList", menuName = "ScriptableObject/Variables/Int List", order = 51)]
public class IntVariableList : ScriptableObject
{
	public List<int> InitContents;

	[NonSerialized] public List<int> RuntimeContents;

	public void OnAfterDeserialize()
	{
		RuntimeContents = InitContents;
	}

	public void OnBeforSerialize()
	{

	}
}
