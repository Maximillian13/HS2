using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonalHighScore : MonoBehaviour
{
	private TextMesh highScoreText;
	private TextMesh yourScoreText;

	private int currentScore;

	// Todo: make an arcade mode leaderbaord 
	// Use this for initialization
	void Start()
	{
		if (PlayerPrefs.GetInt(Constants.gameMode) != Constants.gameModeArcade)
		{
			Destroy(this.gameObject);
			return;
		}

		highScoreText = this.transform.Find("HighScoreNumber").GetComponent<TextMesh>();
		yourScoreText = this.transform.Find("YourScoreNumber").GetComponent<TextMesh>();

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
