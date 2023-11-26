using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class OptionsMenuBehaviour : MonoBehaviour
{
    //public AudioMixer audioMixer;

    public TMP_Dropdown resolutionDropDown;

    List<string> options = new List<string>();

    void Start()
    {
        resolutionDropDown.ClearOptions();
        foreach (Resolution option in Screen.resolutions)
        {
            options.Add(option.width + " x " + option.height);
        }
        resolutionDropDown.AddOptions(options);
    }

    public void SetResolution(int resolutionIndex)
    {

        Screen.SetResolution(Screen.resolutions[resolutionIndex].width, Screen.resolutions[resolutionIndex].height, Screen.fullScreen);
    }

    /*public void SetVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", 80f*(volume-1f));
    }*/

    /*public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }*/

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
