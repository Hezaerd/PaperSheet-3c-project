using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class Colisions : MonoBehaviour
{
    /*~~~~~~~~~~~~~~~~~~~~~~~~~~Variables~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

    // Life System
    [Header("Life :")]
    public GameObject[] hearts;

    public int life;
    private bool isDead;
    public bool toRespawn;
    private bool isShield = false;

    [SerializeField] private Color32 defaultHealthColor = new Color32(255, 255, 255, 255);
    [SerializeField] private Color32 missingHealthColor = new Color32();

    // Obstacle damage
    private const int damage = 1;

    //------------ POWER UP ------------\\
    [Space]
    [Space]
    [Header("Power Up :")]
    // Shield

    [SerializeField] private float shieldDuration = 5f;

    [Space]
    public bool isSuperSpeed = false;

    [SerializeField] private Rigidbody rb;

    [SerializeField] private float speedBoostDuration = 5f;
    [SerializeField] private float speedBoostForce = 2f;

    [Space]
    // Health Pack
    [SerializeField] private int healthPackRegeneration = 1;

    [Space]
    [SerializeField]
    private GameObject Shield;

    [SerializeField]
    private GameObject boostTrail1;

    [SerializeField]
    private GameObject boostTrail2;

    private Transform CheckPoint;
    private Renderer renderer;
    public GameObject FinishUI;
    public Text time;

    private static int FinishCount;
    private float blinkingTime = 10;
    private float timeElapsed = 0;
    private float RespawnDuration = 3;
    private float val;
    private bool toBlink = false;

    private bool iFrame = false;
    public bool isFinished = false;

    private bool srt;

    [SerializeField]
    private AudioSource pickupSound;

    //  private bool haveShield = false;
    // private bool haveSpeedBoost = false;

    private void Start()
    {
        val = 0;
        srt = true;
        renderer = GetComponent<Renderer>();

        // playerPos = GameObject.FindGameObjectWithTag("Player").transform;

        //spawnPoint = GameObject.FindGameObjectWithTag("Respawn").transform;
    }

    private void Update()
    {
        StopWatch();
        if (isDead)
        {
            // INSERT DEATH CODE HERE !
        }
        if (toRespawn)
        {
            RespawnPlayer();
        }
        if (isFinished)
        {
            if (FinishCount == UI_Menu.nbPlayer)
            {
                StartCoroutine(Finishing());
            }
            Finish();
        }
    }

    private void StopWatch()
    {
        if (srt)

        {
            val += Time.deltaTime;
        }

        double b = System.Math.Round(val, 2);

        time.text = b.ToString();
    }

    private IEnumerator Finishing()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(0);
        isFinished = false;
        Timer.CountdownDone = false;
    }

    public void Finish()
    {
        FinishUI.SetActive(true);
        srt = false;
    }

    private IEnumerator Blink()
    {
        iFrame = true;
        blinkingTime = 10f;
        isShield = true;

        while (blinkingTime > 0)
        {
            toBlink = !toBlink;

            blinkingTime--;
            yield return new WaitForSeconds(0.3f);

            renderer.enabled = !toBlink;
        }
        isShield = false;
        renderer.enabled = true;
        iFrame = false;
    }

    private void FixedUpdate()
    {
        if (isSuperSpeed)
        {
            speedBoost(speedBoostForce);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isShield)
        {
            return;
        }
        if (life >= 1)
        {
            life -= damage;
            hearts[life].GetComponent<Image>().color = missingHealthColor;
        }

        if (life == 0)
        {
            toRespawn = true;
            RespawnPlayer();
            resetLife();
        }
    }

    private void resetLife()
    {
        life = 3;
        hearts[0].GetComponent<Image>().color = defaultHealthColor;
        hearts[1].GetComponent<Image>().color = defaultHealthColor;
        hearts[2].GetComponent<Image>().color = defaultHealthColor;
    }

    private IEnumerator shield(float time)
    {
        if (isShield)
        {
            yield return null;
        }
        else
        {
            isShield = true;
            Shield.SetActive(isShield);
            yield return new WaitForSeconds(time);
            isShield = false;
            Shield.SetActive(isShield);
        }
    }

    private void speedBoost(float Force)
    {
        rb.AddRelativeForce(Vector3.forward * Force * 20, ForceMode.Impulse);
    }

    private void healthPack(int Heal)
    {
        if (life == 3) return;

        hearts[life].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        life++;
    }

    private IEnumerator speedBoostTimer(float time)
    {
        isSuperSpeed = true;
        boostTrail1.SetActive(isSuperSpeed);
        boostTrail2.SetActive(isSuperSpeed);
        yield return new WaitForSeconds(time);
        isSuperSpeed = false;
        boostTrail1.SetActive(isSuperSpeed);
        boostTrail2.SetActive(isSuperSpeed);
    }

    private void RespawnPlayer()
    {
        GetComponent<PlayerControler>().rot = Vector3.zero;

        transform.position = Vector3.Lerp(transform.position, CheckPoint.position, timeElapsed / RespawnDuration);

        if (timeElapsed > RespawnDuration)
        {
            timeElapsed = 0;
            toRespawn = false;
            toBlink = false;
        }
        timeElapsed += Time.deltaTime;
        transform.rotation = CheckPoint.rotation;
    }

    private void iFrameFunc()
    {
        if (!iFrame)
        {
            TakeDamage(damage);
            StartCoroutine(Blink());
        }
    }

    /*~~~~~~~~~~~~~~~~~~~~~~~~~~Collisions~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

    private void OnCollisionEnter(Collision infoCollision)
    {
        switch (infoCollision.gameObject.tag)
        {
            case "Obstacles":
                iFrameFunc();
                break;

            case "Floor":
                iFrame = false;
                TakeDamage(damage);
                StartCoroutine(Blink());
                toRespawn = true;
                break;
        }
    }

    private void OnTriggerEnter(Collider infoTrigger)
    {
        switch (infoTrigger.gameObject.tag)
        {
            case "RespawnZone":
                CheckPoint = infoTrigger.gameObject.transform;

                break;

            case "God Mode":
                pickupSound.Play();
                StartCoroutine(shield(shieldDuration));

                break;

            case "SuperSpeed":

                pickupSound.Play();
                //haveSpeedBoost = true;
                StartCoroutine(speedBoostTimer(speedBoostDuration));
                break;

            case "Health Pack":
                pickupSound.Play();
                healthPack(healthPackRegeneration);
                break;

            case "Finish":
                FinishCount++;
                isFinished = true;

                break;
        }
    }
}