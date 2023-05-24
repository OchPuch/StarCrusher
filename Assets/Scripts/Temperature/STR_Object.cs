using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class StrObject : MonoBehaviour, IHeatTransfer
{
    [FormerlySerializedAs("Temperature")] public float temperature;
    
    [SerializeField]protected float temperatureLimit;

    [SerializeField] protected Rigidbody2D Rb;
    protected float DifAngularSpeed;
    protected float LastAngularSpeed;
    
    protected void Start()
    {
        if (Rb == null)
        {
            Rb = GetComponent<Rigidbody2D>();
        }

        LastAngularSpeed = Rb.angularVelocity;
    }

    protected void Update()
    {
        var angularVelocity = Rb.angularVelocity;
        DifAngularSpeed = (angularVelocity - LastAngularSpeed) / Time.deltaTime;
        LastAngularSpeed = angularVelocity;
       
    }

    public void Freeze()
    {
        throw new NotImplementedException();
    }

    public float GetTemperature()
    {
        return temperature;
    }

    public void HeatTransfer(IHeatTransfer heatTransfer, float temperature)
    {
        throw new NotImplementedException();
    }

    public void SetTemperature(float temp)
    {
        temperature = temp;
    }

    public void Explode()
    {
        throw new NotImplementedException();
    }

    public void SplitTemperatures(StrObject obj)
    {
        var temp = (temperature + obj.GetTemperature()) / 2;
        temperature = temp;
        obj.SetTemperature(temp);
    }
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<StrObject>() != null)
        {
            SplitTemperatures(col.gameObject.GetComponent<StrObject>());
        }
    }
    
}