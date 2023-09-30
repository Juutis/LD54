using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAnimationHandler : MonoBehaviour
{
    private Animator anim;

    private Enemy currentEnemy;

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
        currentEnemy = enemy;
    }

    public void KillEnemy() {

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
}
