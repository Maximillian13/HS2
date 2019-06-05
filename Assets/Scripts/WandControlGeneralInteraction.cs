using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Valve.VR;

public class WandControlGeneralInteraction : MonoBehaviour
{
	public bool leftHand;
	private SteamVR_Input_Sources hand;
	private HashSet<InteractableObject> hoveredObject = new HashSet<InteractableObject>();
	private InteractableObject closesyItem;
	private InteractableObject interactingItem;

	private BoxCollider boxColl;
	private float collTimer;

	private void Start()
	{
		collTimer = float.PositiveInfinity;

		if (leftHand == true)
		{
			hand = SteamVR_Input_Sources.LeftHand;
			boxColl = GameObject.Find("BoxColLeft").GetComponent<BoxCollider>();
		}
		else
		{
			hand = SteamVR_Input_Sources.RightHand;
			boxColl = GameObject.Find("BoxColRight").GetComponent<BoxCollider>();
		}
	}

	void Update()
	{
		boxColl.transform.position = this.transform.position;
		boxColl.transform.eulerAngles = this.transform.eulerAngles;
		// If you press the trigger button
		if (SteamVR_Actions._default.TriggerPress.GetStateDown(hand))
		{
			// Calculate the minimum distance if there are multiple object you are interacting with
			this.GetClosestItem();

			// Set the item that you are interacting with
			interactingItem = closesyItem;

			// If you aren't holding anything
			this.CheckHoldIfNull();

			if (interactingItem != null)
			{
				interactingItem.StartInteraction(this);
				boxColl.enabled = false;
			}
		}

		// If you release the trigger button
		if (SteamVR_Actions._default.TriggerPress.GetStateUp(hand))
		{
			// Drop the item
			this.DropItem();
			if (interactingItem != null)
			{
				interactingItem = null;
				closesyItem = null;
			}
			collTimer = Time.time + .25f;
		}

		// Re-enable the box collider after releasing 
		if(Time.time > collTimer)
		{
			collTimer = float.PositiveInfinity;
			boxColl.enabled = true;
		}
	}

	// If you aren't holding anything
	private void CheckHoldIfNull()
	{
		// Check if you grabbed anything
		if (interactingItem != null)
		{
			// End interaction if you are all ready holding it with other hand
			if (interactingItem.IsInteracting() == true)
			{
				interactingItem.EndInteraction(this);
			}
		}
	}

	// Drop the item
	private void DropItem()
	{
		if (interactingItem != null)
		{
			interactingItem.EndInteraction(this);
		}
	}

	// Calculate the minimum distance if there are multiple object you are interacting with
	private void GetClosestItem()
	{
		float minDistance = float.PositiveInfinity;
		float distance;

		foreach (InteractableObject item in hoveredObject)
		{
			if (item != null)
			{
				distance = (item.transform.position - this.transform.position).sqrMagnitude;

				if (distance < minDistance)
				{
					minDistance = distance;
					closesyItem = item;
				}
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		// Find what its touching
		InteractableObject collidedObject = other.GetComponent<InteractableObject>();
		// Set it up to be carried
		if (collidedObject != null)
		{
			hoveredObject.Add(collidedObject);
		}
	}


	void OnTriggerExit(Collider other)
	{
		// Find what its touching
		InteractableObject collidedObject = other.GetComponent<InteractableObject>();
		// Set it up to be dropped
		if (collidedObject != null)
		{
			hoveredObject.Remove(collidedObject);
		}
	}
}
