using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum Attacks
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
public class AttackBehavior : MonoBehaviourPunCallbacks
{

    public Attacks state = Attacks.None;
    PlayerMovement movement;
    Animator animator;
    
    void Start()
    {
        animator = this.GetComponent<Animator>();
        movement = this.GetComponent<PlayerMovement>();
    }
    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }
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
            UpdateState(Attacks.ForwardAir);
        }
        if (!movement.GroundCheck() && (Input.GetKeyDown(KeyCode.J) && direction == -stickdirection))
        {
            UpdateState(Attacks.BackAir);
        }
        if (movement.GroundCheck())
        {
            if (state == Attacks.ForwardAir || state == Attacks.BackAir || state == Attacks.UpAir) //if one of these attacks is interrupted by hitting the ground
            {
                ResetState();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.J) && state == Attacks.None)
            {
                if (Mathf.Sign(y) == 1 && y != 0) //up air
                {
                    UpdateState(Attacks.UpAir);
                    
                }
                else if (direction == stickdirection) //forward air
                {
                    UpdateState(Attacks.ForwardAir);
                }
                else if (direction == -stickdirection) //back air
                {
                    UpdateState(Attacks.BackAir);
                }
                
                else //neutral air
                {

                }
            }
        }
    }
    public void UpAir(GameObject reciever)
    {
        PlayerMovement targetPhotonView = reciever.GetComponent<PlayerMovement>();

        if (targetPhotonView != null)
        {
            // Update the percent on the target
            this.photonView.RPC("UpdatePercent", targetPhotonView.photonPlayer, 5);

            // Launch the target
            int power = (int)(10f + 0.2f * reciever.GetComponent<HurtBox>().percent);
            int direction = (int)this.transform.localScale.x;
            this.photonView.RPC("Launch", targetPhotonView.photonPlayer, 90f, power, direction);
        }
        else
        {
            Debug.LogError("PhotonView not found on receiver!");
        }
    }
    public void ForwardAir(GameObject receiver)
    {
        PlayerMovement targetPhotonView = receiver.GetComponent<PlayerMovement>();

        if (targetPhotonView != null)
        {
            // Update the percent on the target
            this.photonView.RPC("UpdatePercent", targetPhotonView.photonPlayer, 10);

            // Launch the target
            int power = (int)(5f + 0.5f * receiver.GetComponent<HurtBox>().percent);
            int direction = (int)this.transform.localScale.x;
            this.photonView.RPC("Launch", targetPhotonView.photonPlayer, -90f, power, direction);
        }
        else
        {
            Debug.LogError("PhotonView not found on receiver!");
        }
    }

    public void BackAir(GameObject receiver)
    {
        HurtBox targetPhotonView = receiver.GetComponent<HurtBox>();

        
            // Update the percent on the target
            targetPhotonView.photonView.RPC("UpdatePercent", RpcTarget.All, 7);

            // Launch the target
            int power = (int)(5f + 0.3f * receiver.GetComponent<HurtBox>().percent);
            int direction = -(int)this.transform.localScale.x;
            targetPhotonView.photonView.RPC("Launch", RpcTarget.All, 30f, power, direction);

    }
    void UpdateState(Attacks state2update)
    {
        state = state2update;
        Animate();
    }
    public void ResetState()
    {
        state = Attacks.None;
    }
    void Animate()
    {
        switch (state)
        {
            case Attacks.ForwardAir:
                animator.SetTrigger("ForwardAir");
                break;
            case Attacks.BackAir:
                animator.SetTrigger("BackAir");
                break;
            case Attacks.UpAir:
                animator.SetTrigger("UpAir");
                break;

        }
    }


}
