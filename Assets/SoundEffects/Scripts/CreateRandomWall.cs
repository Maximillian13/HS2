using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRandomWall : MonoBehaviour
{
	private GameObject[] walls;
	private Vector3 wallPos = new Vector3(.01f, 0, 1.35f);

	private const int WALL_COUNT = 7;

    // Start is called before the first frame update
    void Start()
    {
		// Set up an array with all the different walls 
		walls = new GameObject[WALL_COUNT];

		for (int i = 0; i < walls.Length; i++)
			walls[i] = Resources.Load<GameObject>("Walls/WallGameObjects/SquatWall" + i);

		// Destroy the place holder wall
		GameObject wall = this.transform.GetChild(0).gameObject;
		Destroy(wall.GetComponent<MeshRenderer>());
		Destroy(wall.GetComponent<MeshFilter>());

		// Add the new wall 
		int rand = Random.Range(0, WALL_COUNT);
		GameObject wallGO = Instantiate<GameObject>(walls[rand], this.gameObject.transform);
		wallGO.transform.localPosition = wallPos;
		wallGO.transform.SetAsFirstSibling();

		// Hide Gibs 
		for (int i = 1; i < this.transform.childCount; i++)
			this.transform.GetChild(i).GetComponent<MeshRenderer>().enabled = false;
	}
}
