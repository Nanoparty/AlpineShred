using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] public int health = 3;
    [SerializeField] public int score = 0;


    [SerializeField] public Vector2 HorizontalLimits;

    public TMP_Text ScoreText;
    public TMP_Text HealthText;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Vector3 updatePosition = transform.position;
        Vector3 updateVelocity = rb.velocity;
        if (Input.GetKey(KeyCode.A)) {
            //Move Left
            //updatePosition.x -= speed * Time.deltaTime;
            //rb.AddForce(new Vector3(-speed * Time.deltaTime, 0, 0));
            updateVelocity.x -= speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            //Move Right
            //updatePosition.x += speed * Time.deltaTime;
            //rb.AddForce(new Vector3(speed * Time.deltaTime, 0, 0));
            updateVelocity.x += speed * Time.deltaTime;
        }

        //Restrict Movement Beyond Limits
        if (updatePosition.x > HorizontalLimits.y) updatePosition.x = HorizontalLimits.y;
        if (updatePosition.x < HorizontalLimits.x) updatePosition.x = HorizontalLimits.x;

        //Set Updated Position
        //transform.position = updatePosition;
        rb.velocity = updateVelocity;

        ScoreText.text = "Score:" + score.ToString();
        HealthText.text = "Health:" + health.ToString();
    }

    public void ActivatePower()
    {

    }

}
