using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] float speed = 5f;

    [SerializeField] int damage = 1;

    [SerializeField] float XCollisionForce = 5f;
    [SerializeField] float YCollisionForce = 5f;
    [SerializeField] float ZCollisionForce = 5f;

    private void Update()
    {
        Vector3 updatePos = transform.position;

        updatePos.z += speed * Time.deltaTime;

        transform.position = updatePos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Destroyer"))
        {
            //Destroy(gameObject);
        }

        if (other.CompareTag("Player"))
        {
            Debug.Log("Hit Player");
            GetComponent<Rigidbody>().AddForceAtPosition(new Vector3(
                Random.Range(-XCollisionForce, XCollisionForce),
                YCollisionForce,
                ZCollisionForce),
                new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z));

            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().health--;
        }
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
