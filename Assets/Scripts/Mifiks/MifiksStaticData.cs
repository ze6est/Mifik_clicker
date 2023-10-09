using UnityEngine;

[CreateAssetMenu(fileName = "MifiksData", menuName = "StaticData/Mifik", order = 51)]
public class MifiksStaticData : ScriptableObject
{
    public MifiksName Name;
    [Range(1, 100000000)]
    public int CostPoints;
    public GameObject Prefab;
}
