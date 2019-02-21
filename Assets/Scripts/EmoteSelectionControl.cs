using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoteSelectionControl : MonoBehaviour
{
	public EmoteSelectionButton[] emoteButtons;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	/// <summary>
	/// Remove the given binding from all emote buttons (0 = up, 1 = right, 2 = bottom, 3 = right) 
	/// </summary>
	public void RemoveBinding(int bindToRemove)
	{
		for(int i = 0; i < emoteButtons.Length; i++)
		{
			GameObject[] eWheel = emoteButtons[i].GetEmoteWheel();
			eWheel[bindToRemove].SetActive(false);
			
		}
	}

	/// <summary>
	/// Remove the highlight effect if no bind is selected
	/// </summary>
	public void RemoveHighlightIfNess()
	{
		for (int i = 0; i < emoteButtons.Length; i++)
		{
			GameObject[] eWheel = emoteButtons[i].GetEmoteWheel();

			bool atleastOneBind = false;
			for (int j = 0; j < eWheel.Length; j++)
			{
				if (eWheel[j].activeSelf == true)
					atleastOneBind = true;
			}

			if (atleastOneBind == false)
				emoteButtons[i].Deselect();
		}
	}
}
