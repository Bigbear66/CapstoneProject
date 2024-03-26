using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifetime = 2;
    private float direction;
    private bool hit;
    private float elapsedTime;

    private Animator anim;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);

        elapsedTime += Time.deltaTime;
        if (elapsedTime > lifetime) Deactivate();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hit) return;

        boxCollider.enabled = false;
        hit = true;
        anim.SetTrigger("explode");
        Deactivate();
    }
    public void SetDirection(float _direction)
    {
        elapsedTime = 0;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
            localScaleX = -localScaleX;

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}