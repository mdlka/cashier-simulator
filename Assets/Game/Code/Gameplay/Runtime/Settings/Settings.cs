using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using YellowSquad.CashierSimulator.Gameplay.Useful;
using YellowSquad.GamePlatformSdk;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class Settings : MonoBehaviour
    {
        private const string MusicVolumeParam = "MusicVolume";
        private const string GameVolumeParam = "GameVolume";
        private const float MusicMaxVolume = -3.1f;
        private const float GameMaxVolume = 0f;
        
        [SerializeField] private Button _closeButton;
        [SerializeField] private SpriteToggle _audioToggle;
        [SerializeField] private SpriteToggle _musicToggle;
        [SerializeField] private Slider _cursorSensitivitySlider;
        [SerializeField] private Slider _rotationSensitivitySlider;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private InputSettings _inputSettings;

        private ISave _save;

        public bool Opened { get; private set; }
        public InputSettings InputSettings => _inputSettings;

        private void Awake()
        {
            _canvasGroup.Disable();
            
            _closeButton.onClick.AddListener(OnCloseButtonClick);
            _musicToggle.StateChanged += OnMusicToggleStateChanged;
            _audioToggle.StateChanged += OnAudioToggleStateChanged;
            _cursorSensitivitySlider.onValueChanged.AddListener(OnCursorSensitivityChanged);
            _rotationSensitivitySlider.onValueChanged.AddListener(OnRotationSensitivityChanged);
        }

        private void OnDestroy()
        {
            _closeButton.onClick.RemoveListener(OnCloseButtonClick);
            _musicToggle.StateChanged -= OnMusicToggleStateChanged;
            _audioToggle.StateChanged -= OnAudioToggleStateChanged;
            _cursorSensitivitySlider.onValueChanged.RemoveListener(OnCursorSensitivityChanged);
            _rotationSensitivitySlider.onValueChanged.RemoveListener(OnRotationSensitivityChanged);
        }

        public void Initialize(ISave save)
        {
            _save = save;
        }

        public void Open()
        {
            Opened = true;
            
            _canvasGroup.Enable(0.2f);
        }
        
        private void OnCursorSensitivityChanged(float value)
        {
            _inputSettings.ChangeMouseSensitivity(
                Mathf.Lerp(
                    _inputSettings.MinMouseSensitivity, 
                    _inputSettings.MaxMouseSensitivity, 
                    value));
        }

        private void OnRotationSensitivityChanged(float value)
        {
            _inputSettings.ChangeRotationSensitivity(
                Mathf.Lerp(
                    _inputSettings.MinRotationSensitivity, 
                    _inputSettings.MaxRotationSensitivity, 
                    value));
        }

        private void OnMusicToggleStateChanged(bool isOn)
        {
            if (Opened == false)
                return;

            _audioMixer.SetFloat(MusicVolumeParam, isOn ? MusicMaxVolume : -80f);
        }

        private void OnAudioToggleStateChanged(bool isOn)
        {
            if (Opened == false)
                return;
            
            _audioMixer.SetFloat(GameVolumeParam, isOn ? GameMaxVolume : -80f);
        }

        private void OnCloseButtonClick()
        {
            if (Opened == false)
                return;

            Opened = false;
            _canvasGroup.Disable(0.2f);
        }
    }

    [Serializable]
    internal class SettingsSave
    {
        [JsonProperty] public bool AudioMute;
        [JsonProperty] public bool MusicMute;
        [JsonProperty] public float RotationSensitivity;
        [JsonProperty] public float CursorSensitivity;
    }
}