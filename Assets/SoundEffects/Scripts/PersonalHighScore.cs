using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PersonalHighScore : MonoBehaviour
{
	private TextMeshPro highScoreText;
	private TextMeshPro yourScoreText;

	private int currentScore;

	// Use this for initialization
	void Start()
	{
		if (PlayerPrefs.GetInt(Constants.gameMode) != Constants.gameModeArcade)
		{
			Destroy(this.gameObject);
			return;
		}

		highScoreText = this.transform.Find("HighScoreNumber").GetComponent<TextMeshPro>();
		yourScoreText = this.transform.Find("YourScoreNumber").GetComponent<TextMeshPro>();

		highScoreText.text = AchivmentAndStatControl.GetStat(Constants.highScore).ToString();
		yourScoreText.text = currentScore.ToString();
	}

	public void UpdateYourScore(int increaseAmount)
	{
		currentScore += increaseAmount;
		yourScoreText.text = currentScore.ToString();
	}

	public int GetYourScore()
	{
		return currentScore;
	}
}
