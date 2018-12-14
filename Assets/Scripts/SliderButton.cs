using HoloToolkit.Unity.Buttons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SliderButtonType
{
    PrevPage,
    NextPage,
    Exit
}

public class SliderButton : MonoBehaviour
{
    private Button buttonComponent;

    private SliderPanelScript parentPanel;

    private bool buttonPressed;
    
    public SliderPanelScript ParentPanel
    {
        get
        {
            return parentPanel;
        }
        set
        {
            parentPanel = value;
        }
    }
        
    /// <summary>
    /// The type description of the button
    /// </summary>
    public SliderButtonType ButtonType;

    private void OnEnable()
    {
        buttonComponent = GetComponent<Button>();
        buttonComponent.OnButtonClicked += OnButtonClicked;
        buttonComponent.OnButtonReleased += OnButtonReleased;
    }

    private void OnDisable()
    {
        if (buttonComponent != null)
        {
            buttonComponent.OnButtonClicked -= OnButtonClicked;
            buttonComponent.OnButtonReleased -= OnButtonReleased;
        }
    }

    /// <summary>
    /// event handler that runs when button is clicked.
    /// </summary>
    /// <param name="obj"></param>
    public void OnButtonClicked(GameObject obj)
    {
        if (!buttonPressed)
        {
            switch (ButtonType)
            {
                case SliderButtonType.PrevPage:
                    parentPanel.DecrementPage();
                    break;

                case SliderButtonType.NextPage:
                    parentPanel.IncrementPage();
                    break;

                case SliderButtonType.Exit:
                    parentPanel.Hide();
                    break;
            }

            buttonPressed = true;
        }
    }

    public void OnButtonReleased(GameObject obj)
    {
        buttonPressed = false;
    }
}
