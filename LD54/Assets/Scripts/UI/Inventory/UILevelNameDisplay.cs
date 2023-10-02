using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UILevelNameDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI txtValue;

    public void SetLevelName(string levelName)
    {
        txtValue.text = levelName;
    }
}
