﻿using System.Collections;
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
    private Filter[] filters;


    public int selectedFilter; // 0 representing original 

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

    public void setImage(Sprite image) {
        GetComponent<SpriteRenderer>().sprite = image;
        Debug.Log(image);
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

    // return the Filter object selected, null if none selected
    public Filter getSelectedFilter() {
        if (selectedFilter == 0) return null;
        return filters[selectedFilter - 1];
    }

    void reshuffle(Filter[] filters)
    {
        for (int t = 0; t < filters.Length; t++ )
        {
            Filter tmp = filters[t];
            int r = Random.Range(t, filters.Length);
            filters[t] = filters[r];
            filters[r] = tmp;
        }
    }

    // set the filters
    public void SetFilters(Filter[] filters)
    {
        if (mat_dict == null) initDict();
        reshuffle(filters);
        this.filters = filters;
        filter1 = mat_dict[filters[0].filterName]; 
        filterText1.text = filters[0].filterName; 
        filter2 = mat_dict[filters[1].filterName];
        filterText2.text = filters[1].filterName;  
        filter3 = mat_dict[filters[2].filterName];
        filterText3.text = filters[2].filterName;  
    }
}
