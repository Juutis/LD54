using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;
    private KnightPhase phase = KnightPhase.Walking;

    private float tmpY;
    private float tmpX;

    [SerializeField]
    private float attackCooldown;
    private float lastAttack = 0f;
    private float tmpYLerp = 0f;

    private float tmpEnemyHP = 1f;

    private float lootTime = 0.5f;
    private float lootStarted = 0f;

    private Enemy currentEnemy = null;

    // Start is called before the first frame update
    void Start()
    {
        tmpY = transform.localPosition.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (phase == KnightPhase.Walking) 
        {
            transform.localPosition = new Vector2(transform.localPosition.x + moveSpeed * Time.deltaTime, tmpY + Mathf.Abs(Mathf.Sin(Time.time * 8) * 0.4f));
        }
        else if (phase == KnightPhase.Fighting)
        {
            transform.localPosition = new Vector2(transform.localPosition.x, Mathf.Lerp(transform.localPosition.y, tmpY, tmpYLerp));
            tmpYLerp = Mathf.Min(1, tmpYLerp + Time.deltaTime);

            if (Time.time - lastAttack > attackCooldown)
            {
                transform.localPosition = new Vector2(transform.localPosition.x, tmpY + 1f);
                tmpYLerp = 0;
                lastAttack = Time.time;
                tmpEnemyHP--;
            }

            if (tmpEnemyHP <= 0 && tmpYLerp >= 1)
            {
                phase = KnightPhase.Looting;
                lootStarted = Time.time;
                tmpX = transform.localPosition.x;
                tmpEnemyHP = 1f;
            }
        }
        else
        {
            //transform.localPosition = new Vector2(tmpX + Mathf.Abs(Mathf.Sin(Time.time * 8) * 0.4f), tmpY + Mathf.Abs(Mathf.Sin(Time.time * 8) * 0.4f));

            transform.localPosition = new Vector2(tmpX + Mathf.Abs(Mathf.Sin(Time.time * 8) * 0.4f), tmpY + Mathf.Abs(Mathf.Sin(Time.time * 8) * 0.4f));
            if (currentEnemy == null || currentEnemy.ItemCount() == 0)
            {
                Debug.Log($"Back to walk: {currentEnemy == null} || {currentEnemy.ItemCount() == 0}");
                phase = KnightPhase.Walking;
            } 
            else if(Time.time - lootStarted > lootTime)
            {
                lootStarted = Time.time;
                currentEnemy.Loot();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("Collided");
        if (collision.TryGetComponent<Enemy>(out Enemy enemy))
        {
            currentEnemy = enemy;
            phase = KnightPhase.Fighting;
        }
    }
}

enum KnightPhase {
    Walking,
    Fighting,
    Looting
}