using System;
using System.Collections;
using Assets.Scripts;
using UnityEngine;

public class ProgressService
{
    private const string ProgressKey = "Progress28";

    private LoadFactory _loadFactory;

    public PlayerProgress Progress { get; set; }

    public event Action ProgressSaved;

    public ProgressService(LoadFactory loadFactory) =>
        _loadFactory = loadFactory;

    public IEnumerator SaveProgress()
    {
        foreach (ISavedProgress savedProgress in _loadFactory.ProgressSaveds)
        {
            savedProgress.UpdateProgress(Progress);
            yield return null;
        }

        PlayerPrefs.SetString(ProgressKey, Progress.ToJson());

        ProgressSaved?.Invoke();
    }

    public PlayerProgress LoadProgress() => 
        PlayerPrefs.GetString(ProgressKey)?.ToDeserialized<PlayerProgress>();

    public void ClearProgress()
    {
        PlayerPrefs.DeleteKey(ProgressKey);
        PlayerPrefs.Save();
    }
}