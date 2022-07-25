using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    [SerializeField] private Vector3 windParameter;

    [SerializeField] private float force;

    // Start is called before the first frame update
    private void Start()
    {
    }

    private void OnTriggerStay(Collider other)
    {
        var truc = other.gameObject.GetComponent<Rigidbody>();
        truc.AddForce(windParameter * force, ForceMode.Impulse);
    }

    // Update is called once per frame
    private void Update()
    {
    }
}