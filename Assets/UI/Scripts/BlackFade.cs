using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackFade : MonoBehaviour {

	public Color[] _presets;
    public Image _screen;
    public float _fadeSpeed;

    private Color _wantedColor;
    private Color _currentColor;
    private bool ready = false;

    private void Awake()
    {
        EventManager.OnGameIsReady += onGameIsReady;
    }

    private void onGameIsReady()
    {
        ready = true;
    }

    private void Start()
    {
        _currentColor = _presets[0];
        _wantedColor = _presets[1];
    }

    private void FixedUpdate()
    {
        if (ready)
        {
            float r = Mathf.MoveTowards(_currentColor.r, _wantedColor.r, _fadeSpeed);
            float g = Mathf.MoveTowards(_currentColor.g, _wantedColor.g, _fadeSpeed);
            float b = Mathf.MoveTowards(_currentColor.b, _wantedColor.b, _fadeSpeed);
            float a = Mathf.MoveTowards(_currentColor.a, _wantedColor.a, _fadeSpeed);

            _currentColor = new Color(r, g, b, a);

            _screen.color = _currentColor;
        }
    }
}
