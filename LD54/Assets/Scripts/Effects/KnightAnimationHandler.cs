using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KnightAnimationHandler : MonoBehaviour
{
    private Animator anim;

    private Enemy currentEnemy;
    private EnemyAnimator currentEnemyAnimator;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LootChest(Chest chest, Enemy enemy) {
        ScrollingWorld.Instance.Pause(true);
        anim.SetBool("Loot", true);
        chest.Open();
        currentEnemyAnimator = null;
        currentEnemy = enemy;
    }

    public void LootEnemy(Enemy enemy) {
        ScrollingWorld.Instance.Pause(true);
        anim.SetBool("Loot", true);
        currentEnemyAnimator = null;
        currentEnemy = enemy;
    }

    public void KillEnemy(EnemyAnimator enemyAnimator, Enemy enemy) {
        ScrollingWorld.Instance.Pause(true);
        anim.SetBool("Attack", true);
        currentEnemyAnimator = enemyAnimator;
        currentEnemy = enemy;
    }

    public void StopLoot() {
        anim.SetBool("Loot", false);
        ScrollingWorld.Instance.Pause(false);
    }

    public void ThrowLoot() {
        if (!currentEnemy.Loot()) {
            StopLoot();
        }
    }

    public void TriggerEnemyDeath() {
        if (currentEnemyAnimator == null) return;
        currentEnemyAnimator.Die();
    }

    public void AttackFinished() {
        anim.SetBool("Attack", false);
        LootEnemy(currentEnemy);
    }
}
