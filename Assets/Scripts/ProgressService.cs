using Assets.Scripts;
using UnityEngine;

public class ProgressService
{
    public PlayerProgress Progress { get; set; }

    private const string ProgressKey = "Progress";
    private LoadFactory _loadFactory;

    public ProgressService(LoadFactory loadFactory) =>
        _loadFactory = loadFactory;


    public void SaveProgress()
    {
        foreach (ISavedProgress savedProgress in _loadFactory.ProgressSaveds)        
            savedProgress.UpdateProgress(Progress);        

        PlayerPrefs.SetString(ProgressKey, Progress.ToJson());
    }

    public PlayerProgress LoadProgress() => 
        PlayerPrefs.GetString(ProgressKey)?.ToDeserialized<PlayerProgress>();
}