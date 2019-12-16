/* author: Brian Tria
 * created: Dec 11, 2019
 * description: Handles user inputs
 */

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameEvent", menuName = "ScriptableObject/Game Event", order = 51)]
public class GameEvent : ScriptableObject
{
	private List<GameEventListener> listeners = new List<GameEventListener>();

	public void Raise()
	{
		for (int idx = 0; idx < listeners.Count; ++idx)
		{
			listeners[idx].OnEventRaised();
		}
	}

	public void RegisterListener(GameEventListener listener)
	{
		listeners.Add(listener);
	}

	public void UnregisterListener(GameEventListener listener)
	{
		listeners.Remove(listener);
	}
}

