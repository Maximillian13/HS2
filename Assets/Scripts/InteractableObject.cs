using UnityEngine;
using System.Collections;

public class InteractableObject : MonoBehaviour
{
	// For highlighting

	// For picking up
	private Rigidbody ridge;
	private bool currentlyInteracting;
	private WandControlGeneralInteraction attachedWand;
	private Transform interactionPoint;

	private float velocityFactor = 3000f;
	private Vector3 posDelta;

	private float rotationFactor = 100;
	private Quaternion rotationDelta;
	private float angle;
	private Vector3 axis;

	// Use this for initialization
	void Start()
	{
		// Set up
		//rend = this.GetComponent<Renderer>();
		ridge = this.GetComponent<Rigidbody>();
		interactionPoint = new GameObject().transform;

		velocityFactor = velocityFactor / ridge.mass;
		rotationFactor = rotationFactor / ridge.mass;
	}

	// Update is called once per frame
	void Update()
	{
		// If you are holding something
		if (attachedWand != null && currentlyInteracting == true)
		{
			KeepInHand(attachedWand);
		}

	}

	private void KeepInHand(WandControlGeneralInteraction normWand)
	{
		// Set the position based on where the wand is
		posDelta = normWand.transform.position - interactionPoint.position;
		// Dont need this anymore because it breaks teleportation and is not necessary 
		//if (posDelta.x > .75f || posDelta.y > .75f || posDelta.z > .75f)
		//{
		//	EndInteraction(normWand);
		//}
		this.ridge.velocity = posDelta * velocityFactor * Time.fixedDeltaTime;

		// Set the rotation based on where the wand is
		rotationDelta = normWand.transform.rotation * Quaternion.Inverse(interactionPoint.rotation);
		rotationDelta.ToAngleAxis(out angle, out axis);

		// Helps make it so it does not do a weird flip
		if (angle > 180)
		{
			angle -= 360;
		}

		// Check if any bad numbers come in
		try
		{
			if (float.IsNaN(axis.x) == false && float.IsNaN(axis.y) == false && float.IsNaN(axis.z) == false && float.IsNaN(this.ridge.angularVelocity.x) == false && float.IsNaN(this.ridge.angularVelocity.y) == false && float.IsNaN(this.ridge.angularVelocity.z) == false && float.IsNaN(rotationFactor) == false && float.IsNaN(angle) == false)
			{
				this.ridge.angularVelocity = (Time.fixedDeltaTime * angle * axis) * rotationFactor;
				//this.transform.rotation = normWand.transform.rotation;   
			}
		}
		catch
		{
			Debug.Log("There was an error calculating the interactable objects velocity");
		}
	}

	// At the start of an interaction
	public void StartInteraction(WandControlGeneralInteraction wand)
	{
		// Attach the wand 
		attachedWand = wand;
		interactionPoint.position = wand.transform.position;
		interactionPoint.rotation = wand.transform.rotation;
		interactionPoint.SetParent(this.transform, true);

		currentlyInteracting = true;
	}

	public void EndInteraction(WandControlGeneralInteraction wand)
	{
		if (wand == attachedWand) // So the other wand cant trigger this
		{
			// Detach wand
			attachedWand = null;
			currentlyInteracting = false;
		}
	}

	// If its already interacting
	public bool IsInteracting()
	{
		return currentlyInteracting;
	}
}
