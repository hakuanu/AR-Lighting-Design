using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharpyMountScript : LightMount
{
    // Use this for initialization
    public override void Start()
    {
        SetPanChannel(transform.parent.GetComponentInParent<SharpyBaseScript>().GetStartChannel() + 7);
        transform.localEulerAngles = new Vector3(0, 0, 0);
        SetPanUnit(540f / 255);
    }
}
