using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScorePopper : MonoBehaviour
{
	public void PopScoreMessage(string message, float size = .05f)
	{
		GameObject messageInstance = Instantiate(Resources.Load("Other/ScorePopperMessageGameObject") as GameObject);
		messageInstance.transform.position = new Vector3(messageInstance.transform.position.x, messageInstance.transform.position.y - 1, messageInstance.transform.position.z);
		messageInstance.transform.localScale = new Vector3(size, size, size);
		messageInstance.GetComponent<ScorePopperMessage>().SetMessage(message);
	}
}
