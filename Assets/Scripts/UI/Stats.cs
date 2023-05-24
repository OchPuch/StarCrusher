using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Temperature;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    private TextMeshProUGUI _text;
    public StatType statType;

    private RocketScript _player;
    private Rigidbody2D _playerRb;

    [SerializeField] private Material _textMaterial;

    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        GameObject player = GameObject.FindWithTag("Player");
        _player = player.GetComponent<RocketScript>();
        _playerRb = player.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        var temp = Math.Ceiling(_player.GetTemperature());
        var sped = Math.Ceiling(Vector3.Magnitude(_playerRb.velocity));
        var spen = Math.Ceiling(Math.Abs(_playerRb.angularVelocity));
        var ricochetTimer = _player.GetAbilityTimer();
        

        //чем ближе сумма temp sped spen к 7030 тем больше становится offset в Glow материала текста
        var offset = ((temp + sped + spen) / 7030);
        _textMaterial.SetFloat("_GlowPower", (float)offset);
        switch (statType)
        {
            case StatType.Temperature:
                _text.text = "Temperature: " + temp;
                if (temp > 3000)
                {
                    _text.color = Color.red;
                }
                else
                {
                    _text.color = Color.Lerp(Color.yellow, Color.red, (float)(temp / 3000));
                }

                break;
            case StatType.Speed:
                _text.text = "\nSpeed: " + sped;

                if (sped > 30)
                {
                    _text.color = Color.red;
                }
                else
                {
                    _text.color = Color.Lerp(Color.yellow, Color.red, (float)(sped / 30));
                }

                break;
            case StatType.Spin:
                _text.text = "\n\nSpin: " + spen;
                if (spen > 4000)
                {
                    _text.color = Color.red;
                }
                else
                {
                    _text.color = Color.Lerp(Color.yellow, Color.red, (float)(spen / 4000));
                }

                break;
            case StatType.RicochetTimer:
                //Show with tolerance of 0.01 with always 2 decimal places
                _text.text =  ricochetTimer.ToString("0.00", CultureInfo.InvariantCulture);
                break;
            
                
        }
    }


    public enum StatType
    {
        Speed,
        Temperature,
        Spin,
        RicochetTimer
    }
}