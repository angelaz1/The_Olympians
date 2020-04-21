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

    public int selectedCaption; // 0 as starting, referring to none 

    // Start is called before the first frame update
    void Start()
    {
        selectedCaption = 0; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectCaption1()
    {
        caption.text = caption1.text;

        ChangeSelectedTo(1); 
    }

    public void SelectCaption2()
    {
        caption.text = caption2.text;

        ChangeSelectedTo(2); 
    }

    public void SelectCaption3()
    {
        caption.text = caption3.text;

        ChangeSelectedTo(3);
    }

    void ChangeSelectedTo(int num)
    {
        selectedCaption = num; 
    }
}
