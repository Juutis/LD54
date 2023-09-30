using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningCharacter : MonoBehaviour
{
    private Animator anim;
    [SerializeField]
    private float animationSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.speed = animationSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Run() {
        anim.SetBool("Run", true);
    }

    public void Stop() {
        anim.SetBool("Run", false);
    }
}
