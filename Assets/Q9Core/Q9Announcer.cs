﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region SVP License
//Covering my ass here.
//Announcer Static Voice Prompts generated by Amazon Polly.
//Used with permission from Amazon Web Services Inc. 2018.
//https://aws.amazon.com/polly/faqs/
//"Q. Can I use the service for generating static voice prompts that will be replayed multiple times?
//Yes, you can. The service does not restrict this and there are no additional costs for doing so."
#endregion

[RequireComponent(typeof(AudioSource))]
public class Q9Announcer : MonoBehaviour {

    public enum VoicePrompts
    {
        InsufficientPower = 0,
        EjectX3 = 1,
        CapitalClassSignatureInbound = 2,
        RequestingDockingClearance = 3,
        ShieldsCritical = 4,
        ShipIntegrityFailing = 5,
        TheCapacitorIsEmpty = 6,
        WarpingToJumpgate = 7,
        WelcomeBackCommander = 8
    }

    public AudioClip[] _clips;
    private List<AudioClip> _queue = new List<AudioClip>();
    private int _queuePosition;
    private AudioSource asource;

    private void Awake()
    {
        Q9GameManager._announcer = this;
        print("Announcer Assigned!");
        asource = GetComponent<AudioSource>();
    }

    public void QueueClip (VoicePrompts p)
    {
        int i = (int)p;
        if (_queue.Count > 0)
        {
            if (_queue[_queue.Count - 1] != _clips[i])
                _queue.Add(_clips[i]);
        }
        else
        {
            _queue.Add(_clips[i]);
        }
    }

    public void Update()
    {
        if (!asource.isPlaying)
        {
            if(_queuePosition < _queue.Count)
            {
                asource.PlayOneShot(_queue[_queuePosition]);
                _queuePosition++;
            }
            else
            {
                _queue.Clear();
                _queuePosition = 0;
            }
        }
    }
}
