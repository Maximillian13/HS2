// Written by Maximillian Coburn, Property of Bean Boy Games LLC. (Feel free to use it)
using UnityEngine;
using System.Collections;
using TMPro;

public class HandPlacment : MonoBehaviour 
{
    // The left and right hand
    public HandPlacmentLeft leftHand;
    public HandPlacmentRight rightHand;
    public TextMeshPro tm;

    // The material and render of the box you need to be inside 
    public Material[] mat;
    private Renderer rend;

    // The hit box of the player to end the game
    private PlayerHitBox phb;

    // If the game is started
    private bool gameStarted;
    private float outTimer;

	// Use this for initialization
	void Start ()
    {
        gameStarted = false;
        rend = this.GetComponent<Renderer>();
        phb = this.transform.parent.GetComponent<PlayerHitBox>();

        rend.material = mat[0];
	}
	
    void Update()
    {
        // If both hands are in then set the material to be the right color and set the timer to stay at 0
        if(leftHand.GetControllerLeftIn() == true && rightHand.GetControllerRightIn() == true)
        {
            rend.material = mat[0];
            tm.gameObject.SetActive(false);
            outTimer = 0;
        }
        else // If one of the hands is out set the material to a warning color and start counting down the time till they lose
        {
            rend.material = mat[1];
            tm.gameObject.SetActive(true);
            if (gameStarted == true)
            {
                outTimer += Time.deltaTime;
            }
        }

        // If the game has started
        if(gameStarted == true)
        {
            // If the hands have been out for more than 2.5 seconds reset the game
            if(outTimer >= 4)
            {
                phb.EndGame();
            }
        }
    }

    /// <summary>
    /// Will set the game to be started
    /// </summary>
    public void GameStarted()
    {
        gameStarted = true;
    }

}
