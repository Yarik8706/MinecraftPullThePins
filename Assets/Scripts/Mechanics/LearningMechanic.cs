using System;
using System.Collections;
using Flatformer.GameData;
using Platformer.Mechanics;
using Platformer.Observer;
using UnityEngine;
using UnityEngine.Serialization;
using YG;

namespace Mechanics
{
    [Serializable]
    public struct LearningStep 
    {
        public Transform learningBlockSpawnPosition;
        public GameObject targetBlockForDelete;
    }
    
    public class LearningMechanic : MonoBehaviour
    {
        [SerializeField] private LearningStep[] _learningSteps;
        
        private GameObject _learningBlockPrefab;
        private GameObject _activeLearningBlock;
        private int _currentStep;
        
        private void Start()
        {
            if(!YandexGame.savesData.isNeedLearning) return;
            _learningBlockPrefab = GameManager.Instance.learningBlockPrefab;
            StartLearning();
        }

        private void StartLearning()
        {
            if(!YandexGame.savesData.isNeedLearning) return;
            _activeLearningBlock = Instantiate(_learningBlockPrefab);
            _activeLearningBlock.transform.position = new Vector3(0, 0, 0.9f);
            NextLearningStep();
        }

        private void OnDestroy()
        {
            Destroy(_activeLearningBlock);
        }

        public void NextLearningStep()
        {
            if (_currentStep == _learningSteps.Length)
            {
                Destroy(_activeLearningBlock);
                return;
            }
            _activeLearningBlock.transform.position = 
                _learningSteps[_currentStep].learningBlockSpawnPosition.position;
            StartCoroutine(WaifForBlockDelete(_currentStep));
            _currentStep++;
        }

        private IEnumerator WaifForBlockDelete(int index)
        {
            var oldPosition = _learningSteps[index].targetBlockForDelete.transform.position;
            yield return new WaitWhile(() => _learningSteps[index].targetBlockForDelete && _learningSteps[index].targetBlockForDelete.transform.position == oldPosition);
            _activeLearningBlock.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            _activeLearningBlock.SetActive(true);
            NextLearningStep();
        }
    }
}