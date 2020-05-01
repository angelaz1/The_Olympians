using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CaptionSelectManager : MonoBehaviour
{
    public TextMeshProUGUI caption;
    public TextMeshProUGUI caption1;
    public TextMeshProUGUI caption2;
    public TextMeshProUGUI caption3;
    private Caption[] captions;

    public int selectedCaption; // 0 as starting, referring to none 

    // Start is called before the first frame update
    void Start()
    {
        selectedCaption = 0; 
    }

    public void SelectCaption1()
    {
        caption.text = captions[0].captionText;

        ChangeSelectedTo(1); 
    }

    public void SelectCaption2()
    {
        caption.text = captions[1].captionText;

        ChangeSelectedTo(2); 
    }

    public void SelectCaption3()
    {
        caption.text = captions[2].captionText;

        ChangeSelectedTo(3);
    }

    void ChangeSelectedTo(int num)
    {
        selectedCaption = num; 
    }

    // return the Caption object selected, null if none selected
    public Caption getSelectedCaption() {
        if (selectedCaption == 0) return null;
        return captions[selectedCaption - 1];
    }

    void reshuffle(Caption[] captions)
    {
        for (int t = 0; t < captions.Length; t++ )
        {
            Caption tmp = captions[t];
            int r = Random.Range(t, captions.Length);
            captions[t] = captions[r];
            captions[r] = tmp;
        }
    }

    // set the captions
    public void SetCaptions(Caption[] captions)
    {
        reshuffle(captions);
        this.captions = captions;
        caption1.text = captions[0].captionButtonText;
        caption2.text = captions[1].captionButtonText;
        caption3.text = captions[2].captionButtonText;
    }
}
