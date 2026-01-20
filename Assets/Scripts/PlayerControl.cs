using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public KeyBindings binds = new KeyBindings();

    void Awake()
    {
        binds.Load();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(binds.left)) 
        if (Input.GetKeyDown(binds.right))
            {

            }
    }

    void RotateTurn(int dir) //dir은 1또는 -1
    {
        
    }
}
