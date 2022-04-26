using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class book_i : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject closedBook;
    [SerializeField]
    private CallOtherObjects COO;
    private string currentInterface;
    private GameObject openBook;
    private bool initBook = false;
    private float timeRemaining = 3;
    private bool bookOpen = false;
    void Start()
    {

        closedBook = transform.GetChild(0).gameObject;
        openBook = transform.GetChild(1).gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        currentInterface = COO.currentInterface;
        if (currentInterface == "Book")
        {
            if (!initBook){
                ChildrenRendering(closedBook,true);
                ChildrenRendering(openBook,false);
                initBook = true;
            }   
            if (timeRemaining > 0)
                timeRemaining -= Time.deltaTime;
            else if (bookOpen == true){
                ChildrenRendering(closedBook,true);
                ChildrenRendering(openBook,false);
                bookOpen = false;
                timeRemaining = 3;
            }
            else if (bookOpen == false){
                ChildrenRendering(closedBook,false);
                ChildrenRendering(openBook,true);
                bookOpen = true;
                timeRemaining = 3;
            }
        } else {
            ChildrenRendering(closedBook,false);
            ChildrenRendering(openBook,false);
        }    
    }

    void ChildrenRendering(GameObject parent, bool isEnabled){
        foreach (Renderer r in parent.GetComponentsInChildren<Renderer>())
            r.enabled = isEnabled;
    } 
}
