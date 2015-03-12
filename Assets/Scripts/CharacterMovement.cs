using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour
{
  private GameObject playerSprite;
  private Rigidbody2D playerRigidbody2D;
  private Animator anim;
  
  private float movePlayerVector;
  private bool facingRight;

  public float speed = 1.4f;

  void Awake()
  {
    playerSprite = transform.Find("PlayerSprite").gameObject;
    playerRigidbody2D = GetComponent<Rigidbody2D>();
    anim = playerSprite.GetComponent<Animator>();
  }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	  movePlayerVector = Input.GetAxis("Horizontal");
    playerRigidbody2D.velocity = new Vector2(movePlayerVector * speed, playerRigidbody2D.velocity.y);

    anim.SetFloat("speed", Mathf.Abs(movePlayerVector));

	  if ((movePlayerVector > 0 && !facingRight) || (movePlayerVector < 0 && facingRight))
	    Flip();
	}

  void Flip()
  {
    facingRight = !facingRight;

    Vector3 theScale = playerSprite.transform.localScale;
    theScale.x *= -1;
    playerSprite.transform.localScale = theScale;
  }


}
