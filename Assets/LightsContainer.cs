using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsContainer : MonoBehaviour
{
    public const int ChannelsPerLight = 9;
    public Dictionary<int, string> ChannelNameMap;

    private void Start()
    {
        ChannelNameMap = new Dictionary<int, string>()
        {
            {0, "Color" },
            {1, "Strobe" },
            {2, "Dimmer" },
            {3, "Zoom" },
            {4, "PInsert" },
            {5, "PAngle" },
            {6, "PRotation" },
            {7, "Pan" },
            {8, "Tilt" }
        };
    }

    public string GetChannelName(int channel)
    {
        if (channel < GlobalScript.minDmxIndex || channel > GlobalScript.maxDmxIndex)
            return channel.ToString();

        string channelName = string.Empty;
        SharpyBaseScript[] lights = GetComponentsInChildren<SharpyBaseScript>();
        foreach (SharpyBaseScript light in lights)
        {
            int endChannel = light.startChannel + ChannelsPerLight;
            if (channel >= light.startChannel && channel < endChannel)
            {
                if (!string.IsNullOrEmpty(channelName))
                    channelName += "\n";
                channelName += light.Id;
                channelName += ChannelNameMap[channel - light.startChannel];
            }
        }

        if (string.IsNullOrEmpty(channelName))
            return channel.ToString();

        return channelName;
    }
}
