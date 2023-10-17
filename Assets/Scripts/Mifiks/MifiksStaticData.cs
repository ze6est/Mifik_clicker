using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "MifiksData", menuName = "StaticData/Mifik", order = 51)]
public class MifiksStaticData : ScriptableObject
{
    public MifiksName NameId;
    [Range(1L, 100000000L)]
    public long CostPoints;
    public long PointsPerSecond;
    public long UpgradeCount;
    public Image Icon;
}
