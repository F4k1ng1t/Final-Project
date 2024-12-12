using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class HurtBox : MonoBehaviourPunCallbacks
{
    public GameObject attackPoint1;
    public GameObject attackPoint2;
    public GameObject attackPoint3;

    public int percent = 0;

    public TextMeshProUGUI percentage;

    Rigidbody2D rig;
    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }
    public void Update()
    {
        percentage.text = percent.ToString();
    }
    [PunRPC]
    void UpdatePercent(int amount)
    {
        percent += amount;
       
    }
    [PunRPC]
    public void Launch(float angle, int power, int direction)
    {
        // Convert angle from degrees to radians
        float radians = angle * Mathf.Deg2Rad;

        // Calculate x and y velocity components based on the angle and power
        float x = Mathf.Cos(radians);
        float y = Mathf.Sin(radians);

        // Apply force to the rigidbody in the direction of the launch
        rig.AddForce(new Vector2(x * power * direction, y * power), ForceMode2D.Impulse);
    }
}
