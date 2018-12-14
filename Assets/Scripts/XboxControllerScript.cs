
using System;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;
using UnityEngine.UI;

public class XboxControllerScript : XboxControllerHandlerBase
{
    private enum DMXChannels
    {
        Red,
        Green,
        Blue,
        Strobe,
        Dimmer,
        Pan,
        Tilt
    }

    private class MenuOptions
    {
        public string name;
        public float value;

        public MenuOptions(string n, float val)
        {
            name = n;
            value = val;
        }
    }

    [Header("Xbox Controller Test Options")]
    
    [SerializeField] private float sliderSpeedMultiplier = 0.5f;

    [SerializeField] private GameObject LightUx = null;

    // Kept as a key value pair to display text easily
    [SerializeField] private List<MenuOptions> menuOptions;
    [SerializeField] private int menuIterator;
    [SerializeField] private int dmxChannelCount;

    protected virtual void Start()
    {
        menuIterator = 0;
        dmxChannelCount = Enum.GetNames(typeof(DMXChannels)).Length;
        menuOptions = new List<MenuOptions>(dmxChannelCount);
        var enumList = Enum.GetNames(typeof(DMXChannels));

        for (int i = 0; i < dmxChannelCount; ++i)
        {
            menuOptions.Add(new MenuOptions(enumList[i], 0.0f));
        }
    }

    public override void OnSourceLost(SourceStateEventData eventData)
    {
        Debug.LogFormat("Joystick {0} with id: \"{1}\" Disconnected", GamePadName, eventData.SourceId);
        base.OnSourceLost(eventData);
        // channelText.text = "No Controller Connected";
    }

    public override void OnXboxInputUpdate(XboxControllerEventData eventData)
    {
        // Debug.Log("hahahah");
        // GameObject.FindGameObjectWithTag("CurrentSelectedTag").a

        //if (string.IsNullOrEmpty(GamePadName))
        //{
        //    Debug.LogFormat("Joystick {0} with id: \"{1}\" Connected", eventData.GamePadName, eventData.SourceId);
        //}

        //base.OnXboxInputUpdate(eventData);
        
        //if (eventData.XboxRightBumper_Down)
        //{
        //    menuIterator = (menuIterator + 1) % dmxChannelCount;
        //}
        //if (eventData.XboxLeftBumper_Down)
        //{
        //    menuIterator = (menuIterator + dmxChannelCount - 1) % dmxChannelCount;
        //}

        //if (eventData.XboxA_Down)
        //{
        //    GameObject.FindGameObjectWithTag("CurrentSelectedTag").GetComponent<MeshRenderer>().enabled = true;
        //}

        //menuOptions[menuIterator].value += eventData.XboxRightStickHorizontalAxis * sliderSpeedMultiplier;
        //menuOptions[menuIterator].value = Math.Min(menuOptions[menuIterator].value, 1.0f);
        //menuOptions[menuIterator].value = Math.Max(menuOptions[menuIterator].value, 0.0f);
    }
}