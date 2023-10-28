using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StaticDataService
{
    private List<MifiksStaticData> _mifiks;
    private List<AchievementsStaticData> _achievements;

    public void LoadMifiks() => 
        _mifiks = Resources.LoadAll<MifiksStaticData>("Mifiks/StaticData").ToList();

    public void LoadAchievements() =>
        _achievements = Resources.LoadAll<AchievementsStaticData>("Achievements/StaticData").ToList();

    public List<MifiksStaticData> GetMifiks() 
        => _mifiks;

    public List<AchievementsStaticData> GetAchievements()
        => _achievements;
}