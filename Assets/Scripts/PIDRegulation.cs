using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class PidRegulation
    {
        private float kp, ki, kd;
        private float _integral = 0;
        private float _dV = 0;
        private float _lastError = 0;
        
        public PidRegulation(float kp, float ki, float kd)
        {
            this.kp = kp;
            this.ki = ki;
            this.kd = kd;
        }
        
        public float GetPID(float error)
        {
            _integral += error * Time.fixedDeltaTime;
            _dV = (error - _lastError) / Time.fixedDeltaTime;
            var pid = error * kp + _integral * ki + _dV * kd;
            _lastError = error;
            return pid;
        }
    }
}