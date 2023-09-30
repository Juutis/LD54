using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        ScrollingWorld.Instance.AddScrollingObject(transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Die() {
        anim.SetBool("Die", true);
    }
}
