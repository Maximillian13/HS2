using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WallSpawnerCardio : MonoBehaviour
{
	private GameModeMaster master;

	// Fields 
	private GameObject[] walls = new GameObject[3];


	// Set-up
	void Start()
	{
		master = GameObject.Find("GameModeMaster").GetComponent<GameModeMaster>();

		walls[0] = Resources.Load<GameObject>("Walls/CardioWallLeft");
		walls[1] = Resources.Load<GameObject>("Walls/CardioWallMid");
		walls[2] = Resources.Load<GameObject>("Walls/CardioWallRight");
	}

	// Update is called once per frame
	void Update()
	{
		// Run the check code and if true return now
		if (master.CheckStopSpawning() == true)
			return;

		// Handle transitions
		master.TransitionHandler();

		// Check if we are on a break
		if (master.HandleOnBreak() == true)
			return;

		// Spawn the waves
		master.SpawnWavesHandlerCardio(walls);
	}

}