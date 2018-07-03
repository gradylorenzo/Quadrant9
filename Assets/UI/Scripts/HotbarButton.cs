using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Q9Core;
using Q9Core.CommonData;


public class HotbarButton : MonoBehaviour
{
    public Image _timer;
    public Color ActiveColor;
    public Color DeactiveColor;
    public Q9Module.Slots _slots;
    public int _mod;
    public Q9Module _targetModule;


    public ShipManager _playerShip;

    public void OnClick()
    {
        if (_targetModule)
        {
            if (_targetModule._activated)
            {
                _targetModule.Deactivate();
            }
            else
            {
                _targetModule.Activate(_playerShip.gameObject, _playerShip.target);
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
                    if(_playerShip.currentAttributes._fitting.m_highSlots.Length > _mod)
                    {
                        _targetModule = _playerShip.currentAttributes._fitting.m_highSlots[_mod];
                        gameObject.SetActive(true);
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                case Q9Module.Slots.Mid:
                    if (_playerShip.currentAttributes._fitting.m_midSlots.Length > _mod)
                    {
                        _targetModule = _playerShip.currentAttributes._fitting.m_midSlots[_mod];
                        gameObject.SetActive(true);
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                case Q9Module.Slots.Low:
                    if (_playerShip.currentAttributes._fitting.m_lowSlots.Length > _mod)
                    {
                        _targetModule = _playerShip.currentAttributes._fitting.m_lowSlots[_mod];
                        gameObject.SetActive(true);
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
    }

    public void Update()
    {
        if(_timer && _targetModule)
        {    
            if (_targetModule._activated)
            {
                _timer.fillAmount = (Time.time - _targetModule._lastCycle) / (_targetModule._nextCycle - _targetModule._lastCycle);
                //_timer.fillAmount = 0.75f;
                if (_targetModule._queueDeactivation)
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
    }
}
