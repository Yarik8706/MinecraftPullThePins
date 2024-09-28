using UnityEngine;
using YG;

namespace Flatformer.GameData.UIManager
{
    public class EffectsStateUI : SoundState
    {
        public static EffectsStateUI Instance { get; private set; }

        private void Awake()
        {
            AudioMixerParameter = "Effects";
            Instance = this;
        }
        
        protected override bool GetMutedState()
        {
            return YandexGame.savesData.mutedSound;
        }
        
        protected override void SetMutedState(bool isMuted)
        {
            base.SetMutedState(isMuted);
            YandexGame.savesData.mutedSound = isMuted;
            YandexGame.SaveProgress();
        }
    }
}