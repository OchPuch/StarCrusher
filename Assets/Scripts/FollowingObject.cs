using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingObject : MonoBehaviour
{
    public Transform target;
    private Rigidbody2D _rb;
    private float _lastError;
    private float _integral;
    private float _dV;

    public float kp;
    public float ki;
    public float kd;

    private bool _targetAcquired = false;

    private void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();

        if (target == null) return;
        transform.parent = null;
        _targetAcquired = true;
    }

    void FixedUpdate()
    {
        if (!_targetAcquired) return;
        var direction = target.position - transform.position;
        var error = Vector3.Magnitude(direction);
        _integral += error * Time.fixedDeltaTime;
        _dV = (error - _lastError) / Time.fixedDeltaTime;
        var pid = error * kp + _integral * ki + _dV * kd;
        _lastError = error;
        _rb.AddForce(pid * (direction).normalized);
    }
}