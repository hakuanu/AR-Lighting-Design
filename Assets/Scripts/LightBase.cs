using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBase : MonoBehaviour
{
    public int startChannel; // starting channel for this fixture
    private int channels; // number of control channels for this light

    // Use this for initialization
    public virtual void Start()
    {
        SetChannels(1);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int GetStartChannel()
    {
        return startChannel;
    }

    // update the start channel for this fixture
    public virtual void SetStartChannel(int channelIndex)
    {
    }

    public int GetChannels()
    {
        return channels;
    }

    public void SetChannels(int c)
    {
        channels = c;
    }

    public void UpdatePosition()
    {

    }

    public void UpdateRotation()
    {

    }
}
