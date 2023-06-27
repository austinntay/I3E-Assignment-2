using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider effectsSlider;

    private void Start()
    {
        SetMusicVolume();
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("BGM", Mathf.Log10(volume)*20);
    }


    public void SetEffectsVolume()
    {
        float volume = effectsSlider.value;
        myMixer.SetFloat("Effects", Mathf.Log10(volume) * 20);
    }
}
