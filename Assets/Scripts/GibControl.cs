using UnityEngine;
using System.Collections;

public class GibControl : MonoBehaviour 
{
    private bool blowUpGib;

    public bool BlowUpGib
    {
        get
        {
            return blowUpGib;
        }
        set
        {
            blowUpGib = value;
        }
    }
	
}
