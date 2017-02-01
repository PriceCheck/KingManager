using UnityEngine;
using System.Collections;

public class Raycaster : MonoBehaviour {
    Ray ray;
    RaycastHit hit;
    GameObject LastObjectHit;
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (LastObjectHit != hit.collider.gameObject)
            {
                if(LastObjectHit != null)
                {
                    if(LastObjectHit.GetComponent<MainMenuButton>())
                         LastObjectHit.GetComponent<MainMenuButton>().IsHighlit();
                }
                if(hit.collider.gameObject.GetComponent<MainMenuButton>())
                {
                    hit.collider.gameObject.GetComponent<MainMenuButton>().IsUnHighlit();
                }
            }
            LastObjectHit = hit.collider.gameObject;
        }
        if(Input.GetMouseButton(0))
        {
            if (LastObjectHit != null)
            {
                MainMenuButton button = LastObjectHit.GetComponent<MainMenuButton>();
                if (button)
                {
                    button.IsPushed();
                }
            }

        }
    }
}
