using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Navbar : MonoBehaviour
{
    public GameObject[] screens;
    public Image[] icons;
    public TMP_Text[] texts;

    public Color32 selected;
    public Color32 diselected;

    public Color32 selectedTxt, diselectedTxt;

    public void buttonPress(int id)
    {
        foreach(GameObject screen in screens)
        {
            screen.SetActive(false);
        }
        screens[id].SetActive(true);

        foreach(Image icon in icons)
        {
            icon.color = diselected;
        }
        icons[id].color = selected;

        foreach(TMP_Text txt in texts)
        {
            txt.color = diselectedTxt;
        }
        texts[id].color = selectedTxt;
    }
}
