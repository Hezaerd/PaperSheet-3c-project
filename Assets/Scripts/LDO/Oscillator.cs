using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [SerializeField] private Vector3 movementVector = new Vector3(0f, 0f, 0f);
    private float movementFactor;
    [SerializeField] private float period = 4f;

    private Vector3 startingpos;

    // Start is called before the first frame update

    private void Start()
    {
        startingpos = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        if (period <= 0)
        {
            return;
        }
        float cycles = Time.time / period;

        const float tau = Mathf.PI * 2;
        float SinWave = Mathf.Sin(cycles * tau);
        movementFactor = SinWave / 2f + 0.5f;

        Vector3 offset = movementVector * movementFactor;
        transform.position = offset + startingpos;
    }
}