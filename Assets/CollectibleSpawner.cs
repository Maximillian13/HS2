using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
	public string whatToSpawn;

	/// <summary>
	/// Spawn the Collectible at the position of the spawn gameobject 
	/// </summary>
	public void SpawnCollectible()
	{
		Instantiate<GameObject>(Resources.Load<GameObject>("Collectibles/" + whatToSpawn));
	}
}
