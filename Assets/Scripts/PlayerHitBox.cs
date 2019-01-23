// Written by Maximillian Coburn, Property of Bean Boy Games LLC. (Feel free to use it)
using UnityEngine;
using System.Collections;

public class PlayerHitBox : MonoBehaviour 
{
	private bool cardioMode;
    private int score; // Player score
    public TextMesh tScore; // Text mesh for the score
    public LeaderBoard leaderBoard; // For setting the leader board
    private CheckUp checkUp; // If the user went up after a wall
    private int checkUpCounter; // For when there are the multi-walls
    private bool gameOver;

	//private SquatCounter sc;
	//private TotalSquatCount tsc;
	//private SteamLeaderBoardUpdater slbu;

	private string GAME_MODE_PATH;

    // Set-up
	void Start () 
    {
		// If we are not in a gym destroy this component 
		GameObject cugo = GameObject.Find("CheckUp");
		if (cugo == null)
			Destroy(this);

		score = 0;
		tScore.text = score.ToString();

		if (cardioMode == false)
		{
			checkUp = GameObject.Find("CheckUp").GetComponent<CheckUp>();
			//sc = GameObject.Find("SquatWallCounterAchievments").GetComponent<SquatCounter>();
			//tsc = GameObject.Find("TotalSquatCount").GetComponent<TotalSquatCount>();
			//slbu = GameObject.Find("UpdateSteamLeaderBoard").GetComponent<SteamLeaderBoardUpdater>();
		}
	}

	/// <summary>
	/// Set the game from normal to cardio (True for cardio, False for normal)
	/// </summary>
	public void SetGameMode(bool cardio)
	{
		this.cardioMode = cardio;
	}

    // If something hits the player
    void OnTriggerEnter(Collider other)
    {
		// For the noremal game
		if (cardioMode == false)
		{
			// If its the Top of the wall reload level (Eventually explode boxes and show player high-score)
			if (other.name == "SquatWallUp")
			{
				this.EndGame();
			}
			// If its the bottom (squatted under the wall) increase the player score
			if (other.name == "SquatWallDown")
			{
				if (checkUp.Ready == true)
				{
					score++;
					tScore.text = score.ToString();

					//sc.UpdateStats(); // Increase the score by one (For achievements)
					//tsc.UpdateStats();

					// If it is a normal sized squat wall make the player stand back up right away
					if (other.transform.parent.parent == null)
						checkUp.Ready = false;
					else if (other.transform.parent.parent.name == "SquatWallx3(Clone)")    // If squat wall is 3 long, allow 3
						this.ResetAfterNumberOfWalls(3);
					else                                                                    // If squat wall is 5 long, allow 5	
						this.ResetAfterNumberOfWalls(5);

				}
				else // Did not come up from a squat
				{
					this.EndGame();
				}
			}
		}
		else // For cardio mode
		{
			// If the player hits the wall
			if (other.name == "CardioWallSolid")
			{
				this.EndGame();
			}
			// If the player goes through the open spot
			if (other.name == "CardioWallOpen")
			{
				score++;
				tScore.text = score.ToString();
			}
		}
    }

    /// <summary>
    /// Make the player stand back up after multiple walls
    /// </summary>
    /// <param name="numberOfWalls">Number of wall that the player is squatting</param>
    private void ResetAfterNumberOfWalls(int numberOfWalls)
    {
        if (checkUpCounter == numberOfWalls - 1)
        {
            checkUp.Ready = false;
            checkUpCounter = 0;
        }
        else
        {
            checkUpCounter++;
        }
    }

    public void EndGame()
    {
        if (gameOver == false)
        {
            // Save Score
			if(cardioMode == false)
				leaderBoard.SaveStats(score);
				//slbu.UpdateLeaderBoard(score);

            if (GameObject.Find("GuideRail") != null)
            {
                GuideRail gr = GameObject.Find("GuideRail").GetComponent<GuideRail>();
                if (gr != null)
                    gr.LowerRail();
            }

            // Get all the wall gibs
            GameObject[] gibsGO = GameObject.FindGameObjectsWithTag("Gibs");
            // Loop through all the gibs
            for (int x = 0; x < gibsGO.Length; x++)
            {
                Rigidbody gibs = gibsGO[x].GetComponent<Rigidbody>(); // Get the rigid body
                if (gibs.GetComponent<GibControl>().BlowUpGib == true)
                {
                    if (gibs.transform.parent != null && gibs.transform.parent.parent != null)
                    {
                        Transform t = gibs.transform.parent.parent; // Get the grandparent (Squat-Wall)
                        gibs.GetComponent<MeshRenderer>().enabled = true; // Make it so the player can see the gibs
                        gibs.gameObject.AddComponent<InteractableObject>(); // Make it so the player can pick up the wall gibs
                        gibs.transform.parent = null; // Un-parent the gibs
                        gibs.isKinematic = false; // Make physics work on them
                        gibs.AddForce(new Vector3(Random.Range(-750, 750), Random.Range(750, 1000), Random.Range(-300, 300))); // Add random force
                        gibs.AddTorque(new Vector3(Random.Range(-100, 100), Random.Range(100, 100), Random.Range(-100, 100))); // Add random torque
                        Destroy(t.gameObject); // Destroy the squat wall
                    }
                }
            }

			if (cardioMode == false)
			{
				// Get the wall spawner and tell it to stop the game
				WallSpawner wallSpawner = GameObject.Find("WallSpawner").GetComponent<WallSpawner>();
				wallSpawner.EndGame();
			}
			else
			{
				WallSpawnerCardio wallSpawnerCardio = GameObject.Find("WallSpawnerCardio").GetComponent<WallSpawnerCardio>();
				wallSpawnerCardio.EndGame();
			}

            gameOver = true;
        }
	}
}
