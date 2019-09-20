// Written by Maximillian Coburn, Property of Bean Boy Games LLC. (Feel free to use it)
using UnityEngine;
using System.Collections;
using TMPro;


public class PersonalWallHighScore : MonoBehaviour 
{
	private TextMeshPro title;
	private TextMeshPro score;

	// Use this for initialization
	void Start()
	{
		if (PlayerPrefs.GetInt(Constants.gameMode) != Constants.gameModeClassic)
		{
			Destroy(this.gameObject);
			return;
		}

		title = this.transform.Find("ScoreTitle").GetComponent<TextMeshPro>();
		score = this.transform.Find("ScoreNumber").GetComponent<TextMeshPro>();

		bool cardio = PlayerPrefs.GetInt(Constants.cardioMode) == 1;
		if (cardio == true)
		{
			title.text = "CARDIO WALL\nHIGH-SCORE";
			score.text = AchivmentAndStatControl.GetStat(Constants.highestCardioConsec).ToString();
		}
		else
		{
			title.text = "SQUAT WALL\nHIGH-SCORE";
			score.text = AchivmentAndStatControl.GetStat(Constants.highestSquatConsec).ToString();
		}
	}
}
