using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]
public class CameraFollower : MonoBehaviour
{
    [SerializeField] private float damping = 1.5f;
    [SerializeField] private Vector2 offset = new Vector2(2f, 1f);
    [SerializeField] private Camera _camera;
    
    private Transform _player;
    private float _fullScreenSize;
    [SerializeField] private float _playerScreenSize;
    private bool _fullScreen = false;
    private Vector3 _fullScreenPos;

    void Start()
    {
        if (_camera == null)
        {
            _camera = GetComponent<Camera>();
        }
        
        _fullScreenPos = transform.position;
        _fullScreenSize = _camera.orthographicSize;
        
        _camera.orthographicSize = _playerScreenSize;
        
        Application.targetFrameRate = 60;
        offset = new Vector2(Mathf.Abs(offset.x), offset.y);
        Invoke(nameof(FindPlayer), 0.02f);
    }

    public void FindPlayer()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        //camera.orthographicSize = 68;

        var transform1 = transform;
        var position = _player.position;
        transform1.position = new Vector3(position.x + offset.x, position.y + offset.y, transform1.position.z);
        transform1.rotation = new Quaternion(0, 0, 0, 0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SwitchFullScreen();
        }
        
        //Если _fullScreen = true, то ортографический размер камеры будет медленно увеличиваться до _fullScreenSize
        if (_fullScreen)
        {
            
            if (Mathf.Abs(_fullScreenSize - _camera.orthographicSize) > 0.1f)
            {
                _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _fullScreenSize, Time.deltaTime * damping);
            }
            else
            {
                _camera.orthographicSize = _fullScreenSize;
            }
        }
        else
        {
            if (Mathf.Abs(_playerScreenSize - _camera.orthographicSize) > 0.1f)
            {
                _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _playerScreenSize, Time.deltaTime * damping);
            }
            else
            {
                _camera.orthographicSize = _playerScreenSize;
            }
        }
        
        if (_player)
        {
            var playerPosition = _player.position;
            var transform1 = transform;
            var position1 = transform1.position;
            Vector3 target;
            if (!_fullScreen)
            {
                target = new Vector3(playerPosition.x + offset.x, playerPosition.y + offset.y, position1.z);

            }
            else
            {
                target = _fullScreenPos;
            }
            Vector3 currentPosition = Vector3.Lerp(position1, target, damping * Time.unscaledDeltaTime);
            transform.position = currentPosition;
        }

        
    }

    public void SwitchFullScreen()
    {
        _fullScreen = !_fullScreen;
        
    }
}