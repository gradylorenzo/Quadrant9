using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;
using UnityEngine.UI;
using System;

public class SelectedItemController : MonoBehaviour {

	public Q9OverviewData defaultData;
	public Q9OverviewData currentData;
	public Image Thumbnail;
	public Text Name;
	public Text Type;
	public Text Alliance;
	public Button Target;
	public Button Loot;
	public Button Align;
	public Button Warp;
	public Button Dock;
	public Button Jump;

	private void Awake()
	{
		EventManager.OnObjectSelected += OnObjectSelected;
		EventManager.OnObjectDestroyed += OnObjectDestroyed;
        EventManager.OnSystemChanged += OnSystemChanged;
	}

    

    private void Start()
    {
        SetData(defaultData);
    }

    private void OnObjectSelected(GameObject go, bool forceLock)
	{
		if(go != currentData._go && go != GameManager._playerShip.gameObject)
		{
			SetData(go.GetComponent<Q9Entity>()._overview);
		}
	}

	private void OnObjectDestroyed(bool b, GameObject go)
	{
		if(go == currentData._go)
		{
			SetData(defaultData);
		}
	}
    private void OnSystemChanged(Vector2 dir)
    {
        SetData(defaultData);
    }
    private void SetData (Q9OverviewData d)
	{
		Thumbnail.sprite = d._thumbnail;
		Name.text = d._name;
		Type.text = d._type;
		Alliance.text = d._alliance;
		currentData = d;

        if (d._go != null)
        {
            Q9Entity q9e = d._go.GetComponent<Q9Entity>();
            Target.interactable = q9e._isTargetable;
            Loot.interactable = q9e._isLootable;
            Align.interactable = true;
            Dock.interactable = q9e._isDockable;
        }
        else
        {
            Target.interactable = false;
            Loot.interactable = false;
            Align.interactable = false;
            Dock.interactable = false;
            Jump.interactable = false;
        }
	}

	private void FixedUpdate()
	{
        if (currentData._go != null)
        {
            Warp.interactable = (Vector3.Distance(currentData._go.transform.position, Vector3.zero) > 150);
            Jump.interactable = (currentData._go.GetComponent<Q9Entity>()._isJumpable && (Vector3.Distance(currentData._go.transform.position, Vector3.zero) < 5));
        }
        else
        {
            Warp.interactable = false;
        }
	}

    #region Button methods
    public void OnTargetClicked()
	{
		print("Target Button Clicked");
        EventManager.OnObjectSelected(currentData._go, true);
	}
	public void OnLootClicked()
	{
		print("Loot Button Clicked, not yet implemented");
	}
	public void OnAlignClicked()
	{
		print("Align Button Clicked");
        EventManager.OnObjectSelectedAsAlignmentTarget(currentData._go, false);
	}
	public void OnWarpClicked()
	{
		print("Warp Button Clicked, internal method is work in progress.");
        EventManager.OnObjectSelectedAsAlignmentTarget(currentData._go, true);
    }
	public void OnDockClicked()
	{
		print("Dock Button Clicked, not yet implemented");
	}
	public void OnJumpClicked()
	{
        GameManager._syscon.Jump(currentData._go.GetComponent<Q9Entity>().jumpDirection);
	}
    #endregion
}
