using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerControler : MonoBehaviour

{
    //Tweakable Var

    [Header("Needed player components")]
    [SerializeField]
    private Rigidbody rby = null;

    [Space]
    [Header("Players movement variables")]
    [SerializeField]
    private float minSpeed = 13.8f;

    [SerializeField]
    private float pDrag = 6f;

    [SerializeField]
    private float rotFactor = 20;

    [SerializeField]
    private float startingForce = 10f;

    [SerializeField]
    private Vector3 Gravity = new Vector3(0, -1, 0);

    [Space]
    private Colisions collisions;

    [HideInInspector]
    public Vector3 rot;
    
    private Vector2 move;

    [HideInInspector]
    public float Percentage;

    private float tiltWaitTime = 0.5f;
    private float tiltFactor = 0.002f;

    private bool firstInialisation = true;

    /*~~~~~~~~~~~~~~~~~~~~~~~~~~InputSystemFunction~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

    public void OnMovement(InputAction.CallbackContext value)
    {
        move = value.ReadValue<Vector2>();
        print(move);
    }

    /*~~~~~~~~~~~~~~~~~~~~~~~~~~UnityFrame Management~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

    // Start is called before the first frame update
    private void Start()
    {
        collisions = GetComponent<Colisions>();
        Physics.gravity = Vector3.zero;
        rot = transform.eulerAngles;
    }

    private void Update()
    {
        if (Timer.CountdownDone && !collisions.isFinished)
        {
            if (move != null)
            {
                //Rotate player
                //X
                rot.x += rotFactor * move.y * Time.deltaTime;

                rot.x = Mathf.Clamp(rot.x, -5, 45);
                //Y
                rot.y += rotFactor * move.x * Time.deltaTime;
                //Z

                rot.z += -rotFactor * move.x * Time.deltaTime;
                rot.z = Mathf.Clamp(rot.z, -5, 5);
            }

            if (rot.x < 0 && move.y == 0)
            {
                StartCoroutine(TiltDown());
            }

            transform.rotation = Quaternion.Euler(rot);
            Percentage = rot.x / 45;
        }
        else
        {
            rby.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (firstInialisation && Timer.CountdownDone)
        {
            Initialize();
        }
        if (Timer.CountdownDone && !collisions.toRespawn)
        {
            //Drag : Fast(4), Slow(6)
            float mod_drag = (Percentage * -pDrag) + pDrag;
            //Speed : Fast(13.8), Slow(12.5)
            float mod_speed = Percentage * (minSpeed - 12.5f) + minSpeed;

            rby.drag = mod_drag;
            Vector3 localV = transform.InverseTransformDirection(rby.velocity);
            localV.z = mod_speed;
            rby.velocity = transform.TransformDirection(localV);
        }
        else
        {
            //rby.constraints = RigidbodyConstraints.FreezePosition;
            rby.velocity = transform.TransformDirection(Vector3.zero);
        }
    }

    /*~~~~~~~~~~~~~~~~~~~~~~~~~~Function~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

    private void Initialize()
    {
        Physics.gravity = Gravity;
        rby.AddRelativeForce(Vector3.forward * startingForce * Time.fixedDeltaTime, ForceMode.Impulse);
        firstInialisation = false;
    }

    private IEnumerator TiltDown()
    {
        for (var i = rot.x; i < 0; ++i)
        {
            yield return new WaitForSeconds(tiltWaitTime);
            rot.x += tiltFactor;
        }
        yield break;
    }
}