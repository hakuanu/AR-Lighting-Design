using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightHead : MonoBehaviour
{
    private int tiltChannel; // the channel we're referencing to denote tilt of the head
    private float tiltUnit; // how many degrees to rotate per channel value

    // Use this for initialization
    public virtual void Start()
    {
        transform.localEulerAngles = new Vector3(35, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // set new rotation of the mount
        transform.localEulerAngles = new Vector3(35 - GlobalScript.dmxUniverse[tiltChannel] * tiltUnit, 0, 0);
    }

    // update tilt channel when light base's start channel changes
    public void SetTiltChannel(int channel)
    {
        tiltChannel = channel;
    }

    public int GetTiltChannel()
    {
        return tiltChannel;
    }

    // update tilt channel when light base's start channel changes
    public void SetTiltUnit(float unit)
    {
        tiltUnit = unit;
    }

    public float GetTiltUnit()
    {
        return tiltUnit;
    }
}
