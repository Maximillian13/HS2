using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchingBag : MonoBehaviour
{
	public StatWall statWall;

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "HandCol")
		{
			AchivmentAndStatControl.IncrementStat(Constants.punchingBagPunches);
			AchivmentAndStatControl.CheckPunchingBagAchiv();
			statWall.ReloadStats();
		}
	}
}
