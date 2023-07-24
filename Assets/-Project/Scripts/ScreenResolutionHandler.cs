using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScreenResolutionHandler : MonoBehaviour
{
    TMP_Dropdown dropDown;

    Resolution[] resolutions;
    List<Resolution> filteredResolutions;

    float currentRefreshRate;
    int currentResolutionIndex = 0;

    // Called by Dropdown Menu component
    public void OnValueChanged(int index)
    {
        switch (index)
        {
            case 0: break;
        }
    }

    void OnEnable() { }

    void OnDisable() { }

    void Awake()
    {
        dropDown = GetComponent<TMP_Dropdown>();
    }

    [System.Obsolete]
    void Start()
    {
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        dropDown.ClearOptions();
        currentRefreshRate = Screen.currentResolution.refreshRate;

        foreach (Resolution resolution in resolutions)
        {
            float aspectRatio = (float)resolution.width / resolution.height;
            if (Mathf.Approximately(aspectRatio, 16f / 9f))
            {
                if (resolution.refreshRate == currentRefreshRate)
                {
                    filteredResolutions.Add(resolution);
                }
            }
        }

        if (filteredResolutions.Count >= 3)
        {
            // Display only 16:9 resolutions if there are 3 or more
            List<string> options = new List<string>();
            for (int i = 0; i < filteredResolutions.Count; i++)
            {
                string resolutionOption = filteredResolutions[i].width + "x" + filteredResolutions[i].height + " " + filteredResolutions[i].refreshRate + " Hz";
                options.Add(resolutionOption);

                if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
                    currentResolutionIndex = i;
            }

            dropDown.AddOptions(options);
        }
        else
        {
            // Display all resolutions if there are less than 3 16:9 resolutions
            List<string> options = new List<string>();
            for (int i = 0; i < resolutions.Length; i++)
            {
                string resolutionOption = resolutions[i].width + "x" + resolutions[i].height + " " + resolutions[i].refreshRate + " Hz";
                options.Add(resolutionOption);

                if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
                    currentResolutionIndex = i;
            }

            dropDown.AddOptions(options);
        }

        dropDown.value = currentResolutionIndex;
        dropDown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution;
        if (filteredResolutions.Count >= 3)
        {
            // Use 16:9 resolution if there are 3 or more
            resolution = filteredResolutions[resolutionIndex];
        }
        else
        {
            // Use any resolution if there are less than 3 16:9 resolutions
            resolution = resolutions[resolutionIndex];
        }

        Screen.SetResolution(resolution.width, resolution.height, true);
    }
}
