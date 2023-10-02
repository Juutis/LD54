using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class PoppingText : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private TextMeshPro txtMessage;

    public void Show(Vector3 position, string message)
    {
        transform.position = new Vector3(position.x, position.y + 2f, position.z);
        txtMessage.text = message;
    }

    public void ShowFinished()
    {
        Debug.Log("Text destructed");
        Destroy(gameObject);
    }
}
