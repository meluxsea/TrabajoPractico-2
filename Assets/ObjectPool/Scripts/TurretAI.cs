using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAI : MonoBehaviour
{
    public enum TurretType
    {
        Single = 1,
        Dual = 2,
        Catapult = 3,
    }

    public GameObject currentTarget;
    public Transform turretHead;

    public float attackDist = 10.0f;
    public float attackDamage;
    public float shootCoolDown;
    private float timer;
    public float lookSpeed;

    public Animator animator;

    [Header("[Turret Type]")]
    public TurretType turretType = TurretType.Single;

    public Transform muzzleMain;
    public Transform muzzleSub;
    public GameObject muzzleEffect;
    public GameObject bulletPrefab;
    private bool shootLeft = true;

    private Transform lockOnPosition;
    private Vector3 randomRotation;

    void Start()
    {
        InvokeRepeating(nameof(CheckForTarget), 0, 0.5f);
        animator = transform.GetChild(0).GetComponent<Animator>();
        randomRotation = new Vector3(0, Random.Range(0, 359), 0);
    }

    void Update()
    {
        UpdateTarget();
        HandleShooting();
    }

    private void CheckForTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackDist);
        float nearestDistance = Mathf.Infinity;
        GameObject closestTarget = null;

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (distance < nearestDistance)
                {
                    closestTarget = collider.gameObject;
                    nearestDistance = distance;
                }
            }
        }

        currentTarget = closestTarget;
    }

    private void UpdateTarget()
    {
        if (currentTarget != null)
        {
            FollowTarget();
            if (Vector3.Distance(transform.position, currentTarget.transform.position) > attackDist)
            {
                currentTarget = null;
            }
        }
        else
        {
            IdleRotate();
        }
    }

    private void FollowTarget()
    {
        Vector3 targetDirection = currentTarget.transform.position - turretHead.position;
        targetDirection.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        turretHead.rotation = turretType == TurretType.Single
            ? Quaternion.LookRotation(targetDirection)
            : Quaternion.RotateTowards(turretHead.rotation, targetRotation, lookSpeed * Time.deltaTime);
    }

    private void HandleShooting()
    {
        timer += Time.deltaTime;
        if (timer < shootCoolDown || currentTarget == null) return;

        timer = 0;
        if (animator != null)
        {
            animator.SetTrigger("Fire");
        }
        ShootProjectile();
    }

    private void IdleRotate()
    {
        if (turretHead.rotation != Quaternion.Euler(randomRotation))
        {
            turretHead.rotation = Quaternion.RotateTowards(turretHead.rotation, Quaternion.Euler(randomRotation), lookSpeed * Time.deltaTime * 0.2f);
        }
        else
        {
            randomRotation = new Vector3(0, Random.Range(0, 359), 0);
        }
    }

    private void ShootProjectile()
    {
        Transform muzzle = turretType == TurretType.Dual && !shootLeft ? muzzleSub : muzzleMain;
        shootLeft = !shootLeft;

        Instantiate(muzzleEffect, muzzle.position, muzzle.rotation);
        GameObject projectileInstance = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
        Projectile projectile = projectileInstance.GetComponent<Projectile>();
        projectile.target = currentTarget.transform;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDist);
    }
}

