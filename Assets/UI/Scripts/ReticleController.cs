using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Q9Core;

public class ReticleController : MonoBehaviour {

    [System.Serializable]
    public class Reticle
    {
        public Image _reticle;
        public Image _markerUp;
        public Image _markerDown;
        public Image _markerLeft;
        public Image _markerRight;
        public Image _timer;
        public Image _spinner;
    }
    public Reticle _reticle;
    public int LockedTargetSlot;

    private ShipController playerShip;
    private GameObject Target;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (GameManager._playerShip != null)
        {
            int t;
            t = GameManager._playerShip._lockedTargets.Count;

            if (LockedTargetSlot < t)
            {
                Target = GameManager._playerShip._lockedTargets[LockedTargetSlot]._target;
                _reticle._timer.enabled = !GameManager._playerShip._lockedTargets[LockedTargetSlot]._lockComplete;
                if (!GameManager._playerShip._lockedTargets[LockedTargetSlot]._lockComplete)
                {
                    float time, start;
                    time = GameManager._playerShip._lockedTargets[LockedTargetSlot]._lockTime;
                    start = GameManager._playerShip._lockedTargets[LockedTargetSlot]._lockStart;
                    _reticle._timer.fillAmount = (1 - ((time - (Time.time - start)) / time));
                    _reticle._timer.enabled = true;
                }
                else
                {
                    _reticle._timer.fillAmount = 0;
                    _reticle._timer.enabled = false;
                }

                _reticle._reticle.enabled = true;
                _reticle._markerUp.enabled = true;
                _reticle._markerDown.enabled = true;
                _reticle._markerLeft.enabled = true;
                _reticle._markerRight.enabled = true;
                _reticle._spinner.enabled = (Target == GameManager._playerShip._activeTarget);
            }
            else
            {
                Target = null;

                _reticle._reticle.enabled = false;
                _reticle._markerUp.enabled = false;
                _reticle._markerDown.enabled = false;
                _reticle._markerLeft.enabled = false;
                _reticle._markerRight.enabled = false;
                _reticle._spinner.enabled = false;
                _reticle._timer.enabled = false;
            }

            if (Target != null)
            {
                //This currently does not work perfectly. The desired result is for the reticle to stay at the edge of the screen if the target
                //is off screen. Unfortunately, WorldToScreenPoint returns on-screen coordinates if the object is behind, when I want it to
                //still stay at the edge, result in the reticle being "reflected". For now, the coordinates are simply clamped to the edge until
                //something better can be written.
                Vector3 wtsp = cam.WorldToScreenPoint(Target.transform.position);
                
                if (wtsp.z < 0)
                {
                    /*Fix this trash
                    bool lockX_L = false;
                    bool lockX_R = false;
                    bool lockY_U = false;
                    bool lockY_D = false;
                    float x, y;
                    
                    //Determine X limit locks
                    if(wtsp.x < 0)
                    {
                        lockX_L = true;
                        lockX_R = false;
                    }
                    else if(wtsp.x > Screen.width)
                    {
                        lockX_L = false;
                        lockX_R = true;
                    }
                    else
                    {
                        lockX_L = false;
                        lockX_R = false;
                    }

                    //Determine Y limit locks
                    if(wtsp.y < 0)
                    {
                        lockY_D = true;
                        lockY_U = false;
                    }
                    else if (wtsp.y > Screen.height)
                    {
                        lockY_D = false;
                        lockY_U = true;
                    }
                    else
                    {
                        lockY_D = false;
                        lockY_U = false;
                    }

                    //Lock cases
                    if (lockX_L)
                    {
                        x = Screen.width;
                        if (lockY_D)
                        {
                            y = 0;
                        }
                        else if (lockY_U)
                        {
                            y = Screen.height;
                        }
                        else
                        {
                            y = wtsp.y;
                        }
                    }

                    if (lockX_R)
                    {
                        x = 0;
                        if (lockY_D)
                        {
                            y = 0;
                        }
                        else if (lockY_U)
                        {
                            y = Screen.height;
                        }
                        else
                        {
                            y = wtsp.y;
                        }
                    }

                    else
                    {
                        x = wtsp.x;
                        if(lockY_D)
                        {
                            y = 0;
                        }
                        else if (lockY_U)
                        {
                            y = Screen.height;
                        }
                        else
                        {
                            //This should never happen.
                            y = wtsp.y;
                        }
                    }
                    
                    transform.position = new Vector3(x, y, 0);
                    */

                    
                }
                else
                {
                    transform.position = new Vector3(Mathf.Clamp(wtsp.x, 0, Screen.width), Mathf.Clamp(wtsp.y, 0, Screen.height), 0);
                }
            }
        }
    }
}
