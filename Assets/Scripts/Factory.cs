using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class Factory
{
    private StaticDataService _staticDataService = new StaticDataService();
    private Points _points;
    private AutoClick _autoClick;

    public Factory(Points points, AutoClick autoClick)
    {
        _points = points;
        _autoClick = autoClick;
    }

    public void InstantiateBloks(GameObject container)
    {
        Block block = Resources.Load<Block>("Block");

        _staticDataService.LoadMifiks();
        List<MifiksStaticData> mifiks = _staticDataService.GetMifiks();

        foreach (MifiksStaticData mifik in mifiks)
        {
            LockedButton lockedButton = block.GetComponentInChildren<LockedButton>();
            lockedButton.Construct(_points, mifik.CostPoints);

            Card card = block.GetComponentInChildren<Card>();
            card.Construct(mifik.NameId, mifik.PointsPerSecond, mifik.UpgradeCount, mifik.Icon, lockedButton, _autoClick, _points);            

            Object.Instantiate(block, container.transform);
        }
    }    
}