using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerControl : MonoBehaviour
{
	public Transform walkingNodeParrent;
	private Transform[] walkingNodes;

	private int goToIndex;

	private float SPEED = 5;

    // Start is called before the first frame update
    void Start()
    {
		// Set up nodes and move the runner in the right place 
		walkingNodes = new Transform[walkingNodeParrent.childCount];
		for (int i = 0; i < walkingNodeParrent.childCount; i++)
			walkingNodes[i] = walkingNodeParrent.GetChild(i);

		this.transform.position = walkingNodes[0].position;
		goToIndex++;
	}

    // Update is called once per frame
    void Update()
    {
		// Make the runner look where they are going and move towards it 
		this.transform.LookAt(walkingNodes[goToIndex]);
		this.transform.localEulerAngles = new Vector3(0, this.transform.localEulerAngles.y + 180, 0);
		this.transform.position = Vector3.MoveTowards(this.transform.position, walkingNodes[goToIndex].position, SPEED * Time.deltaTime);

		// Once we reach the node move to the next node
		if (Vector3.Distance(this.transform.position, walkingNodes[goToIndex].position) < .1f)
			this.GotoNewNode();
    }


	private void GotoNewNode()
	{
		// Move to the next node, if the last node restart at the beginning  
		goToIndex++;
		if (goToIndex == walkingNodes.Length)
			goToIndex = 0;

		// if In normal speed make them go a little faster or slower
		if (SPEED >= 3.5f && SPEED <= 6.5f)
			SPEED += Random.Range(-.2f, .2f);

		// if they are going too fast or too slow adjust accordingly 
		if (SPEED < 3.5f)
		{
			SPEED += Random.Range(0, .2f);
			return;
		}
		if (SPEED > 6.5f)
		{
			SPEED += Random.Range(-.2f, 0);
			return;
		}
	}

}
