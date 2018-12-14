using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.Receivers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderPanelScript : MonoBehaviour
{
    public int ChannelBase;

    /// <summary>
    /// Should always be set to the number of sliders on the panel
    /// </summary>
    public int ChannelsPerPage;

    public SharpyBaseScript CurrentLight; 

	// Use this for initialization
	void Start () {
        foreach (SliderButton button in GetComponentsInChildren<SliderButton>())
            button.ParentPanel = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void IncrementPage()
    {
        ChannelBase += ChannelsPerPage;

        if (ChannelBase + ChannelsPerPage > GlobalScript.maxDmxIndex)
            ChannelBase = (GlobalScript.maxDmxIndex - ChannelsPerPage) + 1;

        UpdateAllSliders();
    }

    public void DecrementPage()
    {
        ChannelBase -= ChannelsPerPage;

        if (ChannelBase < GlobalScript.minDmxIndex)
            ChannelBase = GlobalScript.minDmxIndex;

        UpdateAllSliders();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Unhide(SharpyBaseScript light)
    {
        gameObject.SetActive(true);
        CurrentLight = light;

        if (light != null)
        {
            ChannelBase = light.startChannel;

            SolverHandler solver = GetComponentInChildren<SolverHandler>();
            solver.TransformTarget = light.transform;
        }

        UpdateAllSliders();
    }

    private void UpdateAllSliders()
    {
        foreach (DmxSliderControl slider in GetComponentsInChildren<DmxSliderControl>())
        {
            slider.SetSliderValue(GlobalScript.dmxUniverse[slider.GetChannel()]);
            slider.UpdateChannelLabel();
        }
    }
}
