/* author: Brian Tria
 * created: Dec 11, 2019
 * description: 
 */

using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
	#region Member Variables
	// known issue for SerializedField throwing warnings
	// link: https://forum.unity.com/threads/serializefield-warnings.560878/
#pragma warning disable 0649
	[SerializeField] private GameEvent gameEvent;
	[SerializeField] private UnityEvent response;
#pragma warning restore 0649
	#endregion

	private void OnEnable()
	{
		if (gameEvent != null)
		{
			gameEvent.RegisterListener(this);
		}
		else
		{
			Debug.LogError("Missing reference to game event.");
		}
	}

	private void OnDisable()
	{
		if (gameEvent != null)
		{
			gameEvent.UnregisterListener(this);
		}
		else
		{
			Debug.LogError("Missing reference to game event.");
		}
	}

	public void OnEventRaised()
	{
		if (response != null)
		{
			response.Invoke();
		}
		else
		{
			Debug.LogError("Missing reference to response.");
		}
	}
}
