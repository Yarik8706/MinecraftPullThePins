using Platformer.Observer;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class CanvasManager : MonoBehaviour
{
    [Header("Referencs")]
    [SerializeField] GameObject openSpin;

    [Header("UI elements")]
    [SerializeField]
    private GameObject _mainMenuUI;
    
    [Header("Event CanvasManager")]

    [Space(20f)]
    [SerializeField] private Button _playGameButton;
    [SerializeField] private Button _soundButton;
    [SerializeField] private Button _openShopButton;
    [SerializeField] private Button _removeAdsButton;
    [SerializeField] private Button openSpinButton;

    private void OnDisable()
    {
        if (EventDispatcher.HasInstance())
        {
            EventDispatcher.Instance.RemoveListener(EventID.Home, (param) => Show());
        }
    }
    
    private void Start()
    {
        Hide();
        AddEvents();
    }
    
    private void AddEvents()
    {
        // Handle Event Button Play Game
        _playGameButton.onClick.AddListener(() =>
        {
            SoundManager.instance.PlayAudioSound(SoundManager.instance.buttonAudio);
            OnPlayGame();
        });

        //
        _openShopButton.onClick.AddListener(() =>
        {
            SoundManager.instance.PlayAudioSound(SoundManager.instance.buttonAudio);
            Hide();
            this.PostEvent(EventID.OpenShop);
        });

        //
        _removeAdsButton.onClick.AddListener(() =>
        {
            SoundManager.instance.PlayAudioSound(SoundManager.instance.buttonAudio);
            Debug.Log("remove Ads successfully");
        });

        //
        openSpinButton.onClick.AddListener(() =>
        {
            SoundManager.instance.PlayAudioSound(SoundManager.instance.buttonAudio);
            Hide();
            openSpin.SetActive(true);
        });
    }

    private void OnPlayGame()
    {
        this.PostEvent(EventID.GameStartUI);
        this.PostEvent(EventID.IsPlayGame, true);
        Hide() ;
    }
    
    private void Show()
    {
        _mainMenuUI.SetActive(true);
    }
    
    private void Hide()
        => _mainMenuUI.SetActive(false);
    
    private void OnEnable()
    {
        this.RegisterListener(EventID.Home, (param) => Show());
    }
}
