using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharpyBaseScript : LightBase, IInputHandler, IXboxControllerHandler
{
    public GameObject LightUx;
    public int Id;

    // Use this for initialization
    public override void Start()
    {
        SetChannels(9);
    }

    // update the start channel for this fixture
    public override void SetStartChannel(int channelIndex)
    {
        // padding just in case
        int maxChannel = 512 - GetChannels();
        startChannel = (channelIndex < maxChannel) ? channelIndex : maxChannel;
    }

    public void OnInputDown(InputEventData eventData)
    {
        if (LightUx != null)
        {
            SliderPanelScript sliderPanelScript = LightUx.GetComponentInChildren<SliderPanelScript>(true);
            if (sliderPanelScript != null && 
                (!sliderPanelScript.gameObject.activeSelf || sliderPanelScript.CurrentLight != this))
                sliderPanelScript.Unhide(this);
        }
    }

    public void OnInputUp(InputEventData eventData)
    {
        // intentionally blank
    }

    public void OnXboxInputUpdate(XboxControllerEventData eventData)
    {
        if (eventData.XboxA_Down)
        {
            if (LightUx != null)
            {
                SliderPanelScript sliderPanelScript = LightUx.GetComponentInChildren<SliderPanelScript>(true);
                if (sliderPanelScript != null &&
                    (!sliderPanelScript.gameObject.activeSelf || sliderPanelScript.CurrentLight != this))
                    sliderPanelScript.Unhide(this);
            }
        }
    }
}
