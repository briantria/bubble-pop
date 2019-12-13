/* author: Brian Tria
 * created: MMM d, yyyy
 * description: 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBubbleTypeList", menuName = "ScriptableObject/Data/Bubble Type List", order = 51)]
public class BubbleTypeInfoListObject : ScriptableObject
{
	public List<BubbleTypeInfo> BubbleTypeInfoList;
}
