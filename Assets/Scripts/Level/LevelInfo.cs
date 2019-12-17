/* author: Brian Tria
 * created: Dec 13, 2019
 * description: 
 */

using System.IO;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelInfo
{
	public int RowCount;
	public int ColumnCount;
	public List<Row> Rows;

	public static LevelInfo CreateFromJsonFileForLevel(int level)
	{
		// source: https://support.unity3d.com/hc/en-us/articles/115000341143-How-do-I-read-and-write-data-from-a-text-file-
		string path = "Assets/Resources/Level" + level + ".json";
		StreamReader reader = new StreamReader(path);
		string jsonString = reader.ReadToEnd();
		reader.Close();

		// Debug.Log("json string: " + jsonString);
		return JsonUtility.FromJson<LevelInfo>(jsonString);
	}
}

[System.Serializable]
public class Row
{
	public List<int> Columns;
}
