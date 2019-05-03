using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using TMPro;


public class CosmeticUnlock : MonoBehaviour
{
	public string achivName;
	public string assocaitedStat;
	public GameObject[] lockedUnlockedGloves;
	public Sprite[] lockedUnlockedSprite;
	public int statMax;

	private GameObject button;
	private TextMeshPro statOutOfText;
	private SpriteRenderer spriteRend;

	// Start is called before the first frame update
	void Start()
    {
		button = this.transform.Find("Button").gameObject;
		statOutOfText= this.transform.Find("StatsOutOf").GetComponent<TextMeshPro>();
		spriteRend = this.transform.Find("Sprite").GetComponent<SpriteRenderer>();

		if (SteamManager.Initialized == true)
		{
			bool steamAch = false;
			SteamUserStats.GetAchievement(achivName, out steamAch);
			Debug.Log(achivName + " unlocked = " + steamAch);

			if(steamAch == true)
			{
				button.SetActive(true);
				statOutOfText.text = statMax + "/" + statMax;
				spriteRend.sprite = lockedUnlockedSprite[1];
				lockedUnlockedGloves[0].SetActive(false);
				lockedUnlockedGloves[1].SetActive(true);
			}
			else
			{
				int sc;
				SteamUserStats.GetStat(assocaitedStat, out sc);

				button.SetActive(false);
				statOutOfText.text = sc + "/" + statMax;
				spriteRend.sprite = lockedUnlockedSprite[0];
				lockedUnlockedGloves[0].SetActive(true);
				lockedUnlockedGloves[1].SetActive(false);
			}
		}
	}

	// Closes all sockets and kills all threads (This prevents unity from freezing)
	private void OnApplicationQuit()
	{
		if (SteamManager.Initialized == true)
		{
			SteamAPI.RunCallbacks();
			SteamAPI.Shutdown();
		}
	}
	//private void OnDestroy()
	//{
	//	if (SteamManager.Initialized == true)
	//	{
	//		SteamAPI.RunCallbacks();
	//		SteamAPI.Shutdown();
	//	}
	//}
}
