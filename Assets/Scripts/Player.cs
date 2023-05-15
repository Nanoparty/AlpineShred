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

    public float pitchAccel = 0;
    public float rollAccel = 0;
    public Vector3 updateRotation;

    public float rotationReturnSpeed = .1f;

    public TMP_Text ScoreText;
    public TMP_Text HealthText;

    private Rigidbody rb;

    private GameManager gm;

    private bool dead;
    private bool paused;

    private bool maxRotation = false;
    private bool minRotation = false;

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
        SoundManager.Instance.PlayIdle();

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

            if (!dead)
            {
                SoundManager.Instance.PlayDeath();
                SoundManager.Instance.StopIdle();
            }

            dead = true;
            minRotation = true;
            maxRotation = true;

            rb.constraints = RigidbodyConstraints.None;

            GetComponent<Rigidbody>().AddForceAtPosition(new Vector3(
                Random.Range(-XCollisionForce, XCollisionForce),
                YCollisionForce,
                ZCollisionForce),
                new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z));
        }

        Vector3 updatePosition = transform.position;
        Vector3 updateVelocity = rb.velocity;
        updateRotation = transform.Find("Model").transform.rotation.eulerAngles;

        if (updateRotation.y > 180) updateRotation.y = -(360 - updateRotation.y);
        if (updateRotation.z > 180) updateRotation.z = -(360 - updateRotation.z);

        if (Input.GetKey(KeyCode.A)) {
            //Move Left
            updateVelocity.x -= speed * Time.deltaTime;

            if (pitchAccel < 0) pitchAccel = 0;
            if (rollAccel < 0) rollAccel = 0;

            pitchAccel -= pitchSpeed * Time.deltaTime;
            rollAccel -= rollSpeed * Time.deltaTime;

            //updateRotation.y += pitchAccel;
            //updateRotation.z += rollAccel;
        }
        if (Input.GetKey(KeyCode.D))
        {
            //Move Right
            updateVelocity.x += speed * Time.deltaTime;

            if (pitchAccel > 0) pitchAccel = 0;
            if (rollAccel > 0) rollAccel = 0;

            pitchAccel += pitchSpeed * Time.deltaTime;
            rollAccel += rollSpeed * Time.deltaTime;

            //updateRotation.y += pitchAccel;
            //updateRotation.z += rollAccel;
        }
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) 
        {
            if (updateRotation.y < 0)
            {
                updateRotation.y += Mathf.Min(Mathf.Abs(updateRotation.y - 0), returnSpeed);
                maxRotation = false;
            }
            if (updateRotation.y > 0)
            {
                updateRotation.y -= Mathf.Min(Mathf.Abs(updateRotation.y - 0), returnSpeed);
                minRotation = false;
            }
            if (updateRotation.z > 0) updateRotation.z -= Mathf.Min(Mathf.Abs(updateRotation.z - 0), returnSpeed);
            if (updateRotation.z < 0) updateRotation.z += Mathf.Min(Mathf.Abs(updateRotation.z - 0), returnSpeed);

            if (pitchAccel > 0) pitchAccel -= Mathf.Min(Mathf.Abs(pitchAccel - 0), returnSpeed);
            if (rollAccel > 0) rollAccel -= Mathf.Min(Mathf.Abs(rollAccel - 0), returnSpeed);
            if (pitchAccel < 0) pitchAccel += Mathf.Min(Mathf.Abs(pitchAccel - 0), returnSpeed);
            if (rollAccel < 0) rollAccel += Mathf.Min(Mathf.Abs(rollAccel - 0), returnSpeed);
        }

        updateRotation.y += pitchAccel;
        updateRotation.z += rollAccel;

        if (updateRotation.y > 0) minRotation = false;
        if (updateRotation.y < 0) maxRotation = false;

        //Restrict Movement Beyond Limits
        if (updatePosition.x > HorizontalLimits.y) updatePosition.x = HorizontalLimits.y;
        if (updatePosition.x < HorizontalLimits.x) updatePosition.x = HorizontalLimits.x;

        if (updateRotation.y > maxPitch)
        {
            updateRotation.y = maxPitch;
            if (!maxRotation)
            {
                SoundManager.Instance.PlayTurn();
                maxRotation = true;
            }
        }
        if (updateRotation.y < -maxPitch)
        {
            updateRotation.y = -maxPitch;
            if (!minRotation)
            {
                SoundManager.Instance.PlayTurn();
                minRotation = true;
            }
        }

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
