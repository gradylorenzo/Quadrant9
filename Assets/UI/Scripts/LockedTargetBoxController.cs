﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Q9Core;
using Q9Core.CommonData;

public class LockedTargetBoxController : MonoBehaviour
{
    public bool isVisible;
    public int LockedTargetSlot;
    public Image Thumbnail;
    public Image ShieldBar;
    public Image IntegrityBar;
    public Image ActiveTargetSpinner;
    public GameObject graphic;
    public Text ObjectName;
    public Text ObjectDistance;

    private ShipController _playerShip;
    private GameObject _target;

    public void OnClick()
    {
        if(LockedTargetSlot < _playerShip._lockedTargets.Count)
        {
            EventManager.OnObjectSelected(_playerShip._lockedTargets[LockedTargetSlot]._target, false);
        }
    }

    private void Update()
    {
        if (_playerShip == null)
        {
            if (GameManager._playerShip != null)
            {
                _playerShip = GameManager._playerShip;
            }
        }

        else
        {
            if (LockedTargetSlot < _playerShip._lockedTargets.Count)
            {
                if (_playerShip._lockedTargets[LockedTargetSlot]._lockComplete)
                {
                    isVisible = true;
                    //Active Target Spinner
                    if (_playerShip._activeTarget == _playerShip._lockedTargets[LockedTargetSlot]._target)
                    {
                        ActiveTargetSpinner.enabled = true;
                    }
                    else
                    {
                        ActiveTargetSpinner.enabled = false;
                    }
                    Thumbnail.sprite = _playerShip._lockedTargets[LockedTargetSlot]._target.GetComponent<Q9Entity>()._overview._thumbnail;
                    ObjectName.text = _playerShip._lockedTargets[LockedTargetSlot]._target.GetComponent<Q9Entity>()._overview._name;
                    ObjectDistance.text =
                        Vector3.Distance(_playerShip._lockedTargets[LockedTargetSlot]._target.transform.position, Vector3.zero).ToString("###") + " km";

                    //Health bars
                }
                _target = _playerShip._lockedTargets[LockedTargetSlot]._target;
            }
            else
            {
                isVisible = false;
                ActiveTargetSpinner.enabled = false;
                _target = null;
            }
        }
        graphic.SetActive(isVisible);

        //Meter updates
        if (_target != null)
        {
            float shieldFill = 1;
            float integrityFill = 1;
            if (_target.GetComponent<ShipController>())
            {
                shieldFill = (1 / (_target.GetComponent<ShipController>().modifiedAttributes._shield._capacity / _target.GetComponent<ShipController>().currentAttributes._shield._capacity));
                integrityFill = (1 / (_target.GetComponent<ShipController>().modifiedAttributes._integrity._capacity / _target.GetComponent<ShipController>().currentAttributes._integrity._capacity));
                
            }
            ShieldBar.fillAmount = shieldFill * .75f;
            IntegrityBar.fillAmount = integrityFill * .75f;
        }
    }
}
