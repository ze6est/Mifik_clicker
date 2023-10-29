using System;

[Serializable]
public class CardProgress
{    
    public long PointsPerAutoClick;
    public long UpgradeAutoClickCost;
    public int UpgradeCountAutoClickCurrent;

    public long TimeSecondsAutoClick;
    public long UpgradeTimeCost;
    public int UpgradeCountTimeCurrent;

    public bool IsLocked = true;
}