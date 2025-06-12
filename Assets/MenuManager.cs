using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    [SerializeField] private GameObject _mainMenuTab;
    [SerializeField] private GameObject _settingsTab;
    [SerializeField] private GameObject _videoTab;
    [SerializeField] private GameObject _audioTab;
    [SerializeField] private GameObject _sensitivityTab;
    [SerializeField] private GameObject _exitTab;
    [SerializeField] private CursorController _cursorController;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _clickSound;
    [SerializeField, Range(0f, 1f)] private float  _clickVolume = 0.5f;
    private EMenuState _menuState = EMenuState.Closed;

   private enum EMenuState
    {
        Closed,
        MainMenu,
        Settings,
        Video,
        Audio,
        Sensetivity,
        Exit
    }
    public void OnEsc()
    {
        if (_menu == null || _cursorController == null || _playerInput == null) return;

        PlayClickSound();

        switch (_menuState)
        {
            case EMenuState.Closed:
                {
                    _menu.SetActive(true);
                    Time.timeScale = 0f;
                    _playerInput.isPaused = true;
                    _cursorController.EnableCursor();
                    _menuState = EMenuState.MainMenu;
                }
                break;
            case EMenuState.MainMenu:
                {
                    _menu.SetActive(false);
                    Time.timeScale = 1f;
                    _playerInput.isPaused = false;
                    _cursorController.DisableCursor();
                    _menuState = EMenuState.Closed;
                }
                break;
            case EMenuState.Settings:
                {
                    if(_settingsTab!=null)
                    {
                        _mainMenuTab.SetActive(true);
                        _settingsTab.SetActive(false);
                        _menuState = EMenuState.MainMenu;
                    }
                    
                }
                break;
            case EMenuState.Video:
                {
                    if (_videoTab != null)
                    {
                        _settingsTab.SetActive(true);
                        _videoTab.SetActive(false);
                        _menuState = EMenuState.Settings;
                    }

                }
                break;
            case EMenuState.Exit:
                {
                    if(_exitTab != null)
                    {
                        _mainMenuTab.SetActive(true) ;
                        _exitTab.SetActive(false);
                        _menuState = EMenuState.MainMenu;
                    }
                    
                }
                break;
        }


    }

    public void OnResume()
    {
        PlayClickSound();
        _menu.SetActive(false);
        Time.timeScale = 1f;
        _playerInput.isPaused = false;
        _cursorController.DisableCursor();
        _menuState = EMenuState.Closed;

    }

    public void OnSettings()
    {
        PlayClickSound();
        _mainMenuTab.SetActive(false);
        _settingsTab.SetActive(true);
        _menuState = EMenuState.Settings;
    }

    public void OnVideo()
    {
        PlayClickSound();
        _settingsTab.SetActive(false);
        _videoTab.SetActive(true);
        _menuState=EMenuState.Video;
    }

    public void OnExit()
    {
        if(_mainMenuTab != null && _exitTab != null)
        {
            PlayClickSound();
            _mainMenuTab.SetActive(false);
            _exitTab.SetActive(true);
            _menuState = EMenuState.Exit;
        }
    }

    public void OnExitConfirmation()
    {
        Application.Quit();
    }

    public void PlayClickSound()
    {
        if (_audioSource != null&&_clickSound != null && _audioSource != null)
        {
              _audioSource.clip = _clickSound;
              _audioSource.volume = _clickVolume;
              _audioSource.Play();
        }
    }
}
