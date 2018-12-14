using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharpyHeadScript : LightHead
{
    // Use this for initialization
    public override void Start()
    {
        SetTiltChannel(transform.parent.GetComponentInParent<SharpyBaseScript>().GetStartChannel() + 8);
        transform.localEulerAngles = new Vector3(35, 0, 0);
        SetTiltUnit(250f / 255);
    }
}
