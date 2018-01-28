using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
	private Animator animator;
	private Rigidbody2D body;
	private Vector2 lastVelocity;
	private SpriteRenderer bodyRenderer;
	private bool move;
	private bool stand;

	void Awake()
	{
		animator = GetComponent<Animator> ();
		bodyRenderer = GetComponentInChildren<SpriteRenderer> ();
		body = GetComponentInParent<PlayerController> ().GetComponent<Rigidbody2D> ();
	}

	void Update()
	{
		var vel = body.velocity.normalized;
		animator.SetFloat ( "Horizontal", vel.x );
		animator.SetFloat ( "Vertical", vel.y );

		if ( lastVelocity != vel && vel != Vector2.zero )
		{
			lastVelocity = vel;

			bodyRenderer.flipX = vel.x > 0;
		}

		if ( vel == Vector2.zero )
		{
			if ( !stand )
			{
				stand = true;
				if ( Mathf.Abs ( lastVelocity.x ) < Mathf.Abs ( lastVelocity.y ) )
				{
					if ( lastVelocity.y < 0 )
						animator.SetTrigger ( "StandFront" );
					else
						animator.SetTrigger ( "StandBack" );
				}
				else
				{
					animator.SetTrigger ( "StandSide" );
				}
				animator.ResetTrigger ( "Movement" );
			}
		}
		else
		{
			stand = false;
		}

		if ( vel != Vector2.zero )
		{
			if ( !move )
			{
				move = true;
				animator.SetTrigger ( "Movement" );
				animator.ResetTrigger ( "StandFront" );
				animator.ResetTrigger ( "StandBack" );
				animator.ResetTrigger ( "StandSide" );
			}
		}
		else
		{
			move = false;
		}
	}
}
