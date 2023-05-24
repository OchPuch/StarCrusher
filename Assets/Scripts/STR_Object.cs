using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class StrObject : MonoBehaviour
{
    [FormerlySerializedAs("Temperature")] public float temperature;
    
    [SerializeField]private float temperatureLimit;
    [SerializeField] private float temperatureAbility;

    [SerializeField] protected Rigidbody2D Rb;
    [FormerlySerializedAs("Type")] public ObjectType type;

    private float _difAngularSpeed;
    private float _lastAngularSpeed;

    [SerializeField] private float bigAbilityCooldown = 5f;
    [SerializeField] private float mediumAbilityCooldown = 5f;
    private float _currentBigCoolDown;
    private float _currentMediumCoolDown;

    protected void Start()
    {
        if (Rb == null)
        {
            Rb = GetComponent<Rigidbody2D>();
        }

        _lastAngularSpeed = Rb.angularVelocity;
    }

    protected void Update()
    {
        var angularVelocity = Rb.angularVelocity;
        _difAngularSpeed = (angularVelocity - _lastAngularSpeed) / Time.deltaTime;
        _lastAngularSpeed = angularVelocity;
        switch (type)
        {
            case ObjectType.Accelerator:
                if (temperature <= 0)
                {
                    temperature = 0;
                }
                else if (temperature > 1500)
                {
                    temperature -= Time.deltaTime * temperature / 4;
                    Rb.AddForce(Vector2.up * 2);
                }
                else
                {
                    temperature -= Time.deltaTime * temperature / 8;
                    Rb.AddForce(Vector2.up * 5);
                }

                if (temperature >= temperatureLimit)
                {
                    temperature -= Time.deltaTime * temperature / 2;
                    Rb.AddForce(Vector2.up * 10);
                }

                if (_currentMediumCoolDown < 0 && temperature >= temperatureAbility)
                {
                    _currentMediumCoolDown = mediumAbilityCooldown;
                    MediumAbility();
                }

                if (_currentBigCoolDown < 0 && temperature >= temperatureLimit)
                {
                    _currentBigCoolDown = bigAbilityCooldown;
                    BigAbility();
                }


                if (_difAngularSpeed < 0)
                {
                    temperature += Math.Abs(_difAngularSpeed / 4) * Time.deltaTime;
                }

                _currentBigCoolDown -= Time.unscaledDeltaTime;
                _currentMediumCoolDown -= Time.unscaledDeltaTime;
               
                
                break;

            case ObjectType.Star:
                temperature += Time.deltaTime * 10;
                if (temperature >= temperatureLimit)
                {
                    temperature = temperatureLimit;
                }

                break;
        }
    }

    public float GetTemperature()
    {
        return temperature;
    }

    public void SetTemperature(float temp)
    {
        temperature = temp;
    }

    public void SplitTemperatures(StrObject obj)
    {
        var temp = (temperature + obj.GetTemperature()) / 2;
        temperature = temp;
        obj.SetTemperature(temp);
    }

    public abstract void MediumAbility();

    public abstract void BigAbility();

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<StrObject>() != null)
        {
            SplitTemperatures(col.gameObject.GetComponent<StrObject>());
        }
    }

    public enum ObjectType
    {
        Star,
        Accelerator,
        Junk,
        BlackHole
    }
}