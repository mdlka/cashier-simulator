using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using YellowSquad.CashierSimulator.Gameplay.Useful;
using YellowSquad.GamePlatformSdk;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class GameSettings : MonoBehaviour
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
        private JobWatch _watch;
        private GameSettingsSave _currentSettingsSave;

        public bool Opened { get; private set; }
        public InputSettings InputSettings => _inputSettings;

        private void Awake()
        {
            _canvasGroup.Disable();
        }
        
        private void OnDestroy()
        {
            _closeButton.onClick.RemoveListener(Close);
            _musicToggle.StateChanged -= SetActiveMusic;
            _audioToggle.StateChanged -= SetActiveAudio;
            _cursorSensitivitySlider.onValueChanged.RemoveListener(ChangeCursorSensitivity);
            _rotationSensitivitySlider.onValueChanged.RemoveListener(ChangeRotationSensitivity);
        }

        internal void Initialize(ISave save, JobWatch watch)
        {
            _save = save;
            _watch = watch;

            _inputSettings.Initialize();

            _currentSettingsSave = new GameSettingsSave
            {
                AudioIsOn = true,
                MusicIsOn = true,
                CursorSensitivity = _inputSettings.MouseSensitivity,
                RotationSensitivity = _inputSettings.RotationSensitivity
            };

            if (_save.HasKey(SaveConstants.GameSettingsSaveKey))
            {
                var settingsSave = JsonConvert.DeserializeObject<GameSettingsSave>(
                    _save.GetString(SaveConstants.GameSettingsSaveKey));
                
                SetActiveAudio(settingsSave.AudioIsOn);
                SetActiveMusic(settingsSave.MusicIsOn);
                _inputSettings.ChangeRotationSensitivity(_currentSettingsSave.RotationSensitivity = settingsSave.RotationSensitivity);
                _inputSettings.ChangeMouseSensitivity(_currentSettingsSave.CursorSensitivity = settingsSave.CursorSensitivity);
            }
            
            _audioToggle.ChangeState(_currentSettingsSave.AudioIsOn);
            _musicToggle.ChangeState(_currentSettingsSave.MusicIsOn);

            _rotationSensitivitySlider.value = Mathf.InverseLerp(_inputSettings.MinRotationSensitivity,
                _inputSettings.MaxRotationSensitivity, _currentSettingsSave.RotationSensitivity);
            _cursorSensitivitySlider.value = Mathf.InverseLerp(_inputSettings.MinMouseSensitivity, 
                _inputSettings.MaxMouseSensitivity, _currentSettingsSave.CursorSensitivity);
            
            _closeButton.onClick.AddListener(Close);
            _musicToggle.StateChanged += SetActiveMusic;
            _audioToggle.StateChanged += SetActiveAudio;
            _cursorSensitivitySlider.onValueChanged.AddListener(ChangeCursorSensitivity);
            _rotationSensitivitySlider.onValueChanged.AddListener(ChangeRotationSensitivity);
        }

        public void Open()
        {
            if (Opened)
                return;
            
            Opened = true;
            
            _canvasGroup.Enable(0.2f);
            _watch.Stop();
            Time.timeScale = 0;
        }
        
        private void Close()
        {
            if (Opened == false)
                return;

            Opened = false;
            _canvasGroup.Disable(0.2f);
            _watch.Continue();
            
            _save.SetString(SaveConstants.GameSettingsSaveKey, JsonConvert.SerializeObject(_currentSettingsSave));
            _save.Save();
            Time.timeScale = 1;
        }
        
        private void ChangeCursorSensitivity(float value)
        {
            _inputSettings.ChangeMouseSensitivity(
                Mathf.Lerp(
                    _inputSettings.MinMouseSensitivity, 
                    _inputSettings.MaxMouseSensitivity, 
                    value));

            _currentSettingsSave.CursorSensitivity = _inputSettings.MouseSensitivity;
        }

        private void ChangeRotationSensitivity(float value)
        {
            _inputSettings.ChangeRotationSensitivity(
                Mathf.Lerp(
                    _inputSettings.MinRotationSensitivity, 
                    _inputSettings.MaxRotationSensitivity, 
                    value));

            _currentSettingsSave.RotationSensitivity = _inputSettings.RotationSensitivity;
        }

        private void SetActiveMusic(bool isOn)
        {
            _audioMixer.SetFloat(MusicVolumeParam, isOn ? MusicMaxVolume : -80f);
            _currentSettingsSave.MusicIsOn = isOn;
        }

        private void SetActiveAudio(bool isOn)
        {
            _audioMixer.SetFloat(GameVolumeParam, isOn ? GameMaxVolume : -80f);
            _currentSettingsSave.AudioIsOn = isOn;
        }
    }

    [Serializable]
    internal class GameSettingsSave
    {
        [JsonProperty] public bool AudioIsOn;
        [JsonProperty] public bool MusicIsOn;
        [JsonProperty] public float RotationSensitivity;
        [JsonProperty] public float CursorSensitivity;
    }
}