/* author: Brian Tria
 * created: Dec 14, 2019
 * description: 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameObjectList", menuName = "ScriptableObject/Data/Game Object List", order = 51)]
public class GameObjectList : ScriptableObject
{
	public List<GameObject> Contents;
}
