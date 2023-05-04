using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] public int health = 3;
    [SerializeField] public int score = 0;

    [SerializeField] float maxPitch = 15f;
    [SerializeField] float maxRoll = 15f;
    [SerializeField] float pitchSpeed = 1f;
    [SerializeField] float rollSpeed = 1f;

    [SerializeField] float returnSpeed = 0.5f;

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
        Vector3 updateRotation = transform.Find("Model").transform.rotation.eulerAngles;

        //Debug.Log("Starting Rotation: " + updateRotation);

        if (updateRotation.y > 180) updateRotation.y = -(360 - updateRotation.y);
        if (updateRotation.z > 180) updateRotation.z = -(360 - updateRotation.z);

        if (Input.GetKey(KeyCode.A)) {
            //Move Left
            //updatePosition.x -= speed * Time.deltaTime;
            //rb.AddForce(new Vector3(-speed * Time.deltaTime, 0, 0));
            updateVelocity.x -= speed * Time.deltaTime;
            updateRotation.y += pitchSpeed * Time.deltaTime;
            updateRotation.z += rollSpeed * Time.deltaTime;

            //Debug.Log("INPUT A");
        }
        if (Input.GetKey(KeyCode.D))
        {
            //Move Right
            //updatePosition.x += speed * Time.deltaTime;
            //rb.AddForce(new Vector3(speed * Time.deltaTime, 0, 0));
            updateVelocity.x += speed * Time.deltaTime;
            updateRotation.y += -pitchSpeed * Time.deltaTime;
            updateRotation.z += -rollSpeed * Time.deltaTime;

            //Debug.Log("INPUT D");
        }
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) 
        {
            if (updateRotation.y < 0) updateRotation.y += Mathf.Min(Mathf.Abs(updateRotation.y - 0), returnSpeed);
            if (updateRotation.y > 0) updateRotation.y -= Mathf.Min(Mathf.Abs(updateRotation.y - 0), returnSpeed);
            if (updateRotation.z > 0) updateRotation.z -= Mathf.Min(Mathf.Abs(updateRotation.z - 0), returnSpeed);
            if (updateRotation.z < 0) updateRotation.z += Mathf.Min(Mathf.Abs(updateRotation.z - 0), returnSpeed);

            //Debug.Log("NO INPUT");
        }

        //Restrict Movement Beyond Limits
        if (updatePosition.x > HorizontalLimits.y)
        {
            updatePosition.x = HorizontalLimits.y;
            //if (rb.velocity.x > 0) rb.velocity = new Vector3(0,0,0);
        }
        if (updatePosition.x < HorizontalLimits.x)
        {
            updatePosition.x = HorizontalLimits.x;
            //if (rb.velocity.x < 0) rb.velocity = new Vector3(0, 0, 0);
        }

        if (updateRotation.y > maxPitch)
        {
            updateRotation.y = maxPitch;
            //Debug.Log("Max Pitch Positive");
        }
        if (updateRotation.y < -maxPitch)
        {
            updateRotation.y = -maxPitch;
            //Debug.Log("Max Pitch Negative");
        }

        if (updateRotation.z > maxRoll) updateRotation.z = maxRoll;
        if (updateRotation.z < -maxRoll) updateRotation.z = -maxRoll;

        //Set Updated Position
        //transform.position = updatePosition;
        rb.velocity = updateVelocity;
        //Debug.Log("Ending Rotation: " + updateRotation);
        transform.Find("Model").transform.rotation = Quaternion.Euler(updateRotation);



        ScoreText.text = "Score:" + score.ToString();
        HealthText.text = "Health:" + health.ToString();
    }

    public void ActivatePower()
    {

    }

}
