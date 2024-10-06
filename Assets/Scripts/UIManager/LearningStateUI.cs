using System;
using Platformer.Mechanics;
using Platformer.Observer;
using UnityEngine;
using UnityEngine.Serialization;
using YG;

namespace Flatformer.GameData.UIManager
{
    public class LearningStateUI : MonoBehaviour
    {
        [SerializeField] private GameObject _learningStateInfo;
        [SerializeField] private GameObject _learningMessage;
        [SerializeField] private GameObject _changeLearningStateButton;
        
        public static LearningStateUI Instance { get; private set; }
        
        private void Awake()
        {
            Instance = this;
        }

        public void Init()
        {
            if (GameDataManager.GetLevel() >= 5 || !YandexGame.savesData.isNeedLearning)
            {
                _learningMessage.SetActive(false);
                _changeLearningStateButton.SetActive(false);
            }
            else
            {
                _learningStateInfo.SetActive(!YandexGame.savesData.isNeedLearning);
            }
        }

        private void OnEnable()
        {
            this.RegisterListener(EventID.NextLevel, (param) =>
            {
                if(GameDataManager.GetLevel() == 1)_learningMessage.SetActive(false);
                if (GameDataManager.GetLevel() >= 5)
                {
                    _changeLearningStateButton.SetActive(false);
                }
            });
        }

        public void ChangeLearningState()
        {
            YandexGame.savesData.isNeedLearning = !YandexGame.savesData.isNeedLearning;
            _learningMessage.SetActive(false);
            _learningStateInfo.SetActive(!YandexGame.savesData.isNeedLearning);
            GameManager.Instance.ReplayGame();
            this.PostEvent(EventID.IsPlayGame, true);
        }

        public void ChangeLearningState(bool isNeedLearning)
        {
            YandexGame.savesData.isNeedLearning = isNeedLearning;
            YandexGame.SaveProgress();
            _learningStateInfo.SetActive(!isNeedLearning);
            _learningMessage.SetActive(isNeedLearning);
            GameManager.Instance.ReplayGame();
            this.PostEvent(EventID.IsPlayGame, true);
        }
    }
}