using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusRingController : MonoBehaviour {

    public Image ShieldBar;
    public Image IntegrityBar;
    public Image CapacitorBar;
    public Image SpeedBar;

    public Text ShieldPercentage;
    public Text IntegrityPercentage;
    public Text CapacitorPercentage;
    public Text SpeedRead;

    private float shieldFill = 1;
    private float integrityFill = 1;
    private float capacitorFill = 1;
    private float nextUpdate = 0;
    private float speedFill = 0;

    #region public methods
    public void Reset()
    {
        shieldFill = 1;
        integrityFill = 1;
        capacitorFill = 1;
    }

    public void SetShield(float maxShield, float currentShield)
    {
        shieldFill = (1 / (maxShield / currentShield));
    }

    public void SetIntegrity(float maxInt, float currentInt)
    {
        integrityFill = (1 / (maxInt / currentInt));
    }

    public void SetCapacitorFill(float maxCap, float currentCap)
    {
        capacitorFill = (1 / (maxCap / currentCap));
    }
    #endregion

    private void Start()
    {
        Reset();
    }

    private void Update()
    {
        UpdateMeters();
    }

    private void UpdateMeters()
    {
        //Update Status Ring meters once per second. Timing may be managed later by UIManager through Set..() calls.
        ShieldBar.fillAmount = Mathf.MoveTowards(ShieldBar.fillAmount, shieldFill * .75f, .01f);
        IntegrityBar.fillAmount = Mathf.MoveTowards(IntegrityBar.fillAmount, integrityFill * .75f, .01f);
        CapacitorBar.fillAmount = Mathf.MoveTowards(CapacitorBar.fillAmount, capacitorFill * .75f, .01f);

        //
        if (shieldFill > 0)
        {
            ShieldPercentage.text = (Mathf.Clamp01(shieldFill) * 100).ToString("###") + "%";
        }
        else
        {
            ShieldPercentage.text = "0%";
        }
        //
        if (integrityFill > 0)
        {
            IntegrityPercentage.text = (Mathf.Clamp01(integrityFill) * 100).ToString("###") + "%";
        }
        else
        {
            IntegrityPercentage.text = "0%";
        }
        //
        if (capacitorFill > 0)
        {
            CapacitorPercentage.text = (Mathf.Clamp01(capacitorFill) * 100).ToString("###") + "%";
        }
        else
        {
            CapacitorPercentage.text = "0%";
        }

        //
        speedFill = (GameManager._playerShip.GetComponent<ShipManager>().currentThrottle / GameManager._playerShip.GetComponent<ShipManager>().currentAttributes._travel._burnSpeed) * .22f;
        SpeedBar.fillAmount = speedFill;
        if (speedFill > 0)
        {
            SpeedRead.text = ((GameManager._playerShip.GetComponent<ShipManager>().currentThrottle) * 1000).ToString("###") + " m/s";
        }
        else
        {
            SpeedRead.text = "0 m/s";
        }
    }
}
