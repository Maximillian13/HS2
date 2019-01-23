// Written by Maximillian Coburn, Property of Bean Boy Games LLC. (Feel free to use it)
using UnityEngine;
using System.Collections;

public class HandPlacmentLeft : MonoBehaviour 
{
    ///////////////////////////////////////
    // Look to HandPlacment.cs for logic //
    ///////////////////////////////////////

    // If the left controller is in or not
    private bool controllerLeftIn;

    // Controller is in
    void OnTriggerStay(Collider other)
    {
        if(other.name == "Controller (left)")
        {
            controllerLeftIn = true;
        }
    }

    // Controller is out
    void OnTriggerExit(Collider other)
    {
        if (other.name == "Controller (left)")
        {
            controllerLeftIn = false;
        }
    }

    // The state of the controller 
    public bool GetControllerLeftIn()
    {
        return controllerLeftIn;
    }
}
