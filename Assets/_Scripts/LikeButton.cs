using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LikeButton : MonoBehaviour
{

    private Animator anim;
    private AudioSource audioS;
    public GameObject heartsPrefab;
    [SerializeField] private GameObject[] stars; //5 slots

    private int starsCount = 0;

    void Start()
    {
        anim = GetComponent<Animator>();
        audioS = GetComponent<AudioSource>();

        foreach(GameObject star in stars)
            star.SetActive(false);        
    } 
    
    public void ButtonClicked()
    {        
        anim.SetTrigger("Click");
        audioS.Play();
        Instantiate(heartsPrefab, transform.position, Quaternion.LookRotation(-transform.forward, transform.up));

        if (starsCount < 5) {            
            stars[starsCount].SetActive(true);
            starsCount++;
        }
    }
}
