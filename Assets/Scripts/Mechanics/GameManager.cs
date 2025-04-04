using System;
using Flatformer.GameData;
using Platformer;
using Platformer.Model;
using Platformer.Observer;
using System.Collections.Generic;
using Unitilies;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using YG;
using Random = UnityEngine.Random;


namespace Platformer.Mechanics
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        [SerializeField] private PlatformerModel model;
        [SerializeField] private Canvas Canvas;
        [SerializeField] private SpriteRenderer[] gridSpriteRenderers;

        public int currentLevel;
        public List<GameObject> listEffect = new();
        public GameObject learningBlockPrefab;

        private GameObject _objLevel;
        
        private const string IS_WIN = "IsWin";
        private const string IS_DEATH = "IsDeath";
        
        public int Coin { get; set; }

        private void Awake()
        {
            Instance = this;
            YandexGame.InitEnvirData();
            Canvas.GetComponent<CanvasScaler>().matchWidthOrHeight =
                YandexGame.EnvironmentData.deviceType == "desktop" ? 1 : 0;
        }

        private void OnDisable()
        {
            if (EventDispatcher.HasInstance())
            {
                EventDispatcher.Instance.RemoveListener(EventID.Start, (param) => StartGame());
                EventDispatcher.Instance.RemoveListener(EventID.Replay, (param) => ReplayGame());
            }
        }
        
        private void OnEnable()
        {
            this.RegisterListener(EventID.Start, (param) => StartGame());
            this.RegisterListener(EventID.Replay, (param) => ReplayGame());
        }

        public void StartGame()
        {
            LoadNewLevel();
        }

        public void ReplayGame()
        {
            if (_objLevel != null)
            {
                Destroy(_objLevel.gameObject);
            }
            foreach (GameObject effect in listEffect)
            {
                Destroy(effect.gameObject);
            }
            GameState.IsGameStart = false;
            _objLevel = Instantiate(model.levels[currentLevel],transform) as GameObject;
        }

        private void LoadNewLevel()
        {
            if (_objLevel != null)
                Destroy(_objLevel.gameObject);

            currentLevel = GameDataManager.GetLevel();
            var randomColor = model.backGrounds[Random.Range(0, model.backGrounds.Count)];
            Camera.main.backgroundColor = randomColor;
            foreach (var gridSpriteRenderer in gridSpriteRenderers)
            {
                gridSpriteRenderer.color = randomColor;
            }
            if (GameDataManager.GetLevel() >= model.levels.Count)
            {
                currentLevel = Random.Range(10, model.levels.Count);
            }

            if (GameDataManager.GetLevel() != 0 && GameDataManager.GetLevel() % 6 == 0)
            {
                YandexGame.ReviewShow(false);
            }

            this.PostEvent(EventID.NextLevel);
            GameState.IsGameStart = false;
            MetricaSender.Instance.SendLevelStartData();
            _objLevel = Instantiate(model.levels[currentLevel], transform);
        }

        public void NextLevel()
        {
            if (currentLevel > model.levels.Count)
                this.PostEvent(EventID.EndGame);
           
            foreach(GameObject effect in listEffect)
            {
                Destroy(effect.gameObject);
            }
            LoadNewLevel();
        }

        public void PlayerDeath(PlayerController playerRef)
        {
            if(!GameState.IsGameStart) return;
            GameState.IsGameStart = false;
            SoundManager.instance.PlayAudioFail();
            this.PostEvent(EventID.Loss);
            MetricaSender.Instance.SendLevelFailedData();
            if (playerRef != null)
            {
                playerRef.isControlEnable = false;
                playerRef.transform.rotation = Quaternion.Euler(0, 180f, 0);
                SoundManager.instance.PlayAudioSound(playerRef.deathAudio);
                playerRef._myAnimator.SetBool(IS_DEATH, true);
            }
        }

        public void PlayerWin(PlayerController playerRef, int reward)
        {
            if(!GameState.IsGameStart) return;
            GameState.IsGameStart = false;
            Instance.Coin += reward;
            MetricaSender.Instance.SendLevelCompleteData();
            GameDataManager.AddLevel(1);
            this.PostEvent(EventID.OnCarMove,true);
            if (playerRef != null)
            {
                playerRef.isControlEnable = false;
                playerRef.transform.rotation = Quaternion.Euler(0, 180f, 0);
                playerRef._myAnimator.SetBool(IS_WIN, true);
            }
        }
    }
}


