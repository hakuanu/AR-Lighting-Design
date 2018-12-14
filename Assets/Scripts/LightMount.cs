using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMount : MonoBehaviour
{
    private int panChannel; // the channel we're referencing to denote pan of the mount
    private float panUnit; // how many degrees to rotate per channel value

    // Use this for initialization
    public virtual void Start()
    {
        transform.localEulerAngles = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // set new rotation of the mount
        transform.localEulerAngles = new Vector3(0, GlobalScript.dmxUniverse[panChannel] * panUnit, 0);
    }

    // set the pan channel when light base's start channel changes
    public virtual void SetPanChannel(int channel)
    {
        panChannel = channel;
    }

    // update pan channel when light base's start channel changes
    public int GetPanChannel()
    {
        return panChannel;
    }

    // set the pan channel when light base's start channel changes
    public void SetPanUnit(float unit)
    {
        panUnit = unit;
    }

    // update pan channel when light base's start channel changes
    public float GetPanUnit()
    {
        return panUnit;
    }
}
