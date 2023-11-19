using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PathSetup : MonoBehaviour
{
    [Header("Tags"), Space]
    public HorizontalLayoutGroup statsLayout, tagsLayout;

    bool updated = false;

    void Start()
    {
        Invoke("Refresh", 0.2f);
    }

    void Refresh()
    {
        statsLayout.enabled = false;
        statsLayout.enabled = true;
        tagsLayout.enabled = false;
        tagsLayout.enabled = true;
    }
}
