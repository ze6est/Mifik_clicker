using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StaticDataService
{
    private List<MifiksStaticData> _mifiks;

    public void LoadMifiks() => 
        _mifiks = Resources.LoadAll<MifiksStaticData>("Mifiks/StaticData").ToList();

    public List<MifiksStaticData> GetMifiks() 
        => _mifiks;
}