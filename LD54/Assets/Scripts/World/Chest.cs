using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        ScrollingWorld.Instance.AddScrollingObject(transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open() {
        anim.Play("Open");
    }
}
