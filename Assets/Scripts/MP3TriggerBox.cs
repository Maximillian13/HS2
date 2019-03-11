using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MP3TriggerBox : MonoBehaviour
{
	// What hand is in the box to grab it (0 = left, 1 = right, -1 = none)
	private int handIn = -1;
	private bool mp3InHand;

	private void OnTriggerEnter(Collider other)
	{
		// BUG: For some reason if you press the trigger button as the haptics are firing it will go continusly 
		if (other.name == "Controller (left)" || other.name == "Controller (right)")
			other.GetComponent<WandControlMP3>().TriggerHaptic(.2f, .1f);
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.name == "Controller (left)")
			handIn = 0;
		if (other.name == "Controller (right)")
			handIn = 1;
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.name == "Controller (left)" || other.name == "Controller (right)")
			handIn = -1;
	}

	public bool MP3InHand
	{
		get { return this.mp3InHand; }
		set { this.mp3InHand = value; }
	}

	public int HandInTriggerBox
	{
		get { return this.handIn; }
		private set { this.handIn = value; }
	}
}
