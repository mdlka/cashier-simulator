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

        private Func<float> _timeSpeed;

        public bool Stopped { get; private set; }
        public bool EndTimeReached { get; private set; }
        public float ElapsedTimeInMinutes { get; private set; }
        public int ElapsedHours => (int)(ElapsedTimeInMinutes / 60);
        public int WorkingHours => _endHours - _startHours;

        private float CurrentTimeSpeed => _timeSpeed?.Invoke() ?? 1f;

        public void Run(Func<float> timeSpeed)
        {
            _timeSpeed = timeSpeed;

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