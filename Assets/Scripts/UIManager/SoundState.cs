using UnityEngine;
using UnityEngine.Audio;
using YG;

namespace Flatformer.GameData.UIManager
{
    public abstract class SoundState : MonoBehaviour
    {
        [SerializeField] private AudioMixerGroup _mixerGroup;
        [SerializeField] private GameObject _volumeBlockImage;
        
        private bool _muted;
        
        protected string AudioMixerParameter;

        public void Init()
        {
            _muted = GetMutedState();
            if (_muted)
            {
                _mixerGroup.audioMixer.SetFloat(AudioMixerParameter, -80);
                _volumeBlockImage.SetActive(true);
            }
        }

        protected virtual bool GetMutedState()
        {
            return true;
        }
        
        protected virtual void SetMutedState(bool isMuted)
        {
            _muted = isMuted;
        }
        
        public void ChangeVolumeState()
        {
            SoundManager.instance.PlayAudioSound(SoundManager.instance.buttonAudio);
            if (_volumeBlockImage.activeSelf)
            {
                _volumeBlockImage.SetActive(false);
                _mixerGroup.audioMixer.SetFloat(AudioMixerParameter, 0);
                SetMutedState(false);
            }
            else
            {
                _volumeBlockImage.SetActive(true);
                _mixerGroup.audioMixer.SetFloat(AudioMixerParameter, -80);
                SetMutedState(true);
            }
        }
    }
}