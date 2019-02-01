using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class TeleportControl : MonoBehaviour
{
	public bool leftHand;
	private SteamVR_Input_Sources hand;

	private Transform rig;
	private Transform head;
	private Transform tpStart;

	private GameObject endMarker;

	private Vector3 oldPos;

	private bool snapTele;

    // Start is called before the first frame update
    void Start()
    {
		if (leftHand == true)
			hand = SteamVR_Input_Sources.LeftHand;
		else
			hand = SteamVR_Input_Sources.RightHand;

		rig = this.transform.parent;
		head = rig.Find("Camera");

		// Create Tport base (Will only happen to the dom hand. See method)
		this.CreaterTeleporterBase();

		// Checks what kind of teleportation we should use
		this.UpdateSnapTeleport();
	}


	/// <summary>
	/// Make a teleporting obj on the specified hand via PlayerPrefs.GetInt("DomHand")
	/// </summary>
	public void CreaterTeleporterBase()
	{
		// If its the right hand and we want left, return
		if (leftHand == false && PlayerPrefs.GetInt("DomHand") == 1)
			return;

		// If its the left hand and we want right, return
		if (leftHand == true && PlayerPrefs.GetInt("DomHand") == 0)
			return;

		// Make capsule and place at the tip of the finger 
		tpStart = Instantiate<Transform>(Resources.Load<Transform>("Other/TPHand"));
		tpStart.parent = this.transform;
		tpStart.localScale = new Vector3(.01f, -.01f, .01f);
		tpStart.localPosition = new Vector3(-.002f, .038f, .1f);
		tpStart.localEulerAngles = new Vector3(-90, 0, 0); 
		tpStart.SetAsLastSibling();
		tpStart.name = "TPHand";

		// Make the end marker 
		endMarker = Instantiate<GameObject>(Resources.Load<GameObject>("Other/TPEnd"));
		endMarker.name = "TPEnd";
		endMarker.SetActive(false);

		tpStart.gameObject.SetActive(false);
	}

	/// <summary>
	/// If the teleporter base exists on this hand, delete it
	/// </summary>
	public void DeleteTeleporterBase()
	{
		// Destroy the hand capsule
		Transform tpBase = this.transform.Find("TPHand");
		if (tpBase != null)
			Destroy(tpBase.gameObject);

		// Destroy the tp end marker
		GameObject tpEnd = GameObject.Find("TPEnd");
		if (tpEnd != null)
			Destroy(tpEnd);
	}

	/// <summary>
	/// Check the "SnapTele" Player pref and if its 0 turn on snap tele, else turn it off
	/// </summary>
	public void UpdateSnapTeleport()
	{
		if (PlayerPrefs.GetInt("SnapTele") == 0)
			snapTele = true;
		else
			snapTele = false;
	}

	// Update is called once per frame
	void Update()
    {
		// If we dont have the tp option (Is on the other hand)
		if (tpStart == null)
			return;


		// On grip press activate the teleporting pointer thing
		if (SteamVR_Input._default.inActions.GripPress.GetStateDown(hand))
			tpStart.gameObject.SetActive(true);

		// When being held, display where the player is pointing and will land 
		if (SteamVR_Input._default.inActions.GripPress.GetState(hand))
			this.TeleportPlayer(false);

		// Or release teleport the player to that location 
		if (SteamVR_Input._default.inActions.GripPress.GetStateUp(hand))
		{
			this.TeleportPlayer(true);
			tpStart.gameObject.SetActive(false);
		}
	}

	void TeleportPlayer(bool teleport)
	{
		// If we dont have the tp option (Is on the other hand)
		if (tpStart == null)
			return;

		// Set up ray to be shot 
		Ray ray = new Ray(this.tpStart.position, -this.tpStart.up);
		RaycastHit rayHit;


		// shoot a ray out the front of the tp thing and look for the ground with the tag "TportArea"
		if (Physics.Raycast(ray, out rayHit, float.PositiveInfinity, LayerMask.GetMask("TportArea")) == true)
		{
			// Display a marker where we will land (Check the diff from the old one so its not shaky)
			if (snapTele == true)
			{
				float diff = .2f;
				if (rayHit.point.x > oldPos.x + diff || rayHit.point.x < oldPos.x - diff ||
					rayHit.point.y > oldPos.y + diff || rayHit.point.y < oldPos.y - diff ||
					rayHit.point.z > oldPos.z + diff || rayHit.point.z < oldPos.z - diff)
				{
					endMarker.transform.position = rayHit.point;
				}
			}
			else
			{
				endMarker.transform.position = rayHit.point;
			}

			endMarker.SetActive(true);

			oldPos = endMarker.transform.position;

			// If we are actually teleporting move to the new stop
			if (teleport == true)
			{
				// Get where we need to move and move there 
				Vector3 headPosOnGround = new Vector3(head.position.x, rig.position.y, head.position.z);
				Vector3 translation = rayHit.point - headPosOnGround;
				rig.position += translation;
				
				// Get rid of the marker
				endMarker.SetActive(false);
			}
		}
		else
		{
			// If we leave a collider while having this pressed down 
			endMarker.SetActive(false);
		}
	}

}
