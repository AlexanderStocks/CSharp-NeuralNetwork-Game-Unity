﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InitPlayer : MonoBehaviour
{

    private const float JUMP_VELOCITY = 10f;
    private const float PLAYER_SPEED = 6f;
    private const float GRAVITY_STRENGTH = 5f;
        
    private float gravity;

    private Vector3 velocity;

    private Player player;

    private GameObject roundUI;
    private GameObject healthUI;

    private const int START_HEALTH = 100;
    private int health;

    private int roundNumber;

    private bool isDead;

    private GravityDirection gravityDirection;

    Player playerControl;
    void Start()
    {
        health = START_HEALTH;
        player = GetComponent<Player>();
        isDead = false;

        gravityDirection = GravityDirection.DOWN;
        gravity = GRAVITY_STRENGTH * -1;

        playerControl = GetComponent<Player>();

        health = START_HEALTH;
        isDead = false;

        roundNumber = 0;
    }

    public void KillPlayer()
    {
        Destroy(gameObject);//destory the player game object
        SceneManager.LoadScene("Death");
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            KillPlayer();
        }

        if (player.sideOfCollision.getDown())
        {
            velocity.y = Mathf.Clamp(velocity.y, 0, float.MaxValue);
        }

        if (health <= 0)
        {
            health = 0;
            isDead = true;
        }

        healthUI.gameObject.GetComponent<Text>().text = "Health" + health;
        roundUI.gameObject.GetComponent<Text>().text = "Round: " + roundNumber;


        Vector2 userInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (player.sideOfCollision.getDown() && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = JUMP_VELOCITY;
        } 
        else if (playerControl.sideOfCollision.getDown())
        {
            velocity.y = Mathf.Clamp(velocity.y, 0, float.MaxValue);
        }

        if (gravityDirection == GravityDirection.DOWN || gravityDirection == GravityDirection.UP)
        {
            if ((player.sideOfCollision.getDown() || player.sideOfCollision.getUp()) && Input.GetKeyDown(KeyCode.Space))
            {
                velocity.y = -1 * Mathf.Sign(gravity) * JUMP_VELOCITY;
            }
            velocity.x += userInput.x * Time.deltaTime;

            velocity.y = gravity * PLAYER_SPEED;
        } 
        else
        {
            if ((player.sideOfCollision.getLeft() || player.sideOfCollision.getRight()) && Input.GetKeyDown(KeyCode.Space))
            {
                velocity.x = -1 * Mathf.Sign(gravity) * JUMP_VELOCITY;
            }
            velocity.x += gravity * Time.deltaTime;

            velocity.y =  userInput.y * PLAYER_SPEED;
        }

        player.UpdateFromPlayer(velocity * Time.deltaTime);
    }

    public void DamagePlayer(int damage)
    {
        health -= damage;
    }

    public void AddHealth(int healthToAdd)
    {
        health += healthToAdd;
    }

    public float getGravity() {
        return gravity;
    }

    public void ChangeGravity()
    {
        gravityDirection = (GravityDirection)Random.Range(0, 4);

        if (gravityDirection == GravityDirection.DOWN || gravityDirection == GravityDirection.LEFT)
        {
            gravity = (gravity > 0) ? gravity * -1 : gravity;
        }
        
        if (gravityDirection == GravityDirection.UP || gravityDirection == GravityDirection.RIGHT)
        {
            gravity = (gravity < 0) ? gravity * -1 : gravity;
        }

        Invoke("ResetGravity", 10f);
    }

    private void ResetGravity()
    {
        gravityDirection = GravityDirection.DOWN;
    }

    private enum GravityDirection 
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

}
