// Written by Maximillian Coburn, Property of Bean Boy Games LLC. (Feel free to use it)
using UnityEngine;
using System.Collections;
using System.IO;

public class PersonalHighScore : MonoBehaviour 
{
	private TextMesh title;
	private TextMesh score;

	// Use this for initialization
	void Start()
	{
		if (PlayerPrefs.GetInt("GameMode") != 0)
		{
			Destroy(this.gameObject);
			return;
		}

		title = this.transform.Find("ScoreTitle").GetComponent<TextMesh>();
		score = this.transform.Find("ScoreNumber").GetComponent<TextMesh>();

		bool cardio = PlayerPrefs.GetInt(Constants.cardioMode) == 1;
		if (cardio == true)
		{
			title.text = "CARDIO HIGH-SCORE";
			score.text = AchivmentAndStatControl.GetStat(Constants.highestCardioConsec).ToString();
		}
		else
		{
			title.text = "SQUAT HIGH-SCORE";
			score.text = AchivmentAndStatControl.GetStat(Constants.highestSquatConsec).ToString();
		}
	}
}
