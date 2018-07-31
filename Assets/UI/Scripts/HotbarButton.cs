using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Q9Core;
using Q9Core.CommonData;


public class HotbarButton : MonoBehaviour
{
    public KeyCode _key;
    public Image _timer;
    public Image _icon;
    public Color ActiveColor;
    public Color DeactiveColor;
    public Q9Module.Slots _slots;
    public int _mod;
    public Q9Module _targetModule;


    public ShipController _playerShip;

    public void OnClick()
    {
        if (_targetModule)
        {
            if (_targetModule.isActivated)
            {
                _targetModule.Deactivate();
            }
            else
            {
                _targetModule.Activate(_playerShip.gameObject, _playerShip._activeTarget);
            }
        }
    }

    public void ResetTargetModule()
    {
        if (_playerShip)
        {
            switch (_slots)
            {
                case Q9Module.Slots.High:
                    if(_playerShip.currentAttributes._fitting._highSlots.Length > _mod)
                    {
                        if (_playerShip.currentAttributes._fitting._highSlots[_mod] != null)
                        {
                            _targetModule = _playerShip.currentAttributes._fitting._highSlots[_mod];
                            gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                case Q9Module.Slots.Mid:
                    if (_playerShip.currentAttributes._fitting._midSlots.Length > _mod)
                    {
                        if (_playerShip.currentAttributes._fitting._midSlots[_mod] != null)
                        {
                            _targetModule = _playerShip.currentAttributes._fitting._midSlots[_mod];
                            gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                case Q9Module.Slots.Low:
                    if (_playerShip.currentAttributes._fitting._lowSlots.Length > _mod)
                    {
                        if (_playerShip.currentAttributes._fitting._lowSlots[_mod] != null)
                        {
                            _targetModule = _playerShip.currentAttributes._fitting._lowSlots[_mod];
                            gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                case Q9Module.Slots.Rig:
                    print("Error: Buttons not allowed to control modules in rig slots.");
                    break;
            }
        }
        else
        {
            print("No Player Ship");
        }

        if (_targetModule)
        {
            _icon.sprite = _targetModule._thumbnail;
        }
    }

    public void Update()
    {
        if(_timer && _targetModule)
        {    
            if (_targetModule.isActivated)
            {
                _timer.fillAmount = (1 - ((_targetModule._cooldown - (Time.time - _targetModule._lastCycle)) / _targetModule._cooldown));
                if (_targetModule.isQueuedToDeactivate)
                {
                    _timer.color = DeactiveColor;
                }
                else
                {
                    _timer.color = ActiveColor;
                }
            }
            else
            {
                _timer.fillAmount = 0;
            }
        }
        else
        {
            _timer.fillAmount = 0;
        }

        if (Input.GetKeyDown(_key))
        {
            OnClick();
        }
    }
}
