﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PostingUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // function TBD 
    public void PostPhoto()
    {
        SceneManager.LoadScene("PhoneUIDemo");
    }

    public void ReturnToMainScreen()
    {
        SceneManager.LoadScene("PhoneUIDemo");
    }
}