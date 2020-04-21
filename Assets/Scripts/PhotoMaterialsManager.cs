using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    public TextMeshProUGUI filterText1;
    public TextMeshProUGUI filterText2;
    public TextMeshProUGUI filterText3;

    private Dictionary<string, Material> mat_dict;

    public int selectedFilter; // 0 representing original 

    // Start is called before the first frame update
    void Start()
    {
        if (mat_dict == null) initDict();
        selectedFilter = 0; 
    }

    void initDict() {
        mat_dict = new Dictionary<string, Material>();
        mat_dict.Add("Brighten", brighten);
        mat_dict.Add("Black & White", blackwhite);
        mat_dict.Add("Sepia", sepia);
        mat_dict.Add("Sakura", sakura);
        mat_dict.Add("Hearts", hearts);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setImage(Sprite image) {
        GetComponent<SpriteRenderer>().sprite = image;
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
        if (mat_dict == null) initDict();
        if (num == 1)
        {
            filter1 = mat_dict[name]; 
            filterText1.text = name;
        }
        else if (num == 2)
        {
            filter2 = mat_dict[name]; 
            filterText2.text = name;
        }
        else
        {
            filter3 = mat_dict[name]; 
            filterText3.text = name;
        }
    }
}
