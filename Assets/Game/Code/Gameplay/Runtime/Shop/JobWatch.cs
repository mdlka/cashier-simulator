using System;
using Lean.Localization;
using TMPro;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    internal class JobWatch : MonoBehaviour
    {
        [SerializeField] private int _startHours;
        [SerializeField] private int _endHours;
        [SerializeField] private TMP_Text _timeText;
        [SerializeField, LeanTranslationName] private string _dayTranslationName; 

        private Func<float> _timeSpeed;
        private string _currentDay;

        public bool Stopped { get; private set; } = true;
        public bool EndTimeReached { get; private set; } = true;
        public float ElapsedTimeInMinutes { get; private set; }
        public int ElapsedHours => (int)(ElapsedTimeInMinutes / 60);
        public int WorkingHours => _endHours - _startHours;

        private float CurrentTimeSpeed => _timeSpeed?.Invoke() ?? 1f;

        public void Run(Func<float> timeSpeed, long day)
        {
            _timeSpeed = timeSpeed;
            _currentDay = $"{LeanLocalization.GetTranslationText(_dayTranslationName)} {day}";

            Stopped = false;
            EndTimeReached = false;
            ElapsedTimeInMinutes = 0;

            RenderTime(_startHours);
        }

        public void Stop()
        {
            Stopped = true;
        }

        public void Continue()
        {
            Stopped = false;
        }

        private void Update()
        {
            if (EndTimeReached || Stopped)
                return;

            ElapsedTimeInMinutes += Time.unscaledDeltaTime * CurrentTimeSpeed;
            
            int hours = _startHours + ElapsedHours;
            int minutes = (int)(ElapsedTimeInMinutes % 60);
            
            if (hours >= _endHours)
                RenderTime(_endHours);
            else
                RenderTime(hours, minutes);

            EndTimeReached = hours >= _endHours;
        }

        private void RenderTime(int hours, int minutes = 0)
        {
            _timeText.text = $"<size=80%>{_currentDay}, <size=100%>{hours:00}:{minutes:00}";
        }
    }
}