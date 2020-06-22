using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private bool initialised = false;
    private Transform player;

    private NeuralNetwork network;
    private Rigidbody2D rocketBody;

    // Start is called before the first frame update
    void Start()
    {
        rocketBody = GetComponent<Rigidbody2D>();    
    }

    public void Init(NeuralNetwork network, Transform player)
    {
        initialised = true;

        this.network = network;
        this.player = player;
    }
    // Update is called once per frame

    private readonly int INPUTS_ARRAY_LENGTH = 1;
    void Update()
    {
        if (initialised)
        {
            float[] inputs = new float[INPUTS_ARRAY_LENGTH];

            float angle = transform.eulerAngles.z % 360;

            if (angle < 0f)
            {
                angle += 360f;
            }

            Vector2 vectorToPlayer = (player.position - transform.position);

            float degrees = Mathf.Atan2(vectorToPlayer.y, vectorToPlayer.x) * Mathf.Rad2Deg;

            degrees %= 360;

            if (degrees < 90f)
            {
                degrees += 360f;
            }

            degrees = 360 - degrees;

            degrees -= angle;

            if (degrees < 0f)
            {
                degrees += 360;
            }

            if (degrees >= 180f)
            {
                degrees = 360 - degrees;
                degrees *= -1f;
            }

            float radians = degrees * Mathf.Deg2Rad;

            inputs[0] = radians / Mathf.PI;

            float[] output = network.CalculateValues(inputs);

            rocketBody.velocity = 2.5f * transform.up;
            rocketBody.angularVelocity = 500f * output[0];

            network.AddFitness(1f - Mathf.Abs(inputs[0]));
        }
    }

    public void Destoryrocket()
    {
        Destroy(gameObject);
    }
}
