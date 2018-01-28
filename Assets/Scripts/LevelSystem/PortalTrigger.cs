using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTrigger : BaseTrigger
{
    // Use this for initialization
    void Start()
    {
        Collider2D collider = GetComponent<Collider2D>();
        if (collider == null || !collider.isTrigger)
        {
            Debug.LogError("Collider isTrigger should be true");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController enteredPlayer = other.gameObject.GetComponent<PlayerController>();
        if (enteredPlayer != null)
        {
            LevelObjectReference.OnTriggerEnter(enteredPlayer.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerController exitedPlayer = other.gameObject.GetComponent<PlayerController>();
        if (exitedPlayer != null)
        {
            LevelObjectReference.OnTriggerExit(exitedPlayer.gameObject);
        }
    }

    public override void InitializeLevelObject()
    {

    }

    void Update()
    {
        transform.Rotate(0, 0, -50 * Time.deltaTime);
    }
}
