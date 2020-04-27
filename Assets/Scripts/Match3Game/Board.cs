using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    public int xSize, ySize;
    public GameObject[] orbs;
    public GameObject [,] allOrbs;

    // Start is called before the first frame update
    void Start()
    {
        allOrbs = new GameObject[xSize, ySize];
        SetUp();
    }

    private void SetUp()
    {
        for(int i = 0; i < xSize; i++)
        {
            for(int j = 0; j < ySize; j++)
            {
                Vector2 tempPosition = new Vector2(i, j);
                int orbToUse = Random.Range(0, orbs.Length);
                GameObject orb = Instantiate(orbs[orbToUse], tempPosition, Quaternion.identity);
                orb.transform.parent = this.transform;
                orb.name = "(" + i + ", " + j + ")";
                allOrbs[i,j] = orb;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
