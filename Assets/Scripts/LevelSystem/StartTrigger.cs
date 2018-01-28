using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTrigger : BaseTrigger
{

    void Start()
    {
        Collider2D collider = GetComponent<Collider2D>();
        if (collider == null || !collider.isTrigger)
        {
            Debug.LogError("Collider isTrigger should be true");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerController exitedPlayer = other.gameObject.GetComponent<PlayerController>();
        if (exitedPlayer != null)
        {
            Debug.Log("Start Timeline!");
        }
    }


    /// <summary>
    /// Sets the Start Position for the player
    /// </summary>
    public override void InitializeLevelObject()
    {
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        if (levelManager != null)
        {
            levelManager.SetPlayersStartPosition(transform.position);
        }
    }
}
