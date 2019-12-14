/* author: Brian Tria
 * created: Dec 14, 2019
 * description: 
 */

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameObjectList", menuName = "ScriptableObject/Variables/Game Object List", order = 51)]
public class GameObjectList : ScriptableObject
{
	public List<GameObject> Contents;
}
