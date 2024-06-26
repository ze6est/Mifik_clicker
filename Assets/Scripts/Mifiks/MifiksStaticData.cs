using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MifiksData", menuName = "StaticData/Mifik", order = 51)]
public class MifiksStaticData : ScriptableObject
{
    public MifiksName NameId;
    [Range(1L, 1000000000L)]
    public long CostUnlocked;
    [Header("AutoClick")]
    public long PointsPerAutoClick;
    public long UpgradeAutoClickCost;
    public int UpgradeCountAutoClick;
    [Header("Time")]
    public long TimeSecondsAutoClick;
    public long UpgradeTimeCost;
    public int UpgradeCountTime;
    [Header("Image")]
    public Sprite Icon;
    public bool IsLocked;    
}