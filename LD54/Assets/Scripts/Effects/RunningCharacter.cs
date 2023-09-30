using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningCharacter : MonoBehaviour
{
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
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
