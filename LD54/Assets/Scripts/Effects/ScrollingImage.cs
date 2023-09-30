using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingImage : MonoBehaviour
{
    [SerializeField]
    private Vector2 scrollSpeed = Vector2.right;
    private Material mat;
    private bool pause;
    private float offSet = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (!pause) {
            offSet += Time.deltaTime;
            mat.mainTextureOffset = offSet * scrollSpeed;
        }
    }

    public void Pause(bool pause) {
        this.pause = pause;
    }
}
