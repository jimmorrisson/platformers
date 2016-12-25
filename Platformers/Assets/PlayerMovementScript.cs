using UnityEngine;
using System.Collections;

public class PlayerMovementScript : MonoBehaviour 
{
	float horizontal;
	Rigidbody2D player;
	public Transform groundCheck;
	public LayerMask whatIsGround;
	public bool grounded;
	public float distance = 1f;
	public bool isFacingRight;
	public float speed= 2f;
	public float jumpHeight = 200f;
	public Vector2 jumpDirection;

	void Start () 
	{
		grounded = false;
		player = GetComponent<Rigidbody2D> ();
		isFacingRight = true;
	}
	
	void Update ()
	{
		jumpDirection = Vector2.zero;
		Physics2D.queriesStartInColliders = false;
		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, distance);

		grounded = Physics2D.OverlapCircle (groundCheck.position, 0.1f, whatIsGround);

		horizontal = Input.GetAxis ("Horizontal");
		if (grounded) {
			player.velocity = new Vector2 (horizontal, player.velocity.y);

			if (horizontal < 0 && isFacingRight || horizontal > 0 && !isFacingRight) 
			{
				Flip (horizontal);
			}

			if (Input.GetKeyDown (KeyCode.Space)) 
			{
				player.AddForce (new Vector2 (0, jumpHeight));
			}
		}
		else
		{
			if (Input.GetKeyDown (KeyCode.Space) && hit.collider.tag == "Wall") 
			{
					transform.localScale = transform.localScale.x == 2.3f ? new Vector2 (-2.3f, 2.9f) : new Vector2(2.3f, 2.9f); 
				player.velocity = new Vector2 (speed * hit.normal.x, speed);
			}
		}
	}

	void OnDrawGizmas()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine (transform.position, transform.position + Vector3.right * transform.localScale.x * distance);
	}

	void Flip(float horizontal)
	{
		if (horizontal > 0 && !isFacingRight || horizontal < 0 && isFacingRight) 
		{
			isFacingRight = !isFacingRight;
			Vector3 scale = transform.localScale;
			scale.x *= -1;
			transform.localScale = scale;
		}
	}
}
