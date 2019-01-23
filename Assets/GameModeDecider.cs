using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeDecider : MonoBehaviour
{
	public WallSpawner wallSpawner;
	public WallSpawnerCardio wallSpawnerCardio;
	public GameObject[] objsToDestroy;
	public PlayerHitBox phb;

    // Start is called before the first frame update
    void Start()
    {
		// Check the game mode using player prefs
		bool cardioMode = PlayerPrefs.GetInt("GameMode") == 1 ? true : false;

		if (cardioMode == true)
		{
			Destroy(wallSpawner.gameObject);
			for (int i = 0; i < objsToDestroy.Length; i++)
				Destroy(objsToDestroy[i].gameObject);
		}
		else
			Destroy(wallSpawnerCardio.gameObject);

		phb.SetGameMode(cardioMode);
	}
}
