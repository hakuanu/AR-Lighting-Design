using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalScript : MonoBehaviour
{
    public enum DMXChannels
    {
        ColorWheel,
        Strobe,
        Dimmer,
        Pan,
        Tilt
    }

    static public int[] dmxUniverse; // dmx Universe
    int time = 0;

    // Use this for initialization
    void Start ()
    {
        dmxUniverse = new int[512];
    }

    // Update is called once per frame
    // Update dmxUniverse values from network signal
    void Update ()
    {
    }

    public void SetChannelValue(int channel, int value)
    {
        dmxUniverse[channel] = value;
    }

    public GameObject prefab;
    public GameObject beam;
    public GameObject empty;
    public GameObject lightsContainer;

    // constants
    public const int minDmxIndex = 0;
    public const int maxDmxIndex = 511;
}
