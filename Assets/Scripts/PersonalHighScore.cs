// Written by Maximillian Coburn, Property of Bean Boy Games LLC. (Feel free to use it)
using UnityEngine;
using System.Collections;
using System.IO;

public class PersonalHighScore : MonoBehaviour 
{
    private BinaryWriter bw;
    private BinaryReader br;

    public TextMesh lb; // The text that will display in game
    private string FOLDER_PATH = Directory.GetCurrentDirectory() + "\\Data";
    private string FILE_PATH = Directory.GetCurrentDirectory() + "\\Data\\PHS"; // The directory to put the text file
    int prevNumOfSquats = 0; // prev number of squats so we dont override peoples score
    private bool emptyFile; // If the file is empty

	// Use this for initialization
    void Start()
    {
        // If the file does not exist 
        if (!File.Exists(FILE_PATH))
        {
            // Create the directory
            Directory.CreateDirectory(FOLDER_PATH);
            
            // Fill it with 0
            using (bw = new BinaryWriter(new FileStream(FILE_PATH, FileMode.Create)))
            {
                bw.Write(0);
            }
        }

        using (br = new BinaryReader(new FileStream(FILE_PATH, FileMode.Open)))
        {
            // If the data is good set prevNumberOfSquats to the current high score
            if (int.TryParse(br.ReadInt32().ToString(), out prevNumOfSquats) == false)
            {
                emptyFile = true;
            }
        }

        if(emptyFile == true)
        {
            using (bw = new BinaryWriter(new FileStream(FILE_PATH, FileMode.Create)))
            {
                bw.Write(0);
            }
        }

        // Show the score to beat
        lb.text = prevNumOfSquats.ToString();
    }

    // Called when the game end
    public void SaveStats(int numOfSquats)
    {
        // If the new number is bigger than the old number save over it
        int prev = 0;

        using (br = new BinaryReader(new FileStream(FILE_PATH, FileMode.Open)))
        {
            int.TryParse(br.ReadInt32().ToString(), out prev);
        }

        if (numOfSquats > prev)
        {
            using (bw = new BinaryWriter(new FileStream(FILE_PATH, FileMode.Create)))
            {
                bw.Write(numOfSquats);
            }
        }
    }
}
