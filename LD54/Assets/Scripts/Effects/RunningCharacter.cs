using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningCharacter : MonoBehaviour
{
    private Animator anim;
    [SerializeField]
    private float animationSpeed = 1.0f;

    private Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.speed = animationSpeed;
        startPosition = transform.position;
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

    public void Reset() {
        transform.position = startPosition;
    }
}
