using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitPlayer : MonoBehaviour
{
    private const float jumpVelocity = 10;
    private const float xSpeed = 6f;
    private const float gravity = 5;

    Vector3 velocity;

    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.sideOfCollision.getDown())
        {
            velocity.y = Mathf.Clamp(velocity.y, 0, float.MaxValue);
        }

        Vector2 userInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (player.sideOfCollision.getDown() && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = jumpVelocity;
        }

        velocity.x = userInput.x * xSpeed;
        velocity.y += gravity * Time.deltaTime;

        player.UpdateFromInitPlayer(velocity * Time.deltaTime);
    }

    public float getGravity() {
        return gravity;
    }
}
