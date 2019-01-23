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

	private void Start()
	{
		if(leftHand == true)
			hand = SteamVR_Input_Sources.LeftHand;
		else
			hand = SteamVR_Input_Sources.RightHand;
	}

	void Update()
	{
		// If you press the trigger button
		if (SteamVR_Input._default.inActions.TriggerPress.GetStateDown(hand))
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
			}
		}

		// If you release the trigger button
		if (SteamVR_Input._default.inActions.TriggerPress.GetStateUp(hand))
		{
			// Drop the item
			this.DropItem();
			if (interactingItem != null)
			{
				interactingItem = null;
				closesyItem = null;
			}
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
