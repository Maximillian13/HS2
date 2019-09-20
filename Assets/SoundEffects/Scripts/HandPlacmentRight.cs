// Written by Maximillian Coburn, Property of Bean Boy Games LLC. (Feel free to use it)
using UnityEngine;
using System.Collections;

public class HandPlacmentRight : MonoBehaviour 
{
    ///////////////////////////////////////
    // Look to HandPlacment.cs for logic //
    ///////////////////////////////////////

    // If the left controller is in or not
    private bool controllerRightIn;

    void OnTriggerStay(Collider other)
    {
        if (other.name == "Controller (right)")
        {
            controllerRightIn = true;
        }
    }

    // Controller is out
    void OnTriggerExit(Collider other)
    {
        if (other.name == "Controller (right)")
        {
            controllerRightIn = false;
        }
    }

    // The state of the controller 
    public bool GetControllerRightIn()
    {
        return controllerRightIn;
    }
}
