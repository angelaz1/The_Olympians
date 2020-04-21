﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PostingManager : MonoBehaviour
{
    private GameManager gameManager;
    private CharacterPostImages postImages;
    private CaptionSelectManager captionManager;
    private PhotoMaterialsManager photoManager;

    private PostImage currImage;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        captionManager = GameObject.Find("CaptionManager").GetComponent<CaptionSelectManager>();
        photoManager = GameObject.Find("Photo").GetComponent<PhotoMaterialsManager>();
    }

    public void setPostImages(CharacterPostImages postImages) {
        this.postImages = postImages;
    }

    public void startPostingMinigame() {
        if (captionManager == null) {
            captionManager = GameObject.Find("CaptionManager").GetComponent<CaptionSelectManager>();
        }
        if (photoManager == null) {
            photoManager = GameObject.Find("Photo").GetComponent<PhotoMaterialsManager>();
        }
        if (postImages == null) {
            Debug.Log("Something bad happened!!!");
            return;
        }

        int imagesLen = postImages.images.Length;
        int index = Random.Range(0, imagesLen);

        currImage = postImages.images[index];
        photoManager.setImage(getImage(currImage.imagePath));
        photoManager.SetFilters(currImage.filters);
        captionManager.SetCaptions(currImage.captions);
    }

    public Sprite getImage(string path) {
        return Resources.Load<Sprite>(path);
    } 

    public void PostPhoto()
    {
        Filter filter = photoManager.getSelectedFilter();
        Caption caption = captionManager.getSelectedCaption();
        gameManager.postImage(filter, caption);
        SceneManager.LoadScene("PhoneUIDemo");
    }

    public void ReturnToMainScreen()
    {
        SceneManager.LoadScene("PhoneUIDemo");
    }
}