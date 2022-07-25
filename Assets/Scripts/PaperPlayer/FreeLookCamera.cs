using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FreeLookCamera : MonoBehaviour
{
    //Tweakable Var
    [Header("Needed Component")]
    [SerializeField]
    private Transform _target;

    [SerializeField]
    private Rigidbody player;

    [SerializeField]
    private Colisions colisions;

    [Header("Camera Varaiables")]
    [SerializeField]
    private float _maxFOV = 71;

    [SerializeField]
    private float _startFOV = 70;

    [SerializeField]
    private float _boostSpeedFOV = 72;

    [SerializeField]
    private float _minSpeedToChangeFOV = 61;

    [SerializeField]
    private float FOVSpeedChange = 6f;

    [SerializeField] private LayerMask CeilingCheckLayers;

    [SerializeField]
    private float heightChangeSpeed = 2;

    [Header("FreeLook Variables")]
    [Space]
    [SerializeField]
    private float _mouseSensitivity = 3.0f;

    [SerializeField]
    private float _distanceFromTarget = 3.0f;

    [SerializeField]
    private float _smoothTime = 0.2f;

    [SerializeField]
    private Vector2 _rotationXMinMax = new Vector2(-40, 40);

    //Var
    private Camera mainCam;

    private Vector3 originOffset;
    private Vector3 camOffset;
    private Vector3 _currentRotation;
    private Vector3 _smoothVelocity = Vector3.zero;
    private Vector2 move;
    private float speedMagnitude;
    private float _rotationY;
    private float _rotationX;
    private bool IsFreeCam = false;

    //Display
    [SerializeField]
    private Text displayText;

    /*~~~~~~~~~~~~~~~~~~~~~~~~~~InputSystem~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

    public void OnCamera(InputAction.CallbackContext value)
    {
        move = value.ReadValue<Vector2>();
    }

    public void OnCameraSwitch(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            IsFreeCam = !IsFreeCam;

            if (displayText != null)
            {
                displayText.gameObject.SetActive(IsFreeCam);
            }
        }
    }

    /*~~~~~~~~~~~~~~~~~~~~~~~~~~UnityFrame Management~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

    private void Start()
    {
        mainCam = GetComponent<Camera>();
        camOffset = new Vector3(0, 4, -10);
        originOffset = camOffset;

        mainCam.fieldOfView = _startFOV;
        if (displayText != null)
            displayText.gameObject.SetActive(IsFreeCam);
    }

    private void Update()
    {
        CamFunction();
    }

    private void FixedUpdate()
    {
        FovChanger();
    }

    /*~~~~~~~~~~~~~~~~~~~~~~~~~~Function~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

    private void CamFunction()
    {
        if (IsFreeCam)
        {
            float mouseX = move.y * _mouseSensitivity;
            float mouseY = move.x * _mouseSensitivity;

            _rotationY += mouseY;
            _rotationX += mouseX;

            _rotationX = Mathf.Clamp(_rotationX, _rotationXMinMax.x, _rotationXMinMax.y);

            Vector3 nextRotation = new Vector3(_rotationX, _rotationY);

            _currentRotation = Vector3.SmoothDamp(_currentRotation, nextRotation, ref _smoothVelocity, _smoothTime);
            transform.localEulerAngles = _currentRotation;

            transform.position = _target.position - transform.forward * _distanceFromTarget;
        }
        else
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.up, out hit, 5f, CeilingCheckLayers))
            {
                Debug.DrawRay(transform.position, transform.up * hit.distance, Color.yellow);

                camOffset.y = Mathf.Lerp(camOffset.y, 0, heightChangeSpeed * Time.deltaTime);
            }
            else
            {
                Debug.DrawRay(transform.position, transform.up * 5, Color.white);

                camOffset.y = Mathf.Lerp(camOffset.y, originOffset.y, heightChangeSpeed * Time.deltaTime);
            }

            mainCam.transform.rotation = Quaternion.Euler(25, 0, 0);

            mainCam.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + camOffset.y, player.transform.position.z + camOffset.z);
        }
    }

    private void FovChanger()
    {
        speedMagnitude = player.velocity.magnitude;

        if (speedMagnitude > _minSpeedToChangeFOV && !colisions.isSuperSpeed)
        {
            if (mainCam.fieldOfView < _maxFOV)
            {
                mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, _maxFOV, FOVSpeedChange * Time.fixedDeltaTime);
            }
            else
            {
                mainCam.fieldOfView = _maxFOV;
            }
        }
        else if (colisions.isSuperSpeed)
        {
            if (mainCam.fieldOfView < _boostSpeedFOV)
            {
                mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, _boostSpeedFOV, FOVSpeedChange * Time.fixedDeltaTime);
            }
            else
            {
                mainCam.fieldOfView = _boostSpeedFOV;
            }
        }
        else
        {
            if (mainCam.fieldOfView > _startFOV)
            {
                mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, _startFOV, FOVSpeedChange * Time.fixedDeltaTime);
            }
            else
            {
                mainCam.fieldOfView = _startFOV;
            }
        }
    }
}