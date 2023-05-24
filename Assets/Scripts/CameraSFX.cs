using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]
public class CameraSFX : MonoBehaviour
{
    [SerializeField] private GameObject StarSfx;

    public void SetStarSfx(GameObject star)
    {
        StarSfx = star;

    }
}
