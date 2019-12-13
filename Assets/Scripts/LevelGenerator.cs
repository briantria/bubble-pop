/* author: Brian Tria
 * created: Dec 13, 2019
 * description: 
 */

using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
	#region Member Variables
	// known issue for SerializedField throwing warnings
	// link: https://forum.unity.com/threads/serializefield-warnings.560878/
#pragma warning disable 0649
	[Header("Game Perimeter")]
	[SerializeField] private VectorVariable bottomLeftPerimeterPoint;
	[SerializeField] private VectorVariable topRightPerimeterPoint;

	[Header("References")]
	[SerializeField] private GameObject bubblePrefab;

#pragma warning restore 0649
	#endregion

	// TODO: take json input for level design

	void Start()
	{
		/*
		0 -> space
		1 - 9, A - Z -> bubble with given type

		if even ->
		if odd ->
		*/

		LoadJsonLevel();
	}

	// Update is called once per frame
	void Update()
	{

	}

	#region Private Methods
	bool HasMissingReference()
	{
		if (bottomLeftPerimeterPoint == null)
		{
			Debug.LogError("Missing reference to bottom left perimeter point.");
			return true;
		}

		if (topRightPerimeterPoint == null)
		{
			Debug.LogError("Missing reference to top right perimeter point.");
			return true;
		}

		if (bubblePrefab == null)
		{
			Debug.LogError("Missing reference to bubble prefab.");
			return true;
		}

		return false;
	}

	void LoadJsonLevel()
	{
		string path = "Assets/Resources/Level1.json";

		//Read the text from directly from the test.txt file
		StreamReader reader = new StreamReader(path);
		Debug.Log(reader.ReadToEnd());
		reader.Close();
	}
	#endregion
}
