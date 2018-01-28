using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTrigger : BaseTrigger
{
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
        PlayerController exitedPlayer = other.gameObject.GetComponent<PlayerController>();
        if (exitedPlayer != null && exitedPlayer.IsMainPlayer)
        {
            LevelManager levelManager = FindObjectOfType<LevelManager>();
            if (levelManager != null)
            {
                levelManager.LevelDone();
            }
            else
            {
                Debug.LogError("No LevelManager found in Scene");
            }
        }
    }

    public override void InitializeLevelObject()
    {

    }
}
