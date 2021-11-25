using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    [Range(0.05f, 2.0f)]
    private float m_speed = 1;
    float speed
    {
        get { return m_speed; }
        set { m_speed = Mathf.Clamp(value, 0.05f, 2.0f); }
    }
    [SerializeField]
    int TearCount = 1000;
    [SerializeField]
    GameObject TearParrent;
    List<GameObject> Tears;
    
    void Start()
    {
        //Initialise    
    }

    private void FixedUpdate()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
           
    }
}
