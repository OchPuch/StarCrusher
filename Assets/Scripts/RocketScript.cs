using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketScript : StrObject
{
    [SerializeField] private float kp, ki, kd;
    
    private float _integral = 0;
    private float _dV = 0;
    private float _lastError = 0;

    [SerializeField] private int abilityCount = 3;

    private new void Start()
    {
        
        base.Start();
        _lastError = Vector3.SignedAngle(transform.up, Rb.velocity, Vector3.forward);
    }


    private void FixedUpdate()
    {
        var error = Vector3.SignedAngle(transform.up, Rb.velocity, Vector3.forward);
        _integral += error * Time.fixedDeltaTime;
        _dV = (error - _lastError) / Time.fixedDeltaTime;
        var pid = error * kp + _integral * ki + _dV * kd;
        _lastError = error;
        Rb.AddTorque(pid);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.up);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)Rb.velocity);
    }

    public override void MediumAbility()
    {
        StartCoroutine(SuperRicochetCoroutine(abilityCount));
    }

    private void FreezeRotation()
    {
        Rb.freezeRotation = true;
        Rb.isKinematic = true;
    }

    private void UnfreezeRotation()
    {
        Rb.freezeRotation = false;
        Rb.isKinematic = false;
    }

    public override void BigAbility()
    {
    }

    //SuperRicochet заставляет объект телепортироваться до ближайшего колайдера понаправлению transform.up
    //и зеркально отразить от колайдера направление движения
    //также замедляет время
    //возвращает -2, если объект не может двигаться
    //возвращает -1, если temperature меньше стоимости активации способности
    //возвращает 0, если магнитуда телепортации была меньше 40
    //возвращает 1, если магнитуда телепортации была больше 40
    private int SuperRicochet()
    {
        if (temperature < 300) return -1;
        //Обнулить скорость тела
        Rb.velocity = Vector2.zero;
        //В сторону Vector3.up создать рейкаст бесконечной длины
        var hit = Physics2D.Raycast(transform.position, transform.up, Mathf.Infinity, LayerMask.GetMask("Default"));
        //Нарисовать линию от тела до точки пересечения рейкаста
        Debug.DrawLine(transform.position, hit.point, Color.red, 0.2f);
        //Нарисовать линию нормали к точке пересечения рейкаста
        Debug.DrawLine(hit.point, hit.point + hit.normal, Color.green, 0.2f);
        var magnitude =
            Vector3.Magnitude((Vector3)hit.point - new Vector3(transform.position.x, transform.position.y, 0));

        //телепортировать тело на точку пересечения рейкаста
        transform.position = hit.point;
        //Найти угол между рейкастом и нормалью поверхности которую пересек рейкаст
        var angle = Vector3.SignedAngle(transform.up, hit.normal, Vector3.forward);
        //Повернуть тело на 90 - угол
        transform.Rotate(0, 0, (-1) * Math.Sign(angle) * 90 - angle);
        transform.position += transform.up;

        if (SFXScript.TimeFreeze(3f) != null) StopCoroutine(SFXScript.TimeFreeze(3f));
        StartCoroutine(SFXScript.TimeFreeze(3f));

        if (magnitude == 0)
        {
            return -2;
        }

        if (magnitude < 20) //Короткая длина, при которой красиво смотреть, как ракета бесконечно скачет по экрану
        {
            temperature -= 50;
            return 1;
        }


        temperature -= 300;
        return 0;
    }

    //коротина выполняющая SuperRicochet count раз с интервалом 0.2 секунды
    private IEnumerator SuperRicochetCoroutine(int count)
    {
        FreezeRotation();
        SuperRicochet();
        for (int i = 0; i < count - 1; i++)
        {
            yield return new WaitForSecondsRealtime(0.2f);
            int result = SuperRicochet();
            if (result == -1)
            {
                break;
            }

            if (result == -2)
            {
                continue;
            }

            count += result;
        }

        UnfreezeRotation();
    }
}