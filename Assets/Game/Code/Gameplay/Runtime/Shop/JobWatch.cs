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
        private double _runTime;

        public bool EndTimeReached { get; private set; }
        public int ElapsedHours { get; private set; }
        public int WorkingHours => _endHours - _startHours;
        public float HourDuration => 60f / _timeSpeed;

        public void Run(float timeSpeed)
        {
            _timeSpeed = timeSpeed;
            _runTime = Time.realtimeSinceStartupAsDouble * 100;
            
            EndTimeReached = false;
            ElapsedHours = 0;

            RenderTime(_startHours);
        }

        private void Update()
        {
            if (EndTimeReached)
                return;

            double elapsedTimeInMinutes = (Time.realtimeSinceStartupAsDouble * 100 - _runTime) * _timeSpeed / 60;
            ElapsedHours = (int)(elapsedTimeInMinutes / 60);
            
            int hours = _startHours + ElapsedHours;
            int minutes = (int)(elapsedTimeInMinutes % 60);
            
            if (hours >= _endHours)
                RenderTime(_endHours);
            else
                RenderTime(hours, minutes);

            EndTimeReached = hours == _endHours;
        }

        private void RenderTime(int hours, int minutes = 0)
        {
            _timeText.text = $"{hours:00}:{minutes:00}";
        }
    }
}