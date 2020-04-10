using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoMaterialsManager : MonoBehaviour
{
    public Material filter1;
    public Material filter2;
    public Material filter3;
    public Material dissolve; 
    public Material original; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectFilter1()
    {
        Debug.Log("changing to filter1"); 
        GetComponent<Renderer>().material = filter1; 
    }

    public void SelectFilter2()
    {
        Debug.Log("changing to filter2");
        GetComponent<Renderer>().material = filter2; 
    }

    public void SelectFilter3()
    {
        Debug.Log("changing to filter3");
        GetComponent<Renderer>().material = filter3; 
    }

    // POSSIBLE TODO: DISSOLVE FADE IN EFFECT 
    void DissolveTransition()
    {
        GetComponent<Renderer>().material = dissolve; 
    }

    public void RevertToOriginal()
    {
        GetComponent<Renderer>().material = original;
    }
}
