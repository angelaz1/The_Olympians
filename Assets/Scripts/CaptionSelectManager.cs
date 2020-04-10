using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CaptionSelectManager : MonoBehaviour
{
    public TMP_InputField caption;
    public TextMeshProUGUI caption1;
    public TextMeshProUGUI caption2;
    public TextMeshProUGUI caption3; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectCaption1()
    {
        caption.text = caption1.text; 
    }

    public void SelectCaption2()
    {
        caption.text = caption2.text;  
    }

    public void SelectCaption3()
    {
        caption.text = caption3.text; 
    }
}
