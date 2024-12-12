using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Attacks1
{
    None,
    ForwardAir,
    BackAir,
    UpAir,
    ForwardTilt,
    UpTilt,
    DownTilt,
    //SideSpecial,
    //UpSpecial

}
public class AttackBehavior1 : MonoBehaviour
{

    public Attacks1 state = Attacks1.None;
    PlayerMovement1 movement;
    Animator animator;
    
    void Start()
    {
        animator = this.GetComponent<Animator>();
        movement = this.GetComponent<PlayerMovement1>();
    }
    void Update()
    {
        HandleInput();
        Debug.Log(state);
    }
    void HandleInput()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float direction = this.transform.localScale.x;
        float stickdirection = 0;
        if (x != 0)
        {
            stickdirection = Mathf.Sign(x);
        }
        if (!movement.GroundCheck() && (Input.GetKeyDown(KeyCode.J) && direction == stickdirection) )
        {
            UpdateState(Attacks1.ForwardAir);
        }
        if (!movement.GroundCheck() && (Input.GetKeyDown(KeyCode.J) && direction == -stickdirection))
        {
            UpdateState(Attacks1.BackAir);
        }
        if (movement.GroundCheck())
        {
            if (state == Attacks1.ForwardAir || state == Attacks1.BackAir || state == Attacks1.UpAir) //if one of these attacks is interrupted by hitting the ground
            {
                ResetState();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.J) && state == Attacks1.None)
            {
                if (Mathf.Sign(y) == 1 && y != 0) //up air
                {
                    UpdateState(Attacks1.UpAir);
                    
                }
                else if (direction == stickdirection) //forward air
                {
                    UpdateState(Attacks1.ForwardAir);
                }
                else if (direction == -stickdirection) //back air
                {
                    UpdateState(Attacks1.BackAir);
                }
                
                else //neutral air
                {

                }
            }
        }
    }
    public void UpAir(GameObject reciever)
    {
        reciever.GetComponent<HurtBox1>().percent += 5;
        reciever.GetComponent<HurtBox1>().Launch(90, (int)(10f + 0.2f * reciever.GetComponent<HurtBox1>().percent), (int)this.transform.localScale.x);
    }
    public void ForwardAir(GameObject reciever)
    {
        reciever.GetComponent<HurtBox1>().percent += 10;
        reciever.GetComponent<HurtBox1>().Launch(-90, (int)(5f + 0.5f * reciever.GetComponent<HurtBox1>().percent), (int)this.transform.localScale.x);
    }
    public void BackAir(GameObject reciever)
    {
        reciever.GetComponent<HurtBox1>().percent += 7;
        reciever.GetComponent<HurtBox1>().Launch(30, (int)(5f + 0.3f * reciever.GetComponent<HurtBox1>().percent), -(int)this.transform.localScale.x);
    }
    void UpdateState(Attacks1 state2update)
    {
        state = state2update;
        Animate();
    }
    public void ResetState()
    {
        state = Attacks1.None;
    }
    void Animate()
    {
        switch (state)
        {
            case Attacks1.ForwardAir:
                animator.SetTrigger("ForwardAir");
                break;
            case Attacks1.BackAir:
                animator.SetTrigger("BackAir");
                break;
            case Attacks1.UpAir:
                animator.SetTrigger("UpAir");
                break;

        }
    }


}
