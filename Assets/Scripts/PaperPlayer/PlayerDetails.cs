using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDetails : MonoBehaviour
{
    // Start is called before the first frame update
    public int playerID;

    public Vector3 startPos;

    public Material[] PlayerColours = new Material[4];

    // Start is called before the first frame update

    private void Start()
    {
        GetComponent<Renderer>().material = PlayerColours[playerID - 1];

        transform.position = startPos;
    }
}