using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class CalorieCounter : MonoBehaviour
{
	// Squat Calculation based off this website https://www.livestrong.com/article/313995-calories-burned-during-squats/

	private TextMeshPro count;

	private float prevTime;
	private int currCount;
	private int weight;


	// Use this for initialization
	void Start()
	{
		if (PlayerPrefs.GetInt("UseCalorieCounter") == 0)
			Destroy(this.gameObject);

		// If we have an old one destroy it 
		if (this.CheckIfOriginal() == false)
			return;

		SceneManager.sceneLoaded += this.LevelLoaded;
		count = this.transform.Find("CaloriesBurnedNumber").GetComponent<TextMeshPro>();
		prevTime = float.NegativeInfinity;
		weight = PlayerPrefs.GetInt("PlayerWeight");
		count.text = "0";
		DontDestroyOnLoad(this.gameObject);
		this.name = "CalorieCounterOrig";
	}

	private bool CheckIfOriginal()
	{
		GameObject[] calCounters = GameObject.FindGameObjectsWithTag("CalorieCounter");
		if (calCounters.Length > 1)
		{
			if (this.gameObject.name == "CalorieCounter")
			{
				Destroy(this.gameObject);
				return false;
			}
		}

		return true;
	}

	public void UpdateCount(float currTime, bool cardioMode, float speedFactor = 0)
	{
		// First time through, set up the prev time and return
		if (prevTime == float.NegativeInfinity)
		{
			prevTime = currTime;
			return;
		}

		// Todo: Get correct calBurn factor for cardio mode
		// Get what calBurn factor we will use depending on the game mode
		float calBurn = cardioMode == true ? .111f : .196f;

		float timeDif = currTime - prevTime;
		prevTime = currTime;
		currCount += (int)(((weight * (calBurn + speedFactor)) / 60) * timeDif);
		count.text = currCount.ToString();
	}

	public void SetPrevTime(float curTime)
	{
		prevTime = curTime;
	}

	// When level is loaded
	private void LevelLoaded(Scene level, LoadSceneMode sceneMode)
	{
		// Prevent weird behavior with routine 
		if (this == null)
			return;

		// If we are in the main menu
		if(level.buildIndex == 1)
		{
			AchivmentAndStatControl.IncrementStat(Constants.totalCaloriesBurned, currCount);
			Destroy(this.gameObject);
		}
	}
}
