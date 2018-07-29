using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverviewController : MonoBehaviour {

	public List<Q9OverviewData> _overviewData;
    public List<OverviewRow> _overviewRows;
    public GameObject _overviewRowPrefab;

	private float nextUpdate = 0;

	private void Start()
	{
		EventManager.addOverviewData += addOverviewData;
		EventManager.removeOverviewData += removeOverviewData;
	}

    private void addOverviewData(Q9OverviewData data)
    {
        bool found = false;
        foreach(Q9OverviewData od in _overviewData)
        {
            if(od._guid == data._guid)
            {
                found = true;
            }
        }

        if (!found)
        {
            _overviewData.Add(data);
            Vector3 pos;
            int y = _overviewRows.Count * -20;
            pos = new Vector3(0, y, 0);
            GameObject newOR = Instantiate(_overviewRowPrefab, _overviewRowPrefab.transform.position, _overviewRowPrefab.transform.rotation);
            newOR.transform.parent = this.transform;
			newOR.transform.localPosition = pos;
            _overviewRows.Add(newOR.GetComponent<OverviewRow>());
        }
        else
        {
            print("Duplicate Overview Data found, data was not added");
        }
    }

    private void removeOverviewData(Q9OverviewData data)
    {
        bool found = false;
        foreach (Q9OverviewData od in _overviewData)
        {
            if (od._guid == data._guid)
            {
                found = true;
            }
        }

        if (found)
        {
			int ind = _overviewData.IndexOf(data);
			Destroy(_overviewRows[ind].gameObject);
			_overviewRows.RemoveAt(ind);
            _overviewData.Remove(data);
        }
        else
        {
            print("Overview data not found, cannot remove from list");
        }
    }

	public void FixedUpdate()
	{
		if(Time.time > nextUpdate)
		{
			nextUpdate = Time.time + 1;
			if(_overviewData.Count == _overviewRows.Count)
			{
				for(int i = 0; i < _overviewData.Count; i++)
				{
					_overviewRows[i].icon.sprite = _overviewData[i]._icon;
					_overviewRows[i].name.text = _overviewData[i]._name;
					_overviewRows[i].type.text = _overviewData[i]._type.ToString();
					_overviewRows[i].alliance.text = _overviewData[i]._alliance.ToString();

					float dist = Vector3.Distance(_overviewData[i]._go.transform.position, Vector3.zero);
					_overviewRows[i].distance.text = (dist.ToString("###") + " km");
				}
			}
			else
			{
				print("Overview Lists not synchronized");
			}
		}
	}
}
