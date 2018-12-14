using HoloToolkit.Examples.InteractiveElements;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmxSliderControl : SliderGestureControl
{
    public int ChannelOffset;

    public UnityEventIntInt OnUpdateEventIntInt;

    public void Start()
    {
        UpdateChannelLabel();
    }

    public override void ManipulationUpdate(Vector3 startGesturePosition, Vector3 currentGesturePosition, Vector3 startHeadOrigin, Vector3 startHeadRay, GestureInteractive.GestureManipulationState gestureState)
    {
        base.ManipulationUpdate(startGesturePosition, currentGesturePosition, startHeadOrigin, startHeadRay, gestureState);
        OnUpdateEventIntInt.Invoke(GetChannel(), (int)SliderValue);
    }

    public int GetChannel()
    {
        return GetComponentInParent<SliderPanelScript>().ChannelBase + ChannelOffset;
    }

    public void UpdateChannelLabel()
    {
        LightsContainer lightsContainer = GetComponentInParent<LightsContainer>();

        TextMesh[] labels = GetComponentsInChildren<TextMesh>(true);
        foreach (TextMesh label in labels)
        {
            if (label.name.Equals("ChannelLabel"))
            {
                label.text = lightsContainer.GetChannelName(GetChannel());
                break;
            }
        }
    }
}
