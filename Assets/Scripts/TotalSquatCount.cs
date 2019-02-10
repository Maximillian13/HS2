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
	private string FILE_PATH_SQUAT = Directory.GetCurrentDirectory() + "\\Data\\TS"; // The directory to put the text file
	private string FILE_PATH_CARDIO = Directory.GetCurrentDirectory() + "\\Data\\TC"; // The directory to put the text file
	private int numberOfSquats = 0; // prev number of squats so we dont override peoples score
	private int numberOfCardio = 0; // prev number of cardio so we dont override peoples score
	private bool emptyFile; // If the file is empty


	// Use this for initialization
	void Start()
	{
		numberOfSquats = this.FileSetUp(FILE_PATH_SQUAT);
		numberOfCardio = this.FileSetUp(FILE_PATH_CARDIO);
	}

	private int FileSetUp(string filePath)
	{
		// The value we will get when reading from the file 
		int fileValue = 0;
		
		// If the file does not exist 
		if (!File.Exists(filePath))
		{
			// Create the directory
			Directory.CreateDirectory(FOLDER_PATH);
			using (bw = new BinaryWriter(new FileStream(filePath, FileMode.Create)))
				bw.Write(0);
		}

		// Just in-case some junk data gets in rewrite it just as a 0
		using (br = new BinaryReader(new FileStream(filePath, FileMode.Open)))
		{
			// If the data is good set prevNumberOfSquats to the current high score
			if (int.TryParse(br.ReadInt32().ToString(), out fileValue) == false)
				emptyFile = true;
		}

		if (emptyFile == true)
		{
			using (bw = new BinaryWriter(new FileStream(filePath, FileMode.Create)))
				bw.Write(0);

			return 0;
		}

		return fileValue;
	}

	/// <summary>
	/// Update the stats and keeps track of total squats 
	/// </summary>
	public void UpdateTotalSquatStats()
	{
		// Update total squat count, store it, and check if we have an achivment
		numberOfSquats++;

		AchivmentAndStatControl.CheckAllTotalSquatAchivments(numberOfSquats);
		AchivmentAndStatControl.SetStat("TotalSquatWallCount", numberOfSquats);

		using (bw = new BinaryWriter(new FileStream(FILE_PATH_SQUAT, FileMode.Create)))
		{
			bw.Write(numberOfSquats);
		}
	}

	/// <summary>
	/// Update the stats and keeps track of total cardio walls 
	/// </summary>
	public void UpdateTotalCardioStats()
	{
		// Update total cardio count, store it, and check if we have an achivment
		numberOfCardio++;

		AchivmentAndStatControl.CheckAllTotalCardioAchivments(numberOfCardio);
		AchivmentAndStatControl.SetStat("TotalCardioWallCount", numberOfCardio);

		using (bw = new BinaryWriter(new FileStream(FILE_PATH_CARDIO, FileMode.Create)))
		{
			bw.Write(numberOfCardio);
		}
	}
}
