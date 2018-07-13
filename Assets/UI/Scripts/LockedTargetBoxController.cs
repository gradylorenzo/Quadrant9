using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Q9Core;
using Q9Core.CommonData;

public class LockedTargetBoxController : MonoBehaviour
{
    public bool isVisible;
    public int LockedTargetSlot;
    public Image ShieldBar;
    public Image IntegrityBar;
    public Image ActiveTargetSpinner;
    public GameObject graphic;

    private ShipManager _playerShip;
    private GameObject _target;

    private void Update()
    {
        if (_playerShip == null)
        {
            if (Q9GameManager._playerShip != null)
            {
                _playerShip = Q9GameManager._playerShip;
            }
        }

        else
        {
            if (LockedTargetSlot < _playerShip._lockedTargets.Count)
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

                //Health bars
            }
            else
            {
                isVisible = false;
                ActiveTargetSpinner.enabled = false;
            }
        }

        graphic.SetActive(isVisible);
    }
}
