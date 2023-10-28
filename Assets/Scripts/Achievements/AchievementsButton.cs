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
        [SerializeField] protected TextMeshProUGUI TaskName;
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
            TaskName.text = taskName;
            TaskCount = taskCount;
            TaskValues = taskValues;
            TaskAwardPoints = taskAwardPoints;
            IsLocked = isLocked;

            AchievementNumber = achievementNumber;
        }

        public void Construct(Points points, AchievementsType type, Image icon)
        {
            Points = points;
            Type = type;
            Icon = icon;
        }

        protected void OnValidate()
        {
            Button = gameObject.GetComponent<Button>();
            Icon = gameObject.GetComponentInChildren<Image>();
            TaskName = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        }

        protected virtual void Awake()
        {
            Debug.Log("Awake");

            Button.onClick.AddListener(GetAwardPoints);

            if (IsLocked)
            {
                Debug.Log("Locked");

                Button.interactable = false;
                TaskName.text = $"{TaskName.text} {TaskValues[AchievementNumber]}";
            }
            else
            {
                Debug.Log("UnLocked");

                Button.interactable = true;
                TaskName.text = $"{TaskAwardPoints[AchievementNumber]}";
            }
        }

        protected void GetAwardPoints()
        {
            Points.RefreshPoints(TaskAwardPoints[AchievementNumber]);

            AchievementNumber += 1;

            Button.interactable = false;

            if (AchievementNumber >= TaskAwardPoints.Length)
            {                
                Button.onClick.RemoveListener(GetAwardPoints);
                TaskName.text = "Все задания выполнены.";
            }
        }
    }
}