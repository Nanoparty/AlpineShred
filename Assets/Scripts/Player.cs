using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

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

    [SerializeField] float XCollisionForce = 5f;
    [SerializeField] float YCollisionForce = 5f;
    [SerializeField] float ZCollisionForce = 5f;

    [SerializeField] public Vector2 HorizontalLimits;


    public TMP_Text ScoreText;
    public TMP_Text HealthText;

    private Rigidbody rb;

    private GameManager gm;

    private bool dead;
    private bool paused;

    private Vector3 tempVel;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    public void Pause()
    {
        paused = true;
        tempVel = rb.velocity;
        rb.constraints |= RigidbodyConstraints.FreezePositionX;
    }

    public void Unpause()
    {
        paused = false;
        rb.constraints &= ~RigidbodyConstraints.FreezePositionX;
        rb.velocity = tempVel;
    }

    private void Update()
    {
        if (paused)
        {

            return;
        }

        // Check Death
        if (health <= 0)
        {
            gm.GameOver = true;
            dead = true;

            rb.constraints = RigidbodyConstraints.None;

            GetComponent<Rigidbody>().AddForceAtPosition(new Vector3(
                Random.Range(-XCollisionForce, XCollisionForce),
                YCollisionForce,
                ZCollisionForce),
                new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z));
        }

        Vector3 updatePosition = transform.position;
        Vector3 updateVelocity = rb.velocity;
        Vector3 updateRotation = transform.Find("Model").transform.rotation.eulerAngles;

        if (updateRotation.y > 180) updateRotation.y = -(360 - updateRotation.y);
        if (updateRotation.z > 180) updateRotation.z = -(360 - updateRotation.z);

        if (Input.GetKey(KeyCode.A)) {
            //Move Left
            updateVelocity.x -= speed * Time.deltaTime;
            updateRotation.y += pitchSpeed * Time.deltaTime;
            updateRotation.z += rollSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            //Move Right
            updateVelocity.x += speed * Time.deltaTime;
            updateRotation.y += -pitchSpeed * Time.deltaTime;
            updateRotation.z += -rollSpeed * Time.deltaTime;
        }
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) 
        {
            if (updateRotation.y < 0) updateRotation.y += Mathf.Min(Mathf.Abs(updateRotation.y - 0), returnSpeed);
            if (updateRotation.y > 0) updateRotation.y -= Mathf.Min(Mathf.Abs(updateRotation.y - 0), returnSpeed);
            if (updateRotation.z > 0) updateRotation.z -= Mathf.Min(Mathf.Abs(updateRotation.z - 0), returnSpeed);
            if (updateRotation.z < 0) updateRotation.z += Mathf.Min(Mathf.Abs(updateRotation.z - 0), returnSpeed);
        }

        //Restrict Movement Beyond Limits
        if (updatePosition.x > HorizontalLimits.y) updatePosition.x = HorizontalLimits.y;
        if (updatePosition.x < HorizontalLimits.x) updatePosition.x = HorizontalLimits.x;

        if (updateRotation.y > maxPitch) updateRotation.y = maxPitch;
        if (updateRotation.y < -maxPitch) updateRotation.y = -maxPitch;

        if (updateRotation.z > maxRoll) updateRotation.z = maxRoll;
        if (updateRotation.z < -maxRoll) updateRotation.z = -maxRoll;

        // Update Velocity and Rotation
        rb.velocity = updateVelocity;
        transform.Find("Model").transform.rotation = Quaternion.Euler(updateRotation);

        ScoreText.text = "Score:" + score.ToString();
        HealthText.text = "Health:" + health.ToString();
    }

    public void ActivatePower()
    {

    }

}
