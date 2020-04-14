using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationButtonListener : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void moveToLocation(string location) {
        gameManager.moveToLocation(location);
    }
}
