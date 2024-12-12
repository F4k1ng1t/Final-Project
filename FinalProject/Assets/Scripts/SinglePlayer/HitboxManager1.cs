using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxManager1 : MonoBehaviour
{
    // Store all colliders attached to child objects of the player (hitboxes)
    private Collider2D[] hitboxes;
    AttackBehavior1 attackBehavior;

    void Start()
    {
        // Collect all colliders that are children of this GameObject (Player)
        hitboxes = GetComponentsInChildren<Collider2D>();
        attackBehavior = this.GetComponent<AttackBehavior1>();
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
            if (attackBehavior.state == Attacks1.ForwardAir)
            {
                attackBehavior.ForwardAir(other.gameObject);
            }
            else if (attackBehavior.state == Attacks1.UpAir)
            {
                attackBehavior.UpAir(other.gameObject);
            }
            else if(attackBehavior.state == Attacks1.BackAir)
            {
                attackBehavior.BackAir(other.gameObject);
                Debug.Log(attackBehavior.state);
            }
            else
            {
                Debug.Log(attackBehavior.state);
            }
            
        }
    }
}
