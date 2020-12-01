using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonTypeControl : EventTrigger
{
    public int index;
    public int type;
    public Lenscrafter mainer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void buttonFunc () {
        mainer.mainer.currentPieceType[type] = index; 
        Debug.Log("here");
    }
}
