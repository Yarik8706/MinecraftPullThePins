
namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        public string shopData;
        public string playerData;
        public bool mutedMusic = false;
        public bool mutedSound = false;
        public int allMoney;
        public string openLevels;
        public int freeSpin;
        public bool isNeedLearning = true;
    }
}
