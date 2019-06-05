using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using TMPro;


public class CosmeticUnlock : MonoBehaviour
{
	public bool forceUnlock;
	public string achivName;
	public string assocaitedStat;
	public GameObject[] lockedUnlockedGloves;
	public Sprite[] lockedUnlockedSprite;
	public int statMax;

	public TextMeshPro[] descStrings;

	private bool unlocked;

	private CosmeticSelectionButton button;
	private TextMeshPro statOutOfText;
	private SpriteRenderer spriteRend;

	private bool fadeOut;
	private bool fadeIn;
	private float alpha;
	private const float SPEED = 4;

	// Start is called before the first frame update
	void Start()
    {
		fadeOut = true;
		alpha = 1;
		spriteRend = this.transform.Find("Sprite").GetComponent<SpriteRenderer>();
		if (forceUnlock == true && lockedUnlockedGloves.Length < 1)
		{
			unlocked = true;
			return;
		}

		button = this.GetComponent<CosmeticSelectionButton>();
		statOutOfText= this.transform.Find("StatsOutOf").GetComponent<TextMeshPro>();

		if (SteamManager.Initialized == true)
		{
			bool steamAch = false;
			SteamUserStats.GetAchievement(achivName, out steamAch);
			Debug.Log(achivName + " unlocked = " + steamAch);

			if(steamAch == true || forceUnlock == true)
			{
				unlocked = true;
				button.enabled = true;
				statOutOfText.text = statMax + "/" + statMax;
				spriteRend.sprite = lockedUnlockedSprite[1];
				lockedUnlockedGloves[0].SetActive(false);
				lockedUnlockedGloves[1].SetActive(true);
			}
			else
			{
				int sc;
				SteamUserStats.GetStat(assocaitedStat, out sc);

				button.enabled = false;
				statOutOfText.text = sc + "/" + statMax;
				spriteRend.sprite = lockedUnlockedSprite[0];
				lockedUnlockedGloves[0].SetActive(true);
				lockedUnlockedGloves[1].SetActive(false);
			}
		}
	}

	private void Update()
	{
		// Fade in or out the descriptive texts
		if (fadeIn == true)
		{
			alpha += SPEED * Time.deltaTime;
			for (int i = 0; i < descStrings.Length; i++)
				descStrings[i].color = new Color(1, 1, 1, alpha);
			spriteRend.color = new Color(1, 1, 1, alpha);
			if (alpha > 1)
				fadeIn = false;
		}
		if (fadeOut == true)
		{
			alpha -= SPEED * Time.deltaTime;
			for (int i = 0; i < descStrings.Length; i++)
				descStrings[i].color = new Color(1, 1, 1, alpha);
			spriteRend.color = new Color(1, 1, 1, alpha);
			if (alpha > 1)
				fadeIn = false;
		}
	}

	/// <summary>
	/// Fade the descriptions out
	/// </summary>
	public void FadeOutAll()
	{
		alpha = 1;
		fadeIn = false;
		fadeOut = true;
	}

	/// <summary>
	/// Fade the descriptions in
	/// </summary>
	public void FadeInAll()
	{
		alpha = -1; // Delay so it waits to be in before fading 
		fadeIn = true;
		fadeOut = false;
	}

	public bool GetUnlocked()
	{
		return unlocked;
	}

	// Closes all sockets and kills all threads (This prevents unity from freezing)
	//private void OnApplicationQuit()
	//{
	//	if (SteamManager.Initialized == true)
	//	{
	//		SteamAPI.RunCallbacks();
	//		SteamAPI.Shutdown();
	//	}
	//}
}
