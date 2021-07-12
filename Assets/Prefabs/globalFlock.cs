using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class globalFlock : MonoBehaviour
{
    public GameObject beesPrefab;
    public static int fieldSize = 5;

    static int numBees = 10;
    public static GameObject[] allBees = new GameObject[numBees];
    public static Vector3 goalPos = Vector3.zero;


    void Start()
    {
        for (int i = 0; i < numBees; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-fieldSize, fieldSize),
                                      Random.Range(-fieldSize, fieldSize),
                                      Random.Range(-fieldSize, fieldSize));
            allBees[i] = (GameObject)Instantiate(beesPrefab, pos, Quaternion.identity);

        }

    }

    // Update is called once per frame
    void Update()
    {
        if(Random.Range(0,10000) < 50)
        {
            goalPos = new Vector3(Random.Range(-fieldSize, fieldSize),
                                      Random.Range(-fieldSize, fieldSize),
                                      Random.Range(-fieldSize, fieldSize));

        }

    }
}
