using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotifyButton : MonoBehaviour
{

    private Animator anim;
    private AudioSource audioS;
    public GameObject notifyPrefab;

    private int starsCount = 0;

    void Start()
    {
        anim = GetComponent<Animator>();
        audioS = GetComponent<AudioSource>();
    } 
    
    public void ButtonClicked()
    {        
        anim.SetTrigger("Click");
        audioS.Play();
        Instantiate(notifyPrefab, transform.position, Quaternion.LookRotation(-transform.forward, transform.up));
    }
}
