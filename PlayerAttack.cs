using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;

    private Animator anim;
    private Player_Movement playerMovement;
    private float cooldownTimer = Mathf.Infinity;
    private bool isAttacking = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<Player_Movement>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown && playerMovement.canAttack() && !isAttacking)
            Attack();

        cooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        isAttacking = true;
        anim.SetTrigger("attack");
        cooldownTimer = 0;

        StartCoroutine(FireFireball());
    }

    private IEnumerator FireFireball()
    {
        yield return new WaitForSeconds(0.1f);

        fireballs[FindFireball()].transform.position = firePoint.position;
        fireballs[FindFireball()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));

        isAttacking = false;
    }

    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
}