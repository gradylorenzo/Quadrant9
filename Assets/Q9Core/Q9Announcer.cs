using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Q9Announcer : MonoBehaviour {

    public AudioClip[] _clips;
    private List<AudioClip> _queue = new List<AudioClip>();
    private int _queuePosition;
    private AudioSource asource;

    private void Awake()
    {
        Q9GameManager._announcer = this;
        asource = GetComponent<AudioSource>();
    }

    public void QueueClip_InsufficientPower()
    {
        int i = 0;
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
