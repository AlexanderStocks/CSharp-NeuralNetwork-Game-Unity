using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    private float cameraX;
    private float cameraY;

    private bool updateX;

    private float minX, minY, maxX, maxY;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");    
    }

    // Update is called once per frame
    void Update()
    {
        float posX = player.transform.position.x;
        float posY = player.transform.position.y;
        float posZ = player.transform.position.z;

        if (posX > minX && updateX)
        {
            UpdateMinX();
            UpdateMinY();
        }

        cameraX = Mathf.Clamp(posX, minX, maxX);
        cameraY = Mathf.Clamp(posY, minY, maxY);

        gameObject.transform.position = new Vector3(cameraX, cameraY, posZ);
    }

    private void UpdateMinX()
    {
        minX += player.transform.position.x - minX;
    }

    private void UpdateMinY()
    {
        minY += player.transform.position.y - minY;
    }
}
