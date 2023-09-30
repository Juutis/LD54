using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encounter : MonoBehaviour
{
    [SerializeField]
    private Transform trigger;
    private KnightAnimationHandler knight;

    [SerializeField]
    private EncounterType type;

    private bool encountered = false;

    private Enemy enemy;

    // Start is called before the first frame update
    void Start()
    {
        knight = GameObject.FindGameObjectWithTag("Knight").GetComponentInChildren<KnightAnimationHandler>();
        enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!encountered && trigger.position.x < knight.transform.position.x) {
            StartEncounter();
        }
    }

    public void StartEncounter() {
        if (type == EncounterType.CHEST) {
            var chest = GetComponent<Chest>();
            knight.LootChest(chest, enemy);
        }
        if (type == EncounterType.ENEMY) {
            var enemyAnimator = GetComponent<EnemyAnimator>();
            knight.KillEnemy(enemyAnimator, enemy);
        }
        encountered = true;
    }
}

public enum EncounterType {
    ENEMY, CHEST
}
