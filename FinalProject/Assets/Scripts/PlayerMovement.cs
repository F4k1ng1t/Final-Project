using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Characters Characters;

    [Header("Components")]
    public Rigidbody2D rig;
    public GameObject player;
    [Header("Movement Stats")]
    public float runSpeed;
    public float walkSpeed;
    public float airSpeed;
    public float shortHopHeight;
    public float fullHopHeight;
    public float doubleJumpHeight;
    
    //Check for jumps???
    bool isGrounded;
    bool hasDoubleJump;

    float xPrevious= 0;

    //Check for running
    bool isRunning = false;
    bool isWalking = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        if (GroundCheck())
        {
            Debug.Log("Grounded");
        }
        //last stick = cur stick
    }
    void Run(float direction)
    {
        rig.velocity = new Vector2(direction * runSpeed, rig.velocity.y);
        this.transform.localScale = new Vector2(direction / Mathf.Abs(direction), this.transform.localScale.y);
    }
    void Walk(float direction)
    {
        rig.velocity = new Vector2(direction * walkSpeed, rig.velocity.y);
    }
    void Drift(float direction)
    {

    }
    void ShortHop()
    {
        rig.AddForce(new Vector2(0, shortHopHeight));
    }
    void FullHop(float direction)
    {

    }
    void DoubleJump()
    {

    }
    bool GroundCheck()
    {
        //Variables
        float rayDistance = 0.51f;
        int groundLayer = LayerMask.GetMask("Ground");
        //Visualizing ray for debugging
        Debug.DrawRay(this.transform.position, Vector2.down * rayDistance,Color.black);

        if (Physics2D.Raycast(this.transform.position, Vector2.down, rayDistance, groundLayer)) return true;
        return false;
    }
    void HandleInput()
    {
        //float rawX = Input.GetAxisRaw("Horizontal");
        
        //Collecting Input
        
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        bool shift = Input.GetKey(KeyCode.LeftShift);
        //is the character grounded
        if (GroundCheck())
        {
            //    May use this later, but for now we're going with keyboard controls

            //    Debug.Log(x - xPrevious);
            //    if (Mathf.Abs(x - xPrevious) > 0.1f && !isRunning)
            //    {
            //        isRunning = true;
            //        Run(x);
            //    }
            //    if(!isRunning && Mathf.Abs(x - xPrevious) < 0.1f)
            //    {
            //        isWalking = true;
            //        Walk(x);
            //    }
            //    if (!isRunning && isWalking && Mathf.Abs(x) != 0)
            //    {
            //        Walk(x);
            //    }
            //    else if (!isRunning && isWalking && Mathf.Abs(x) == 0)
            //    {
            //        isWalking = false;
            //    }

            //    if (isRunning && Mathf.Abs(x) >= 0.8f)
            //    {
            //        Run(x);
            //    }
            //    else if (isRunning && Mathf.Abs(x) <= 0.8f)
            //    {
            //        isRunning = false;
            //    }
            //xPrevious = x;
            
            //Handling Input
            if (x != 0) //Run
            {
                if (shift)
                {
                    isRunning = true;
                }
                else
                {
                    isWalking = true;
                }
            }
            else
            {
                isRunning = false;
                isWalking = false;
            }
            if (isRunning)
            {
                Run(x);
            }
            if (isWalking)
            {
                Walk(x);
            }
            

        }
        else
        {

        }

    }
}
public enum Characters
{
    Blast,
    Gambler,
    Basic,
    Swordie
}
