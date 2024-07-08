using System;
using TMPro;
using UnityEngine;

namespace YellowSquad.CashierSimulator.Gameplay
{
    internal class JobWatch : MonoBehaviour
    {
        [SerializeField] private int _startHours;
        [SerializeField] private int _endHours;
        [SerializeField] private TMP_Text _timeText;

        private float _timeSpeed;
        private float _acceleratedTimeSpeed;
        private Func<bool> _needSpeedUp;

        public bool Stopped { get; private set; }
        public bool EndTimeReached { get; private set; }
        public float ElapsedTimeInMinutes { get; private set; }
        public int ElapsedHours => (int)(ElapsedTimeInMinutes / 60);
        public int WorkingHours => _endHours - _startHours;
        public float HourDuration => 60f / CurrentTimeSpeed;
        
        private float CurrentTimeSpeed => NeedSpeedUp ? _acceleratedTimeSpeed : _timeSpeed;
        private bool NeedSpeedUp => _needSpeedUp?.Invoke() ?? false;

        public void Run(float timeSpeed, float acceleratedTimeSpeed, Func<bool> needSpeedUp)
        {
            _timeSpeed = timeSpeed;
            _acceleratedTimeSpeed = acceleratedTimeSpeed;
            _needSpeedUp = needSpeedUp;

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
            _timeText.text = $"{hours:00}:{minutes:00}";
        }
    }
}