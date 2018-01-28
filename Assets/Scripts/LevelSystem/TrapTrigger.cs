using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTrigger : BaseTrigger
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
            LevelManager manager = FindObjectOfType<LevelManager>();
            if (manager != null)
            {
                if (enteredPlayer.IsMainPlayer)
                {
                    manager.RewindNextFixedUpdate();
                    StartCoroutine(manager.Freeze(0.5f));
                }
                else
                {
                    enteredPlayer.KillPhantom();
                }
            }
        }
    }

    public override void InitializeLevelObject()
    {

    }
}
