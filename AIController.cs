using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    private GameObject rocketGameObject;
    private Transform playerTransform;
    private GameObject target;

    private bool evolving = false;

    private int iteration = 0;
    private int[] layers = new int[] { 1, 10, 10, 1 };//number of neurons in each layer

    private Vector2 spawnPoint;

    private List<NeuralNetwork> networkList;

    private List<Rocket> rocketList = null;

    private readonly int NUMBER_OF_INSTANCES = 10;

    // Update is called once per frame
    void Update()
    {
        spawnPoint = playerTransform.position;

        if (!evolving)
        {
            if (iteration == 0)
            {
                StartRockets();
            } 
            else
            {
                networkList.Sort();
                for (int i = 0; i < NUMBER_OF_INSTANCES / 2; i++)
                {
                    networkList[i] = new NeuralNetwork(networkList[i + (NUMBER_OF_INSTANCES / 2)]);
                    networkList[i].Evolve();
                    networkList[i + (NUMBER_OF_INSTANCES / 2] = new NeuralNetwork(networkList[i + (NUMBER_OF_INSTANCES / 2)]);
                }

                for (int i = 0; i < NUMBER_OF_INSTANCES; i++)
                {
                    networkList[i].SetFitness(0f);
                }
            }
            iteration++;

            evolving = true;

            InitRockets();

            Invoke("Reset", 20f);
        }
    }

    private void Reset()
    {
        evolving = false;
    }

    private void InitRockets()
    {
        if (rocketList != null)
        {
            for (int i = 0; i < rocketList.Count; i++)
            {
                if (rocketList[i] == null)
                {
                    continue;
                }
                Destroy(rocketList[i].gameObject);
            }
        }

        rocketList = new List<Rocket>();

        for (int i = 0; i < NUMBER_OF_INSTANCES; i++)
        {
            Rocket rocketInstance = (Instantiate(rocketGameObject,
                                new Vector3(
                                    Random.Range(-4, 4) + target.transform.position.x,
                                    Random.Range(-4, 4) + target.transform.position.y, 0),
                                rocketGameObject.transform.rotation)).GetComponent<Rocket>();
            rocketInstance.Init(networkList[i], playerTransform.transform);

            rocketList.Add(rocketInstance);
        }
    }

    void StartRockets()
    {
        networkList = new List<NeuralNetwork>();

        for (int i = 0; i < NUMBER_OF_INSTANCES; i++)
        {
            NeuralNetwork network = new NeuralNetwork(layers);

            network.Evolve();

            networkList.Add(network);
        }
    }
}
