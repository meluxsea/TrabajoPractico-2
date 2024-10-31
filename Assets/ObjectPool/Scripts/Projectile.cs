using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Projectile : MonoBehaviour
{
    public TurretAI.TurretType type = TurretAI.TurretType.Single;
    public Transform target;
    public bool lockOn;
    public float speed = 1;
    public float turnSpeed = 1;
    public bool catapult;
    public float knockBack = 0.1f;
    public float boomTimer = 1;
    public ParticleSystem explosion;

    private void Start()
    {
        if (catapult) lockOn = true;

        if (type == TurretAI.TurretType.Single && target != null)
        {
            RotateTowardsTarget();
        }
    }

    private void Update()
    {
        if (target == null || transform.position.y < -0.2f || boomTimer <= 0)
        {
            Explosion();
            return;
        }

        boomTimer -= Time.deltaTime;

        switch (type)
        {
            case TurretAI.TurretType.Catapult:
                HandleCatapultMovement();
                break;
            case TurretAI.TurretType.Dual:
                HandleDualMovement();
                break;
            case TurretAI.TurretType.Single:
                HandleSingleMovement();
                break;
        }
    }

    private void HandleCatapultMovement()
    {
        if (lockOn)
        {
            Vector3 launchVelocity = CalculateCatapult(target.position, transform.position, 1);
            GetComponent<Rigidbody>().velocity = launchVelocity;
            lockOn = false;
        }
    }

    private void HandleDualMovement()
    {
        Vector3 directionToTarget = target.position - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, directionToTarget, turnSpeed * Time.deltaTime, 0.0f);
        Debug.DrawRay(transform.position, newDirection, Color.red);

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    private void HandleSingleMovement()
    {
        transform.Translate(Vector3.forward * speed * 2 * Time.deltaTime, Space.World);
    }

    private void RotateTowardsTarget()
    {
        Vector3 directionToTarget = target.position - transform.position;
        transform.rotation = Quaternion.LookRotation(directionToTarget);
    }

    private Vector3 CalculateCatapult(Vector3 targetPosition, Vector3 origin, float time)
    {
        Vector3 displacement = targetPosition - origin;
        Vector3 displacementXZ = new Vector3(displacement.x, 0, displacement.z);

        float verticalDisplacement = displacement.y;
        float horizontalDistance = displacementXZ.magnitude;

        float horizontalVelocity = horizontalDistance / time;
        float verticalVelocity = verticalDisplacement / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 result = displacementXZ.normalized * horizontalVelocity;
        result.y = verticalVelocity;

        return result;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ApplyKnockBack(other.transform);
            Explosion();
        }
    }

    private void ApplyKnockBack(Transform player)
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 knockBackPosition = player.position + direction * knockBack;
        knockBackPosition.y = 1; // Set the Y position to avoid underground knockback
        player.position = knockBackPosition;
    }

    private void Explosion()
    {
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}


