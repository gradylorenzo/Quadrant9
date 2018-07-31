using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;
using UnityEngine.UI;

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

	private void Start()
	{
		EventManager.OnObjectSelected += OnObjectSelected;
		EventManager.OnObjectDestroyed += OnObjectDestroyed;
		SetData(defaultData);
	}

	private void OnObjectSelected(GameObject go)
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

	private void SetData (Q9OverviewData d)
	{
		print("Selected Item set to " + d._name);
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
            Jump.interactable = q9e._isBridging;
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
        }
        else
        {
            Warp.interactable = false;
        }
	}

	public void OnTargetClicked()
	{
		print("Target Button Clicked");
        EventManager.OnObjectLocked(currentData._go);
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
		print("Warp Button Clicked, internal method not yet implemented");
        EventManager.OnObjectSelectedAsAlignmentTarget(currentData._go, true);
    }

	public void OnDockClicked()
	{
		print("Dock Button Clicked, not yet implemented");
	}
	public void OnJumpClicked()
	{
		print("Jump Button Clicked, not yet implemented");
	}
}
