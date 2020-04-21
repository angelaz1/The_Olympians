using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbScript : MonoBehaviour
{
    private OrbManager orbManager;

    // Start is called before the first frame update
    void Start()
    {
        orbManager = GameObject.Find("OrbManager").GetComponent<OrbManager>();;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown() 
    {
        orbManager.OrbClicked(transform.position);
    }
}
