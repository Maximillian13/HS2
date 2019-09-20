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
	public bool displayProgress;

	public TextMeshPro[] descTexts;
	public string unlockedDescString;

	private bool unlocked;

	private CosmeticSelectionButton button;
	private BoxCollider boxCol;
	private TextMeshPro statOutOfText;
	private SpriteRenderer spriteRend;

	private bool fadeOut;
	private bool fadeIn;
	private float alpha;
	private const float SPEED = 2;

	// Start is called before the first frame update
	void Start()
    {
		alpha = 1;
		if(this.transform.Find("Sprite") != null)
			spriteRend = this.transform.Find("Sprite").GetComponent<SpriteRenderer>();
		else
			spriteRend = this.transform.parent.Find("Sprite").GetComponent<SpriteRenderer>();

		if (forceUnlock == true && lockedUnlockedGloves.Length < 1)
		{
			unlocked = true;
			descTexts[0].text = unlockedDescString;
			return;
		}

		button = this.GetComponent<CosmeticSelectionButton>();
		boxCol = this.GetComponent<BoxCollider>();

		if (displayProgress == true)
		{
			if (this.transform.Find("StatsOutOf") != null)
				statOutOfText = this.transform.Find("StatsOutOf").GetComponent<TextMeshPro>();
			else
				statOutOfText = this.transform.parent.Find("StatsOutOf").GetComponent<TextMeshPro>();
		}

		if (SteamManager.Initialized == true)
		{
			bool steamAch = false;
			SteamUserStats.GetAchievement(achivName, out steamAch);
			if(steamAch == true || forceUnlock == true)
			{
				unlocked = true;
				descTexts[0].text = unlockedDescString;
				if (button != null)
					button.enabled = true;
				if(displayProgress == true)
					statOutOfText.text = statMax + "/" + statMax;
				spriteRend.sprite = lockedUnlockedSprite[1];
				lockedUnlockedGloves[0].SetActive(false);
				lockedUnlockedGloves[1].SetActive(true);
			}
			else
			{
				if (button != null)
					button.enabled = false;
				if (boxCol != null)
					boxCol.enabled = false;
				if (displayProgress == true)
					statOutOfText.text = AchivmentAndStatControl.GetStat(assocaitedStat) + "/" + statMax;
				spriteRend.sprite = lockedUnlockedSprite[0];
				lockedUnlockedGloves[0].SetActive(true);
				lockedUnlockedGloves[1].SetActive(false);
			}
		}
	}

	private void Update()
	{
		if (fadeIn == true || fadeOut == true)
		{
			for (int i = 0; i < descTexts.Length; i++)
				descTexts[i].color = new Color(1, 1, 1, alpha);
			spriteRend.color = new Color(1, 1, 1, alpha);
			if(statOutOfText != null && displayProgress == true)
				statOutOfText.color = new Color(1, 1, 1, alpha);
			if (fadeIn == true)
			{
				alpha += SPEED * Time.deltaTime;
				if (alpha > 1.5f)
					fadeIn = false;
			}
			if (fadeOut == true)
			{
				alpha -= SPEED * Time.deltaTime;
				if (alpha < -1.5f)
					fadeOut = false;
			}
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
}
