using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharpyBeamScript : MonoBehaviour {

    private int colorChannel;   // +0: the channel we're referencing to denote color of beam
    private int strobeChannel;  // +1: the channel we're referencing to denote strobe
    private int dimmerChannel;  // +2: the channel we're referencing to denote dimmer
    private int zoomChannel;    // +3: the channel we're referencing to denote zoom
    private int prismChannel;   // +4: the channel we're referencing to denote number of beams
    private int prismAngleChan; // +5: the channel we're referencing to denote angle of beams
    private int prismRotation;  // +6: the channel we're referencing to denote rotation speed of beams
    private float lightAlpha;   // current alpha of light (based on dimmer and strobe channels)
    private bool strobing;      // bool to know whether or not to call StrobeLight coroutine
    private float rotationSpeed;
    private bool rotating;
    private int prevPrisms;
    private float prevPrismAngle;
    private GameObject pivot;
    private GlobalScript gs;

    // 0:   white
    // 9:   red 
    // 18:  orange
    // 26:  aquamarine
    // 35:  green
    // 43:  light green
    // 52:  lavender
    // 60:  pink
    // 69:  yellow
    // 77:  magenta
    // 86:  cyan
    // 94:  cto 260
    // 103: cto 190
    // 111: cto 8000
    // 120: blue
    // 128: rotation (0.2 rpm - 160 rpm)
    static public Color[] sharpyColor = new Color[128];

    // set up color array
    static SharpyBeamScript()
    {
        int i = 0;
        while (i < 9)
        {
            sharpyColor[i] = Color.white;
            i++;
        }
        while (i < 18)
        {
            sharpyColor[i] = Color.red;
            i++;
        }
        Color orange = Color.red + Color.yellow;
        while (i < 26)
        {
            sharpyColor[i] = orange;
            i++;
        }
        Color aquamarine = new Color(127f / 255, 1, 212f / 255, 1);
        while (i < 35)
        {
            sharpyColor[i] = aquamarine;
            i++;
        }
        while (i < 43)
        {
            sharpyColor[i] = Color.green;
            i++;
        }
        Color lightGreen = Color.green + Color.yellow;
        while (i < 52)
        {
            sharpyColor[i] = lightGreen;
            i++;
        }
        Color lavender = new Color(230f / 255, 230f / 255, 250f / 255, 1);
        while (i < 60)
        {
            sharpyColor[i] = lavender;
            i++;
        }
        Color pink = Color.red + Color.white;
        while (i < 69)
        {
            sharpyColor[i] = pink;
            i++;
        }
        while (i < 77)
        {
            sharpyColor[i] = Color.yellow;
            i++;
        }
        while (i < 86)
        {
            sharpyColor[i] = Color.magenta;
            i++;
        }
        while (i < 94)
        {
            sharpyColor[i] = Color.cyan;
            i++;
        }
        Color cto260 = new Color(218f / 255, 165f / 255, 32f / 255, 1);
        while (i < 103)
        {
            sharpyColor[i] = cto260;
            i++;
        }
        Color cto190 = new Color(238f / 255, 221f / 255, 130f / 255, 1);
        while (i < 111)
        {
            sharpyColor[i] = cto190;
            i++;
        }
        Color cto8000 = new Color(118f / 255, 238f / 255, 198f / 255, 1);
        while (i < 120)
        {
            sharpyColor[i] = cto8000;
            i++;
        }
        while (i < 128)
        {
            sharpyColor[i] = Color.blue;
            i++;
        }
    }

    // Use this for initialization
    void Start()
    {
        colorChannel = transform.GetComponentInParent<SharpyBaseScript>().GetStartChannel();
        strobeChannel = colorChannel + 1;
        dimmerChannel = colorChannel + 2;
        zoomChannel = colorChannel + 3;
        prismChannel = colorChannel + 4;
        prismAngleChan = colorChannel + 5;
        prismRotation = colorChannel + 6;
        strobing = false;
        rotating = false;
        prevPrisms = 0;
        prevPrismAngle = 0;
        gs = GameObject.FindWithTag("Global").GetComponent<GlobalScript>();
    }

    // Update is called once per frame
    void Update ()
    {
        // determine if we need to start the strobing coroutine
        int strobeVal = GlobalScript.dmxUniverse[strobeChannel];
        bool newStrobing = !(strobeVal < 4) && !(strobeVal > 251) && !(strobeVal > 103 && strobeVal < 108);
        if (!strobing && newStrobing)
        {
            strobing = newStrobing;
            StartCoroutine(StrobeLight(setLightAlpha));
        }
        else if (strobing && !newStrobing)
        {
            lightAlpha = GlobalScript.dmxUniverse[dimmerChannel] / 255f;
            strobing = newStrobing;
            StopCoroutine(StrobeLight(setLightAlpha));
        }
        else if (!strobing)
        {
            lightAlpha = GlobalScript.dmxUniverse[dimmerChannel] / 255f;
        }

        // get zoom and prism values
        int prisms = 1 + GlobalScript.dmxUniverse[prismChannel] / 36;
        float zoomAngle = 1 + GlobalScript.dmxUniverse[zoomChannel] / 255f * 45 / prisms;
        float prismAngle = 1 + GlobalScript.dmxUniverse[prismAngleChan] / 255f * 22;

        if (prisms == 1)
            prismAngle = 0;

        // if prisms change, update prisms
        if (prisms != prevPrisms || prismAngle != prevPrismAngle)
            updatePrisms(prisms, prismAngle);
        prevPrisms = prisms;
        prevPrismAngle = prismAngle;

        // if zoom changes, update zoom
        updateZoom(zoomAngle);

        // determine if we need to start the rotation coroutine
        int rotVal = GlobalScript.dmxUniverse[prismRotation];
        bool newRotating = rotVal != 0 && rotVal != 255 && !(rotVal > 124 && rotVal < 131);
        if (!rotating && newRotating && prisms != 1)
        {
            rotating = newRotating;
            StartCoroutine(updateRotation(setRotationSpeed));
        }
        else if ((rotating && !newRotating) || prisms == 1)
        {
            rotating = newRotating;
            StopCoroutine(updateRotation(setRotationSpeed));
        }

        // update color
        UpdateColor(GlobalScript.dmxUniverse[colorChannel]);
    }

    public void setRotationSpeed(float speed)
    {
        rotationSpeed = speed;
        if (rotationSpeed == 0)
        {
            rotating = false;
            return;
        }

        // rotate the beams
        pivot.transform.Rotate(new Vector3(0, 0, rotationSpeed));
    }

    // 
    private IEnumerator updateRotation(Action<float> rotationCallback)
    {
        while (rotating)
        {
            // update rotation speed
            int rotVal = GlobalScript.dmxUniverse[prismRotation];
            rotating = rotVal != 0 && rotVal != 255 && !(rotVal > 124 && rotVal < 131);
            if (rotating)
            {
                if (rotVal < 128)
                    rotationSpeed = 1 + (rotVal - 1) / 123f * 29;
                else
                    rotationSpeed = -1 - (254 - rotVal) / 123f * 29;
            }
            else
                rotationSpeed = 0;

            // set new rotation speed
            rotationCallback(rotationSpeed);
            yield return null;
        }
    }

    // handle prism logic
    private void updatePrisms(int prisms, float prismAngle)
    {
        if (pivot != null)
            Destroy(pivot);
        float rad = (prisms == 1) ? 0.1f : 0.01f;
        pivot = Instantiate(gs.empty, gameObject.GetComponent<SharpyHeadScript>().transform);
            for (int i = 0; i < prisms; i++)
            {
                GameObject b = Instantiate(gs.beam, pivot.transform);
                b.GetComponent<VLB.VolumetricLightBeam>().coneRadiusStart = rad;
                b.transform.Rotate(prismAngle, 0, 0);
                b.transform.RotateAround(pivot.transform.position, pivot.transform.forward, 360f / prisms * i);
            }
        pivot.transform.position = gameObject.GetComponent<SharpyHeadScript>().transform.position;
    }

    // handle zoom logic
    private void updateZoom(float zoomAngle)
    {
        // set color of the material to a gradient between the colors as listed above
        Light[] lights = GetComponentsInChildren<Light>();

        foreach (var light in lights)
        {
            VLB.VolumetricLightBeam beam = light.GetComponentInChildren<VLB.VolumetricLightBeam>();
            light.spotAngle = zoomAngle;
            beam.UpdateAfterManualPropertyChange();
        }
    }

    // handle color changes
    // @REACH: true color wheel implementation
    private void UpdateColor(int c)
    {
        // set color of the material to a gradient between the colors as listed above
        Light[] lights = GetComponentsInChildren<Light>();

        foreach (var light in lights)
        {
            VLB.VolumetricLightBeam beam = light.GetComponentInChildren<VLB.VolumetricLightBeam>();

            // single color
            if (c < 128)
            {
                // set the color of the beam
                Color set = sharpyColor[c];
                light.color = new Color(set.r, set.g, set.b, lightAlpha);
                beam.UpdateAfterManualPropertyChange();

                // COMMENTED OUT: code for blending, but that's not what we really want
                //int colorIndex = c / 9; // which color a we're mixing;
                //int step = c % 9; // step between color a and b
                //float mix = step / 9f; // the mix between color a and b

                //// get the colors
                //Color a = sharpyColor[colorIndex];
                //Color b;

                //// wrap around for color b
                //if (colorIndex < 14)
                //    b = sharpyColor[colorIndex + 1];
                //else
                //    b = sharpyColor[0];

                //// set the material's color
                //Color mixed = Color.Lerp(a, b, mix);
                //rend.material.color = new Color (mixed.r, mixed.g, mixed.b, 0.5f);

                //// may use this mixing instead if not satisfied with Lerp color mixing above
                //// Color mixed = LerpHSV(a, b, mix);
                //// rend.material.color = new Color (mixed.r, mixed.g, mixed.b, 0.5f);
            }
            // rotating color wheel
            else
            {
                // rotate colors
            }
        }
    }

    public void setLightAlpha(float alpha)
    {
        lightAlpha = alpha;
    }

    private IEnumerator StrobeLight(Action<float> alphaCallback)
    {
        // strobe and dimmer values
        int strobeVal;
        float dimmer;
        bool on = true;

        // strobe logic
        while (strobing)
        {
            // update strobe and dimmer values
            strobeVal = GlobalScript.dmxUniverse[strobeChannel];
            strobing = !(strobeVal < 4) && !(strobeVal > 251) && !(strobeVal > 103 && strobeVal < 108);
            dimmer = GlobalScript.dmxUniverse[dimmerChannel] / 255f;

            // convert strobeVal to a percentage based on range from 4-103
            float strobeRate = 1 + (strobeVal - 4) * 11f / 99;

            // max strobe is 12 flashes/sec
            float halfPeriod = 1f / (strobeRate * 2); // * 2 because half the second it's on and half it's off

            // set the new on/off state
            alphaCallback((on) ? dimmer : 0);
            on = !on;
            yield return new WaitForSecondsRealtime(halfPeriod);
        }
    }

    // interpolate color mix in HSV
    public static Color LerpHSV(Color a, Color b, float t)
    {
        float aH, aS, aV;
        float bH, bS, bV;
        Color.RGBToHSV(a, out aH, out aS, out aV);
        Color.RGBToHSV(b, out bH, out bS, out bV);
        float h, s;

        if (aV == 0)
        {
            h = bH;
            s = bS;
        }
        else if (bV == 0)
        {
            h = aH;
            s = aS;
        }
        else
        {
            if (aS == 0)
                h = bH;
            else if (bS == 0)
                h = aH;
            else
            {
                float angle = Mathf.LerpAngle(aH * 360f, bH * 360f, t);
                while (angle < 0f)
                    angle += 360f;
                while (angle > 360f)
                    angle -= 360f;
                h = angle / 360f;
            }
            s = Mathf.Lerp(aS, bS, t);
        }
        return Color.HSVToRGB(h, s, Mathf.Lerp(aV, bV, t));
    }
}
