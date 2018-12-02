using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    private const string OnIconPath = "Images/SoundOn";
    private const string OffIconPath = "Images/SoundOff";

    private void Start()
    {
        AudioListener.pause = PlayerPrefs.GetInt(PlayerPrefsKeys.IsSoundOn) != 0;
        if (AudioListener.pause)
        {
            GetComponent<Image>().sprite = Resources.Load<Sprite>(OffIconPath);
        }
        else
        {
            GetComponent<Image>().sprite = Resources.Load<Sprite>(OnIconPath);
        }
    }

    public void SoundOnOff()
    {
        AudioListener.pause = !AudioListener.pause;
        PlayerPrefs.SetInt(PlayerPrefsKeys.IsSoundOn, AudioListener.pause ? 1 : 0);
        if (AudioListener.pause)
        {
            GetComponent<Image>().sprite = Resources.Load<Sprite>(OffIconPath);
        }
        else
        {
            GetComponent<Image>().sprite = Resources.Load<Sprite>(OnIconPath);
        }
    }
}
