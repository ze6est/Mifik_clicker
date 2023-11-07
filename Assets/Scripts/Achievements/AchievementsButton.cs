using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.Scripts.Infrastructure.States;

namespace Assets.Scripts.Achievements
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class AchievementsButton<T> : MonoBehaviour, ISavedProgress
    {
        [SerializeField] protected Points Points;
        [SerializeField] protected Button Button;
        [SerializeField] protected AchievementsType Type;
        [SerializeField] protected Image Icon;
        [SerializeField] protected string TaskName;
        [SerializeField] protected TextMeshProUGUI TaskNameHUD;
        [SerializeField] protected int TaskCount;
        [SerializeField] protected int[] TaskValues;
        [SerializeField] protected long[] TaskAwardPoints;
        [SerializeField] protected bool IsLocked;
        [SerializeField] protected int AchievementNumber;
        [SerializeField] protected AudioSource AudioSourceAchievement;
        [SerializeField] protected AudioClip OpenAchievement;
        [SerializeField] protected AudioClip GetAchievement;

        protected abstract T Parametr { get; set; }

        protected void OnValidate()
        {
            Button = gameObject.GetComponent<Button>();
            Icon = gameObject.GetComponentInChildren<Image>();
            TaskNameHUD = gameObject.GetComponentInChildren<TextMeshProUGUI>();
            AudioSourceAchievement = gameObject.GetComponent<AudioSource>();

            OpenAchievement = Resources.Load<AudioClip>("StreamingAssets/OpenAchievement");
            GetAchievement = Resources.Load<AudioClip>("StreamingAssets/GetAchievement");
        }

        public void Construct(Points points, AchievementsType type, Image icon, string taskName, long[] taskAwardPoints, int taskCount, int[] taskValues, bool isLocked, int achievementNumber)
        {
            Points = points;
            Type = type;
            Icon = icon;
            TaskName = taskName;
            TaskNameHUD.text = taskName;
            TaskCount = taskCount;
            TaskValues = taskValues;
            TaskAwardPoints = taskAwardPoints;
            IsLocked = isLocked;

            AchievementNumber = achievementNumber;
        }

        public void Construct(Points points, AchievementsType type, string taskName, int taskCount, int[] taskValues, long[] taskAwardPoints, Image icon)
        {
            Points = points;
            Type = type;
            TaskName = taskName;
            TaskNameHUD.text = taskName;
            TaskCount = taskCount;
            TaskValues = taskValues;
            TaskAwardPoints = taskAwardPoints;
            Icon = icon;
        }        

        protected virtual void Start()
        {
            Button.onClick.AddListener(GetAwardPoints);

            if (IsLocked)
            {
                if(AchievementNumber <= TaskValues.Length)
                    TaskNameHUD.text = $"{TaskName} {TaskValues[AchievementNumber - 1]}";
                else
                    TaskNameHUD.text = "Все задания выполнены";

                Button.interactable = false;
            }
            else
            {                
                Button.interactable = true;
                TaskNameHUD.text = $"Заберите {TaskAwardPoints[AchievementNumber - 1]} поинтов";
            }

            if (AchievementNumber > TaskValues.Length)
                Button.onClick.RemoveListener(GetAwardPoints);
        }

        protected abstract void GetAwardPoints();

        protected void TryGetPlayOpenAchievementSound()
        {
            if (IsLocked)
                AudioSourceAchievement.PlayOneShot(OpenAchievement);
        }

        public virtual void LoadProgress(PlayerProgress progress)
        {
            if (!LoadProgressState.IsNewProgress)
            {
                IsLocked = progress.Achievements[(int)Type].IsLocked;
                AchievementNumber = progress.Achievements[(int)Type].AchievementNumber;                
            }
        }

        public virtual void UpdateProgress(PlayerProgress progress)
        {
            progress.Achievements[(int)Type].IsLocked = IsLocked;
            progress.Achievements[(int)Type].AchievementNumber = AchievementNumber;            
        }
    }
}