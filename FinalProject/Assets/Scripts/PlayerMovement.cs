using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum State
{
    RUNNING,
    WALKING,
    JUMPSQUAT,
    IDLEGROUND,
    IDLEAIR,
    HITSTUN,
    INACTIONABLE
}
public class PlayerMovement : MonoBehaviourPunCallbacks
{
    State state = State.IDLEAIR;

    [HideInInspector]
    public int id;

    [Header("Components")]
    public Rigidbody2D rig;
    public Animator animator;

    [Header("Movement Stats")]
    public float runSpeed;
    public float walkSpeed;
    public float airSpeed;
    public float shortHopHeight;
    public float fullHopHeight;
    public float doubleJumpHeight;
    
    //Jump stuff
    bool isGrounded; //Checks if grounded
    bool hasDoubleJump; //Whether the player has used their double jump
    bool hasJumped; //
    bool isJumping;
    int holdFrames = 0;
    int shortHopFrames = 3;

    public Player photonPlayer;


    //Check for running
    bool isRunning = false;
    bool isWalking = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

    [PunRPC]
    public void Initialize(Player player)
    {
        photonPlayer = player;
        id = player.ActorNumber;

        GameManager.instance.players[id - 1] = this;
        GameManager.instance.targetgroup.AddMember(this.transform, 1, 4);

        if (!photonView.IsMine)
            rig.simulated = false;
    }
    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        HandleInput();
    }
    void Run(float direction)
    {
        rig.velocity = new Vector2(direction * runSpeed, rig.velocity.y);
        this.transform.localScale = new Vector2(direction / Mathf.Abs(direction), this.transform.localScale.y);
    }
    void Walk(float direction)
    {
        rig.velocity = new Vector2(direction * walkSpeed, rig.velocity.y);
        this.transform.localScale = new Vector2(direction / Mathf.Abs(direction), this.transform.localScale.y);
    }
    void Drift(float direction)
    {
        rig.velocity = new Vector2(direction * airSpeed, rig.velocity.y);
    }
    void HandleJumpInput()
    {
        if (!hasJumped && GroundCheck())
        {
            
            if (holdFrames <= shortHopFrames)
            {
                ShortHop(); // Perform a short hop if within the threshold
                //Debug.Log(holdFrames);
            }
            else
            {
                FullHop(Input.GetAxis("Horizontal")); // Perform a full hop if beyond the threshold
                //Debug.Log(holdFrames);
            }
            holdFrames = 0;
            Debug.Log(holdFrames);
        }
    }
    void ShortHop()
    {
        Debug.Log("Shorthop");
        hasJumped = true;
        rig.AddForce(new Vector2(0, shortHopHeight), ForceMode2D.Impulse);

    }
    void FullHop(float direction)
    {
        Debug.Log("FullHop");
        rig.AddForce(new Vector2(0, fullHopHeight), ForceMode2D.Impulse);
        rig.velocity = new Vector2(airSpeed * direction, rig.velocity.y);
    }
    void DoubleJump(float direction)
    {
        rig.velocity = new Vector2(rig.velocity.x, 0);
        rig.AddForce(new Vector2(0, doubleJumpHeight), ForceMode2D.Impulse);
        rig.velocity = new Vector2(airSpeed * direction, rig.velocity.y);
        hasDoubleJump = false;
    }
    public bool GroundCheck()
    {
        //Variables
        float rayDistance = this.transform.localScale.y * 1.52f;
        int groundLayer = LayerMask.GetMask("Ground");
        //Visualizing ray for debugging
        Debug.DrawRay(this.transform.position, Vector2.down * rayDistance,Color.black);

        if (Physics2D.Raycast(this.transform.position, Vector2.down, rayDistance, groundLayer)) 
        {
            hasJumped = false;
            hasDoubleJump = true;
            animator.SetBool("Air",false);
            return true;
        }
        UpdateState(State.IDLEAIR);
        return false;
    }
    void HandleInput()
    {
        //float rawX = Input.GetAxisRaw("Horizontal");
        
        //Collecting Input
        
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        bool shift = Input.GetKey(KeyCode.LeftShift);
       
        if (GroundCheck()) //Character is grounded
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
            if (x != 0 && !hasJumped) //Run
            {
                if (shift)
                {
                    //Debug.Log("Running");
                    UpdateState(State.RUNNING);
                }
                else
                {
                    //Debug.Log("walking");
                    UpdateState(State.WALKING);
                }
            }
            else
            {
                UpdateState(State.IDLEGROUND);
            }
            if (state == State.RUNNING)
            {
                Run(x);
            }
            if (state == State.WALKING)
            {
                Walk(x);
            }
            
            //Logic for fullhopping vs shorthopping

            hasJumped = false; // Reset jumping when grounded

            if (Input.GetKeyDown(KeyCode.Space) && !hasJumped)
            {
                isJumping = true;
                holdFrames = 0; // Reset the frame counter
                UpdateState(State.JUMPSQUAT);
            }

            if (isJumping && Input.GetKey(KeyCode.Space))
            {
                holdFrames++; // Increment frame counter each frame
                if (holdFrames >= shortHopFrames + 1)
                {
                    // Release the jump after 3 frames, with leniency for 1 extra frame
                    isJumping = false;
                    HandleJumpInput(); // Call jump input logic
                }
            }

            if (Input.GetKeyUp(KeyCode.Space) && isJumping)
            {
                isJumping = false; // Stop tracking
                HandleJumpInput(); // Call the logic function to decide the jump type
            }


        }
        else //Character is airborne
        {
            UpdateState(State.IDLEAIR);
            Drift(x);
            if (Input.GetKeyDown(KeyCode.Space) && hasDoubleJump)
            {
                DoubleJump(x);
            }
        }

    }
    void UpdateState(State stateToUpdate)
    {
        state = stateToUpdate;
        Animate();
    }
    void Animate()
    {
        switch (state)
        {
            case State.WALKING:
                animator.SetTrigger("Walk");
                break;
            case State.RUNNING:
                animator.SetTrigger("Run");
                break;
            case State.IDLEGROUND:
                animator.SetTrigger("Idle");
                break;
            case State.JUMPSQUAT:
                animator.SetTrigger("Jumpsquat");
                break;
            case State.IDLEAIR:
                animator.SetBool("Air",true);
                break;
        }
    }
}

