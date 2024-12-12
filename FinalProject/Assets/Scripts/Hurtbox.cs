using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBox : MonoBehaviour
{
    public GameObject attackPoint1;
    public GameObject attackPoint2;
    public GameObject attackPoint3;

    public int percent = 0;

    Rigidbody2D rig;
    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("erm is this thing on");
        if (collision.gameObject == attackPoint1 || collision.gameObject == attackPoint2 || collision.gameObject == attackPoint3)
        {
            Debug.Log("Self Collision");
            return;
        }
    }
    public void Launch(float angle, int power)
    {
        // Convert angle from degrees to radians
        float radians = angle * Mathf.Deg2Rad;

        // Calculate x and y velocity components based on the angle and power
        float x = Mathf.Cos(radians);
        float y = Mathf.Sin(radians);

        // Apply force to the rigidbody in the direction of the launch
        rig.AddForce(new Vector2(x * power, y * power), ForceMode2D.Impulse);
    }
}
