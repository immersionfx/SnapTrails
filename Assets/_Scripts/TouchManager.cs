using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class TouchManager : MonoBehaviour
{

    void Update()
    {      

        //Tap on Screen
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("@@@ Touched the UI");
                return;
            }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Touch: " + hit.collider.gameObject.name);
                if (hit.transform.tag == "Like")
                    hit.collider.transform.GetComponent<LikeButton>().ButtonClicked();
                if (hit.transform.tag == "Notify")
                    hit.collider.transform.GetComponent<NotifyButton>().ButtonClicked();
            }
        }

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log("Touch: " + hit.collider.gameObject.name);
                    if (hit.transform.tag == "Like")
                        hit.collider.transform.GetComponent<LikeButton>().ButtonClicked();
                    if (hit.transform.tag == "Notify")
                        hit.collider.transform.GetComponent<NotifyButton>().ButtonClicked();
            }
            }
#endif

    }
}
