using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Steamworks;

public class TotalSquatCount : MonoBehaviour
{
	private BinaryWriter bw;
	private BinaryReader br;

	private string FOLDER_PATH = Directory.GetCurrentDirectory() + "\\Data";
	private string FILE_PATH = Directory.GetCurrentDirectory() + "\\Data\\TS"; // The directory to put the text file
	int numberOfSquats = 0; // prev number of squats so we dont override peoples score
	private bool emptyFile; // If the file is empty

	private const int ONE_ACHIV = 50; // Home Gym
	private const int TWO_ACHIV = 100; // 1-Month Trial
	private const int THREE_ACHIV = 250; // Cardio Casual
	private const int FOUR_ACHIV = 500; // Gym Rat
	private const int FIVE_ACHIV = 1000; // Leg Day, Every Day
	private const int SIX_ACHIV = 10000; // King Of The Gym
	private const int SEVEN_ACHIV = 25000; // Guardian Of The Gym

	// Use this for initialization
	void Start()
	{

		// If the file does not exist 
		if (!File.Exists(FILE_PATH))
		{
			// Create the directory
			Directory.CreateDirectory(FOLDER_PATH);
			using (bw = new BinaryWriter(new FileStream(FILE_PATH, FileMode.Create)))
			{
				bw.Write(0);
			}
		}

		// Just in-case some junk data gets in rewrite it just as a 0
		using (br = new BinaryReader(new FileStream(FILE_PATH, FileMode.Open)))
		{
			// If the data is good set prevNumberOfSquats to the current high score
			if (int.TryParse(br.ReadInt32().ToString(), out numberOfSquats) == false)
			{
				emptyFile = true;
			}
		}

		if (emptyFile == true)
		{
			using (bw = new BinaryWriter(new FileStream(FILE_PATH, FileMode.Create)))
			{
				bw.Write(0);
			}
		}

	}

	/// <summary>
	/// Update the stats and keeps track of total squats 
	/// </summary>
	public void UpdateTotalSquatStats()
	{
		// Update squat count, store it, and check if we have an achivment
		numberOfSquats++;
		SteamUserStats.SetStat("SquatCount", numberOfSquats);
		Debug.Log("Storing Steam Stat was successful = " + SteamUserStats.StoreStats());
		AchivmentAndStatControl.CheckAllTotalSquatAchivments(numberOfSquats);

		using (bw = new BinaryWriter(new FileStream(FILE_PATH, FileMode.Create)))
		{
			bw.Write(numberOfSquats);
		}
	}
}
