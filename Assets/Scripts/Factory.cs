using System.Collections.Generic;
using UnityEngine;

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
            Card card = block.GetComponentInChildren<Card>();
            card.Construct(mifik.NameId, mifik.PointsPerSecond, mifik.UpgradeCount, mifik.Icon);

            LockedButton lockedButton = block.GetComponentInChildren<LockedButton>();
            lockedButton.Construct(_points, mifik.CostPoints);

            Debug.Log($"{mifik.CostPoints} F");

            Object.Instantiate(block, container.transform);
        }
    }
}