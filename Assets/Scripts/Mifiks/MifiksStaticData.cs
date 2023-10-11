using UnityEngine;

[CreateAssetMenu(fileName = "MifiksData", menuName = "StaticData/Mifik", order = 51)]
public class MifiksStaticData : ScriptableObject
{
    public MifiksName Name;
    [Range(1L, 100000000L)]
    public long CostPoints;
    public GameObject Prefab;
}
