using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Assets.Scripts.Achievements
{
    public abstract class AchievementsButton : MonoBehaviour
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

        protected void OnValidate()
        {
            Button = gameObject.GetComponent<Button>();
            Icon = gameObject.GetComponentInChildren<Image>();
            TaskNameHUD = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        }

        protected virtual void Start()
        {
            if (IsLocked)
            {
                if(AchievementNumber < TaskValues.Length)
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
        }        
    }
}