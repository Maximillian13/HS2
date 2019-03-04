using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LocalArcadeLeaderBoard : MonoBehaviour
{
	private TextMesh bodyText;

	private List<LocalArcadeLeaderBoardEntry> lbEntries = new List<LocalArcadeLeaderBoardEntry>();
	private string dataPath;

	private const int NUM_OF_ENTRIES_PER_PAGE = 8;


    // Start is called before the first frame update
    void Start()
    {
		// Destroy if we are not in arcade mode
        if(PlayerPrefs.GetInt(Constants.gameMode) != Constants.gameModeArcade)
		{
			Destroy(this.gameObject);
			return;
		}

		bodyText = this.transform.Find("Body").GetComponent<TextMesh>();

		dataPath = Application.persistentDataPath + "/ALS.txt";

		// If it is not here then we need to make it
		FileStream arcadeDataPath = File.Open(dataPath, FileMode.OpenOrCreate);
		arcadeDataPath.Close();


		// Go into the local leader board data file 
		using (StreamReader sr = new StreamReader(dataPath))
		{
			// Go through every line on the file
			while(sr.EndOfStream == false)
			{
				try
				{
					// Get line from file and split into elements (0 = name, 1 = score)
					string lbLine = sr.ReadLine();
					string[] lbEnements = lbLine.Split(' ');

					// Add a new LocalArcadeLeaderBoardEntry to the list using the file values
					lbEntries.Add(new LocalArcadeLeaderBoardEntry(lbEnements[0], int.Parse(lbEnements[1])));
				}
				// If something goes wrong just skip this element 
				catch { continue; }
			}
		}

		// Clear text
		bodyText.text = "";

		// Sort get entries in the correct order
		lbEntries.Sort();

		// Move through all entries (Structured like this so that if we have less than NUM_OF_ENTRIES it will just print those)
		for (int i = 0; i < lbEntries.Count; i++)
		{
			// If we print the top 8 return out 
			if (i == NUM_OF_ENTRIES_PER_PAGE)
				break;

			// Add the sores 
			bodyText.text += (i + 1) + ". " + lbEntries[i].Name + ": " + lbEntries[i].Score + "\n";
		}


		// If no entries then give a no entries message 
		if (bodyText.text == "")
			bodyText.text = "No local scores yet :(";
    }

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.H))
		{
			this.AddEntryAndShowPlacment("Test", 8);
		}
	}

	public void AddEntryAndShowPlacment(string name, int score)
	{
		// Add new entry and sort
		LocalArcadeLeaderBoardEntry newEntry = new LocalArcadeLeaderBoardEntry(name, score);
		lbEntries.Add(newEntry);
		lbEntries.Sort();

		// Clear text to be ready for new text
		bodyText.text = "";

		// Show surrounding entries
		int entryIndex = lbEntries.IndexOf(newEntry);

		// Less than the max amount, just print it all them out
		if(lbEntries.Count <= NUM_OF_ENTRIES_PER_PAGE)
		{
			// Add all the entries 
			for (int i = 0; i < lbEntries.Count; i++)
				this.AddEntry(i, entryIndex);
		}
		else // If we have more than NUM_OF_ENTRIES we want to print the score in the middle
		{
			// If in the top of the leader board, it can print normally 
			if (entryIndex < NUM_OF_ENTRIES_PER_PAGE / 2)
			{
				// Add all the entries 
				for (int i = 0; i < NUM_OF_ENTRIES_PER_PAGE; i++)
					this.AddEntry(i, entryIndex);
			}

			// If in the last few entries, just print the end normally
			else if (entryIndex > lbEntries.Count - (NUM_OF_ENTRIES_PER_PAGE / 2))
			{
				// Add all the entries 
				for (int i = lbEntries.Count - NUM_OF_ENTRIES_PER_PAGE; i < lbEntries.Count; i++)
					this.AddEntry(i, entryIndex);
			}

			// If somewhere in the middle
			else
			{
				// Add all the entries between the new index
				for (int i = entryIndex - (NUM_OF_ENTRIES_PER_PAGE / 2); i < entryIndex + (NUM_OF_ENTRIES_PER_PAGE / 2); i++)
					this.AddEntry(i, entryIndex);
			}
		}

		// Save to file 
		using (StreamWriter sw = new StreamWriter(dataPath))
		{
			for (int i = 0; i < lbEntries.Count; i++)
				sw.WriteLine(lbEntries[i].ToString());
		}
	}

	/// <summary>
	/// Adds the entry and surrounding entries, highlights the new entry 
	/// </summary>
	private void AddEntry(int currIndex, int entryIndex)
	{
		if (entryIndex == currIndex)
			bodyText.text += "<color=yellow>" + currIndex + ". " + lbEntries[currIndex].Name + ": " + lbEntries[currIndex].Score + "</color>\n";
		else
			bodyText.text += currIndex + ". " + lbEntries[currIndex].Name + ": " + lbEntries[currIndex].Score + "\n";
	}
}

/// <summary>
/// Holds Local Arcade Leader Board Entry info such as name and score
/// </summary>
public class LocalArcadeLeaderBoardEntry : System.IComparable
{
	public string Name { get; private set; }
	public int Score { get; private set; }

	/// <summary>
	/// Fills out an entry
	/// </summary>
	/// <param name="n">Name</param>
	/// <param name="s">Score</param>
	public LocalArcadeLeaderBoardEntry(string n, int s)
	{
		this.Name = n;
		this.Score = s;
	}

	// Format the ToString to be Name Score (i.e. Maxx 200)
	public override string ToString()
	{
		return this.Name + " " + this.Score;
	}

	// For sorting when in a list
	public int CompareTo(object obj)
	{
		LocalArcadeLeaderBoardEntry other = obj as LocalArcadeLeaderBoardEntry;

		if (this.Score > other.Score)
			return -1;
		else if (this.Score < other.Score)
			return 1;
		else
			return 0;
	}
}
