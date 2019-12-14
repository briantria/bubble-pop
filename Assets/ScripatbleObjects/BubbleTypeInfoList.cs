/* author: Brian Tria
 * created: Dec 14, 2019
 * description: 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBubbleTypeList", menuName = "ScriptableObject/Data/Bubble Type Info List", order = 51)]
public class BubbleTypeInfoList : ScriptableObject
{
	public List<BubbleTypeInfo> Contents;
}
