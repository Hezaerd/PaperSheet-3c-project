using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private Camera MainCam;

    [SerializeField] private Rigidbody Player;

    private Vector3 OriginalPos;
    private Vector3 Newpos;

    private float maxFOV = 110f;
    private float startFOV = 100.0f;

    // Start is called before the first frame update
    private void Start()
    {
        MainCam = GetComponent<Camera>();
        OriginalPos = MainCam.transform.position;
        Newpos = Player.transform.position - OriginalPos;

        MainCam.fieldOfView = startFOV;
    }

    // Update is called once per frame
    private void Update()
    {
        MainCam.transform.rotation = Quaternion.Euler(
        Mathf.Clamp(Player.transform.rotation.x, 45f, 55f),
        0,
        Mathf.Clamp(Player.transform.rotation.z, -10.0f, 10.0f)
        );
        MainCam.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y - Newpos.y, Player.transform.position.z - Newpos.z);

        if (Player.velocity.magnitude > 18.5f)
        {
            if (MainCam.fieldOfView < maxFOV)
            {
                MainCam.fieldOfView += (Player.velocity.magnitude * Time.deltaTime);
            }
        }
        else
        {
            if (MainCam.fieldOfView > startFOV)
            {
                MainCam.fieldOfView += (-(Player.velocity.magnitude) * Time.deltaTime - 0.01f);
            }
            else
            {
                MainCam.fieldOfView = startFOV;
            }
        }
    }
}