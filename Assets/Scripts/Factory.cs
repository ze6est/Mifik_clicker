using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class Factory
{
    private StaticDataService _staticDataService = new StaticDataService();
    private Points _points;    

    public Factory(Points points)
    {
        _points = points;        
    }

    public void InstantiateBloks(GameObject container)
    {
        Block block = Resources.Load<Block>("Block");

        _staticDataService.LoadMifiks();
        List<MifiksStaticData> mifiks = _staticDataService.GetMifiks();

        foreach (MifiksStaticData mifik in mifiks)
        {
            LockedButton lockedButton = block.GetComponentInChildren<LockedButton>();
            lockedButton.Construct(_points, mifik.CostUnlocked);

            Card card = block.GetComponentInChildren<Card>();
            card.Construct(mifik.NameId, mifik.PointsPerAutoClick, mifik.TimeSecondsAutoClick, mifik.UpgradeCount, mifik.UpgradeTimeCount, mifik.Icon, lockedButton, _points);            

            Object.Instantiate(block, container.transform);
        }
    }    
}