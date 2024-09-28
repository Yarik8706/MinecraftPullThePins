using YG;

namespace Flatformer.GameData.UIManager
{
    public class MusicStateUI : SoundState
    {
        public static MusicStateUI Instance { get; private set; }

        private void Awake()
        {
            AudioMixerParameter = "Music";
            Instance = this;    
        }
        
        protected override bool GetMutedState()
        {
            return YandexGame.savesData.mutedMusic;
        }
        
        protected override void SetMutedState(bool isMuted)
        {
            base.SetMutedState(isMuted);
            YandexGame.savesData.mutedMusic = isMuted;
            YandexGame.SaveProgress();
        }
    }
}