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
    public Material brighten;
    public Material blackwhite;
    public Material sepia;
    public Material sakura;
    public Material hearts;
    private Dictionary<string, Material> mat_dict;

    public int selectedFilter; // 0 representing original 

    // Start is called before the first frame update
    void Start()
    {
        mat_dict = new Dictionary<string, Material>();
        mat_dict.Add("brighten", brighten);
        mat_dict.Add("blackwhite", blackwhite);
        mat_dict.Add("sepia", sepia);
        mat_dict.Add("sakura", sakura);
        mat_dict.Add("hearts", hearts);

        selectedFilter = 0; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectFilter1()
    {
        Debug.Log("changing to filter1"); 
        GetComponent<Renderer>().material = filter1;

        ChangeSelectedTo(1); 
    }

    public void SelectFilter2()
    {
        Debug.Log("changing to filter2");
        GetComponent<Renderer>().material = filter2;

        ChangeSelectedTo(2);
    }

    public void SelectFilter3()
    {
        Debug.Log("changing to filter3");
        GetComponent<Renderer>().material = filter3;

        ChangeSelectedTo(3); 
    }

    void ChangeSelectedTo(int num)
    {
        selectedFilter = num; 
    }

    // POSSIBLE TODO: DISSOLVE FADE IN EFFECT 
    void DissolveTransition()
    {
        GetComponent<Renderer>().material = dissolve; 
    }

    public void RevertToOriginal()
    {
        GetComponent<Renderer>().material = original;

        ChangeSelectedTo(0); 
    }

    // change filter number num to material name 
    public void ChangeFilter(int num, string name)
    {
        if (num == 1)
        {
            filter1 = mat_dict[name]; 
        }
        else if (num == 2)
        {
            filter2 = mat_dict[name]; 
        }
        else
        {
            filter3 = mat_dict[name]; 
        }
    }
}
