using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class BFSaveTest : MonoBehaviour {

    BinaryWriter bw;
    BinaryReader br;
    private string FOLDER_PATH = Directory.GetCurrentDirectory() + "\\Data";
    private string FILE_PATH = Directory.GetCurrentDirectory() + "\\Data\\test"; // The directory to put the text file

	// Use this for initialization
	void Start ()
    {
        if (!File.Exists(FILE_PATH))
        {
            // Create the directory
            Directory.CreateDirectory(FOLDER_PATH);
        }

        using (bw = new BinaryWriter(new FileStream(Directory.GetCurrentDirectory() + "\\Data\\test", FileMode.Create)))
        {
            bw.Write(456);
        }

        using (br = new BinaryReader(new FileStream(Directory.GetCurrentDirectory() + "\\Data\\test", FileMode.Open)))
        {
            Debug.Log(br.ReadInt32());
        }

    }
	
	// Update is called once per frame
	void Update () {

	}
}
