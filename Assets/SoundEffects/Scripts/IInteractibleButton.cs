using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractibleButton
{
	/// <summary>
	/// Active whatever this particular button does 
	/// </summary>
	void PressButton();

	/// <summary>
	/// Highlight with color if highlight == true, if false take away highlight
	/// </summary>
	void HighLight(bool highLight);

	/// <summary>
	/// Get and set the highlight value, if set to true, do not disable the mesh render when leaving the collision box
	/// </summary>
	//bool HighLightOverRide { get; set; }
}
