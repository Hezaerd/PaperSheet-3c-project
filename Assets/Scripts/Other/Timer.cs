using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool CountdownDone = false;

    [SerializeField]
    private int countdowntTime;

    public Text countdowntDisplay;
    private float val;

    private void Start()
    {
        StartCoroutine(CountDown());
    }

    private void Update()
    {
        // StartCoroutine(CountDown());
        //StopWatch();
    }

    private IEnumerator CountDown()
    {
        while (countdowntTime > 0)
        {
            countdowntDisplay.text = countdowntTime.ToString();

            yield return new WaitForSeconds(1f);

            countdowntTime--;
        }

        countdowntDisplay.text = "GO";

        CountdownDone = true;

        yield return new WaitForSeconds(1f);

        countdowntDisplay.gameObject.SetActive(false);
    }
}