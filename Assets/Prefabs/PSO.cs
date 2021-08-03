using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSO : MonoBehaviour
{
    public GameObject agentPrefab;
    public GameObject globalBestPrefab;
    float sizeX = 30;
    float sizeY = 9;

    public int popsize = 20;// population size
    public int MAXITER = 3000;  // Maximum number of iterations

    float gBestCost = float.MaxValue;
    int bestParticle;
    Vector3[] geschwindigkeit;
    Vector3[] positions;
    Vector3[] pBestPositions;
    float[] pBestCosts;
    Vector3 gBestPosition;
    GameObject[] particles;
    public Transform target;
    public float waitTime = 1;
    public float maxVelocity = 10;
    public float startInertia = 0.9f;
    public float endInertia = 0.4f;
    private int iteration = 0;
    public float c1 = 2;
    public float c2 = 2;
    GameObject globalBest;

    // Start is called before the first frame update
    void Start()
    {
        particles = new GameObject[popsize];
        pBestCosts = new float[popsize];
        pBestPositions = new Vector3[popsize];
        geschwindigkeit = new Vector3[popsize];
        positions = new Vector3[popsize];

        //gBest bestimmen
        for (int i = 0; i < popsize; i++)
        {
            Vector2 pos = new Vector2(Random.Range(-sizeX, sizeX), Random.Range(-sizeY, sizeY));
            particles[i] = Instantiate(agentPrefab, pos, Quaternion.identity);
            //distanz zum gBest
            float cost = Vector3.Distance(target.position, pos);

            //Wenn Distanz kleiner als gBest-Distanz ist -> ersetzen
            if (cost < gBestCost)
            {
                gBestCost = cost;
                bestParticle = i;
                gBestPosition = pos;
            }

            //Werte des Partikels speichern
            pBestPositions[i] = pos;
            pBestCosts[i] = cost;
            positions[i] = pos;
            geschwindigkeit[i] = Vector3.ClampMagnitude(pos, maxVelocity);//.normalized;
        }

        showGlobalBest();

        //PSO durchführen
        StartCoroutine("RunPSO");
       
    }

    //PSO-Algorithmus ausführen
    IEnumerator RunPSO()
    {
        //Solange Durchläufe nicht höher als MaxDurchläufe sind
        while (iteration < MAXITER)
        {
            iteration++;
            Debug.Log("gBestCost " + gBestCost);

            //Epoche
            for (int i = 0; i < popsize; i++)
            {
                //Geschwindigkeit des Partikels bestimmen
                Vector2 vel = Vector3.ClampMagnitude(getVelocity(geschwindigkeit[i], positions[i], pBestPositions[i]), maxVelocity);//.normalized;
                //neue Position berechnen
                Vector2 pos = getPosition(positions[i], vel);
                //Distanz bestimmen zum Ziel
                float cost = Vector3.Distance(target.position, pos);
                Debug.Log("cost " + cost);

                //Wenn Distanz geringer als gBest-Distanz -> ersetzen
                if (cost < gBestCost)
                {
                    gBestCost = cost;
                    bestParticle = i;
                    gBestPosition = pos;
                }
                //geringere neue Distanz des Partikels speichern
                if (cost < pBestCosts[i])
                {
                    pBestCosts[i] = cost;
                    pBestPositions[i] = pos;
                }
                positions[i] = pos;
                geschwindigkeit[i] = vel;
                particles[i].transform.position = pos;
            }
            showGlobalBest();
            yield return new WaitForSeconds(waitTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
  
    }

    //Geschwindigkeit berechnen mithilfe der inertia-weight-Formel
    Vector3 getVelocity(Vector3 previousVelocity, Vector3 previousPosition, Vector3 pBest)
    {
        return getInertia() * previousVelocity + c1 * Random.Range(0f, 1f) * (pBest - previousPosition) + c2 * Random.Range(0f, 1f) * (gBestPosition - previousPosition);
    }

    //neue Position des Partikels berechnen
    Vector3 getPosition(Vector3 previousPosition, Vector3 currentVelocity)
    {
        return previousPosition + currentVelocity;
    }

    //Interia-Wert
    float getInertia()
    {
        return startInertia - ((startInertia - endInertia) * iteration / MAXITER);
    }

    //GlobalBest anzeigen
    void showGlobalBest()
    {
        if (globalBest != null)
        {
            Destroy(globalBest);
        }
        globalBest = Instantiate(globalBestPrefab, gBestPosition, Quaternion.identity);
    }
}

