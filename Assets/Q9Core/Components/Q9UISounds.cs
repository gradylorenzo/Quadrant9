using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Q9UISounds : MonoBehaviour {

    public AudioClip[] _clips;

    public AudioSource _source;
    public AudioSource _secondarySource;

	void Start ()
    {
        EventManager.OnTargetLockComplete += OnTargetLockComplete;
    }

    void OnTargetLockComplete()
    {
        _source.PlayOneShot(_clips[0]);
    }

    private void Update()
    {
        if (EventManager.isPlayerLocking)
        {
            if (!_secondarySource.isPlaying)
            {
                _secondarySource.clip = _clips[1];
                _secondarySource.Play();
            }
        }
        else
        {
            if (_secondarySource.isPlaying)
            {
                _secondarySource.Stop();
            }
        }
    }
}
