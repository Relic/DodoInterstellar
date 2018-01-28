using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ButtonTrigger : BaseTrigger
{
    public Sprite NotTriggeredSprite;
    public Sprite TriggeredSprite;

    // Use this for initialization
    void Start()
    {
        Collider2D collider = GetComponent<Collider2D>();
        if (collider == null || !collider.isTrigger)
        {
            Debug.LogError("Collider isTrigger should be true");
        }
        UpdateSprites();
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController enteredPlayer = other.gameObject.GetComponent<PlayerController>();
        if (enteredPlayer != null)
        {
            _triggeredPlayers.Add(enteredPlayer);
            LevelObjectReference.OnTriggerEnter();
            UpdateSprites();
        }
    }

    /// <summary>
    /// Testing Method, to check if a player object is colliding with the trigger object
    /// </summary>
    private void UpdateSprites()
    {
        if (_triggeredPlayers.Count > 0)
        {
            GetComponent<SpriteRenderer>().sprite = TriggeredSprite;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = NotTriggeredSprite;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerController exitedPlayer = other.gameObject.GetComponent<PlayerController>();
        if (exitedPlayer != null)
        {
            if (!_triggeredPlayers.Remove(exitedPlayer))
            {
                Debug.LogError("Player object left the trigger without properly entered it");
            }
            LevelObjectReference.OnTriggerExit();
            UpdateSprites();
        }
    }

    public override void InitializeLevelObject()
    {

    }
}
