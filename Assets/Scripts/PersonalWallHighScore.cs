// Written by Maximillian Coburn, Property of Bean Boy Games LLC. (Feel free to use it)
using UnityEngine;
using System.Collections;


public class PersonalWallHighScore : MonoBehaviour 
{
	private TextMesh title;
	private TextMesh score;

	// Use this for initialization
	void Start()
	{
		if (PlayerPrefs.GetInt(Constants.gameMode) != Constants.gameModeClassic)
		{
			Destroy(this.gameObject);
			return;
		}

		title = this.transform.Find("ScoreTitle").GetComponent<TextMesh>();
		score = this.transform.Find("ScoreNumber").GetComponent<TextMesh>();

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
