using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Q9UISounds : MonoBehaviour {

    public AudioClip[] _clips;

    private AudioSource _source;

	void Start ()
    {
        _source = GetComponent<AudioSource>();
        EventManager.OnTargetLocking += OnTargetLocking;
        EventManager.OnTargetLockComplete += OnTargetLockComplete;
    }

    void OnTargetLocking()
    {

    }

    void OnTargetLockComplete()
    {
        _source.PlayOneShot(_clips[0]);
    }
}
