using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    [SerializeField]
    Slider musicSlider;

    [SerializeField]
    Slider effectSlider;

    [SerializeField]
    Toggle soundOn;

    AudioSource musicSound;
    EffectSound[] effectSounds;

    public SoundData data;
    private void Awake()
    {
        musicSound = GameObject.FindWithTag("BackGroundMusic").GetComponent<AudioSource>();

    }
    private void Start()
    {
        effectSlider.value = data.effectScale;
        musicSlider.value = data.bgmScale;
        soundOn.isOn = !data.isMute;
        gameObject.SetActive(false);
    }
    public void SetMusicSlider()
    {
        musicSound.volume = musicSlider.value;
    }
    public void SetEffectSlider()
    {
        var soundValue = effectSlider.value;
        foreach (var effect in effectSounds)
        {
            effect.volume = soundValue;
        }
    }
    public void SetSoundOn()
    {
        bool isState = soundOn.isOn;

        musicSound.mute = !isState;
        foreach (var effect in effectSounds)
        {
            effect.mute = !isState;
        }
    }
    public void OnClickQuit()
    {
        Application.Quit();
    }

    private void OnEnable()
    {
        effectSounds = FindObjectsOfType<EffectSound>();
        Time.timeScale = 0;
    }
    private void OnDisable()
    {
        Time.timeScale = 1;
    }
}
