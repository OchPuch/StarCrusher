using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrabScript : MonoBehaviour
{
    private Camera _camera;
    private Transform grabbedObject = null;
    private bool isGrabbing = false;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        bool grabbedObjectExist = grabbedObject;
        
        
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isGrabbing)
        {
            if (!grabbedObjectExist)
            {
                OnLeftClick();
            }
        }
        
        if(Input.GetKeyUp(KeyCode.Mouse0) && isGrabbing)
        {
            if (grabbedObjectExist)
            {
                grabbedObject = null;
                grabbedObjectExist = false;
                isGrabbing = false;
            }
        }

        if (grabbedObjectExist)
        {
            Vector3 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = grabbedObject.position.z;
            grabbedObject.position = mousePosition;
        }
    }

    private void OnLeftClick()
    {
        // Raycast из позиции мышки в направлении камеры только для объектов на слое Grabable
        RaycastHit2D hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0f,
            LayerMask.GetMask("Grabable"));
        if (hit.collider != null)
        {
            isGrabbing = true;
            //Если что-то попалось, то запоминаем объект
            grabbedObject = hit.transform;
        }
    }


}