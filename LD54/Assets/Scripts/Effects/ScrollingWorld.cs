using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScrollingWorld : MonoBehaviour
{
    public static ScrollingWorld Instance;

    private List<ScrollingImage> backgrounds;

    [SerializeField]
    private Vector2 objectScrollSpeed;

    private List<Transform> scrollingObjects = new List<Transform>();

    private bool pause;

    void Awake() {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        backgrounds = GetComponentsInChildren<ScrollingImage>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        if (!pause) {
            Vector3 posDelta = -objectScrollSpeed * Time.deltaTime;
            scrollingObjects.ForEach(it => it.position += posDelta);
        }
    }

    public void Pause(bool pause) {
        this.pause = pause;
        backgrounds.ForEach(it => it.Pause(pause));
    }

    public void AddScrollingObject(Transform transform) {
        scrollingObjects.Add(transform);
    }
}
