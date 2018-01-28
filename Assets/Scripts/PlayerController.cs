using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    [Tooltip("The player object's speed in the game world.")]
    public float speed = 5f;
    [HideInInspector]
    public Vector3 StartPosition = Vector3.zero;
    [HideInInspector]
    public bool IsMainPlayer = false;

    public System.Action Killed;

    private Rigidbody2D _rigidBody;
	
    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        StartPosition = transform.position;
    }

    /// <summary>
    /// Move the player using a specific InputFrame
    /// </summary>
    /// <param name="input"></param>
    public void Move(InputFrame input)
    {
        if (input.MouseDown)
        {
            Vector2 direction = input.MousePosition - transform.position;
            if (direction.magnitude > .5) {
                direction.Normalize();
                _rigidBody.velocity = direction * speed;
            }
            else {
                _rigidBody.velocity = Vector2.zero;
            }
        }
        else if (input.HorizontalInput != 0.0f || input.VerticalInput != 0.0f)
        {
            _rigidBody.velocity = new Vector2(input.HorizontalInput, input.VerticalInput) * speed;
        }
        else _rigidBody.velocity = Vector3.zero;
    }

    /// <summary>
    /// Set the player's velocity directly
    /// </summary>
    /// <param name="velocity"></param>
    public void SetVelocity(Vector2 velocity)
    {
        _rigidBody.velocity = velocity;
    }

    public void KillPhantom()
    {
        if(!IsMainPlayer)
        {
            if(Killed != null)
            {
                Killed();
            }
        }
    }
}
