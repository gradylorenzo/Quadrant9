using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;
public class AudioVolumeController : MonoBehaviour {

    public Settings.volumeTypes Type;

    private float initialVolume;
    private float currentVolume;
    private float wantedVolume;

    private void Start()
    {
        SetVolume();
    }

    private void FixedUpdate()
    {
        currentVolume = Mathf.MoveTowards(currentVolume, wantedVolume, .03f);
        GetComponent<AudioSource>().volume = currentVolume;
    }

    private void SetVolume()
    {
        initialVolume = GetComponent<AudioSource>().volume;
        float f = 0.5f;
        switch (Type)
        {
            case Settings.volumeTypes.sfx:
                f = Settings.currentVolume.sfx;
                break;
            case Settings.volumeTypes.world:
                f = Settings.currentVolume.world;
                break;
            case Settings.volumeTypes.ambience:
                f = Settings.currentVolume.ambience;
                break;
            case Settings.volumeTypes.music:
                f = Settings.currentVolume.music;
                break;
            case Settings.volumeTypes.ui:
                f = Settings.currentVolume.ui;
                break;
            case Settings.volumeTypes.voice:
                f = Settings.currentVolume.voice;
                break;
        }

        currentVolume = 0;
        wantedVolume = initialVolume * f;
    }
}
