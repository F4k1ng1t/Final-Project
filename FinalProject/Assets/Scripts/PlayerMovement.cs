using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Characters Characters;

    [Header("Components")]
    public Rigidbody2D rig;
    [Header("Movement Stats")]
    public float runSpeed;
    public float walkSpeed;
    public float airSpeed;
    public float shortHopHeight;
    public float fullHopHeight;
    
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
    }
    void Walk(float direction)
    {
        rig.velocity = new Vector2(direction * walkSpeed, rig.velocity.y);
    }
    void Drift(float direction)
    {

    }
    void ShortHop(float direction)
    {

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
        float rawX = Input.GetAxisRaw("Horizontal");
        float x = Mathf.Round(rawX * 10f) / 10f;
        //Debug.Log($"Rounded Input: {x}");
        float y = Input.GetAxis("Vertical");
        //is the character grounded
        if (GroundCheck())
        {
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

            xPrevious = x;

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
