﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractButtonManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onHover() {
        this.GetComponent<Animator>().SetBool("isHovering", true);
    }

    public void offHover() {
        this.GetComponent<Animator>().SetBool("isHovering", false);
    }
}