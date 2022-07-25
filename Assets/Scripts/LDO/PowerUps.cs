using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    //-------- Variables --------\\
    [SerializeField, Tooltip("The X rotation speed")] 
    private float RotationSpeedX = 180f;

    [SerializeField, Tooltip("The Y rotation speed")] 
    private float RotationSpeedY = 0;

    [SerializeField, Tooltip("The Z rotation speed")]
    private float RotationZSpeed = 0;

    private Renderer PowerUpRenderer;

    [SerializeField, Tooltip("The time the PowerUp take to respawn")]
    private float RespawnTime = 2f;

    private void Start()
    {
        PowerUpRenderer = GetComponent<Renderer>();
    }


    private void Update()
    {
        AnimationSpinning(RotationSpeedX, RotationSpeedY, RotationZSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Respawn(RespawnTime));
        }
    }

    //-------- Function --------\\

    private void AnimationSpinning(float X, float Y, float Z)
    {
        transform.Rotate(X * Time.deltaTime, Y * Time.deltaTime, Z * Time.deltaTime);
    }

    private IEnumerator Respawn(float time)
    {
        PowerUpRenderer.enabled = false;
        yield return new WaitForSeconds(time);
        PowerUpRenderer.enabled = true;
    }

}