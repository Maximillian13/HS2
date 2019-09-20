// Written by Maximillian Coburn, Property of Bean Boy Games LLC. (Feel free to use it)
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadLevelButton : MonoBehaviour, IInteractibleButton
{
	public int whatToLoad;      // What the button will do
	private MeshRenderer mr;    // For highlighting
	//private bool highLightOverRide;

	// Setup
	void Start()
    {
		mr = this.transform.GetChild(0).GetComponent<MeshRenderer>();
    }

    // At the start of an interaction
    public void PressButton()
    {
		if (whatToLoad == -1)
			Application.Quit();

		SceneManager.LoadScene(whatToLoad);
    }


	/// <summary>
	/// Highlight with color if highlight == true, if false take away highlight
	/// </summary>
    public void HighLight(bool highLight)
    {
		mr.enabled = highLight;
    }


	///// <summary>
	///// Get and set the highlight value, if set to true, do not disable the mesh render when leaving the collision box
	///// </summary>
	//public bool HighLightOverRide
	//{
	//	get { return highLightOverRide; }
	//	set { highLightOverRide = value; }
	//}

}
