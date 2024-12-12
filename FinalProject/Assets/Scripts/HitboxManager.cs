using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxManager : MonoBehaviour
{
    // Store all colliders attached to child objects of the player (hitboxes)
    private Collider2D[] hitboxes;
    AttackBehavior attackBehavior;

    void Start()
    {
        // Collect all colliders that are children of this GameObject (Player)
        hitboxes = GetComponentsInChildren<Collider2D>();
        attackBehavior = GetComponent<AttackBehavior>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Only check if the colliding object is an "Enemy"
        if (other.CompareTag("Player"))
        {
            // Loop through each hitbox and check for collisions
            for (int i = 0; i < hitboxes.Length; i++)
            {
                if (hitboxes[i].IsTouching(other)) // Check if this hitbox is colliding with the enemy
                {
                    // Debug: Log which hitbox collided
                    Debug.Log($"Hitbox {i + 1} collided with {other.name}");
                    break; // Stop checking once we've detected the first hitbox collision
                    
                }
            }
            if (attackBehavior.state == Attacks.ForwardAir)
            {
                other.gameObject.GetComponent<HurtBox>().Launch(-90, 100);
            }
            else if (attackBehavior.state == Attacks.UpAir)
            {
                attackBehavior.UpAir(other.gameObject);
            }
            else if(attackBehavior.state == Attacks.BackAir)
            {
                other.gameObject.GetComponent<HurtBox>().Launch(30, 50);
                Debug.Log(attackBehavior.state);
            }
            else
            {
                Debug.Log(attackBehavior.state);
            }
            
        }
    }
}
