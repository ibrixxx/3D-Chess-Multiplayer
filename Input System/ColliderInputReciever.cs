using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderInputReciever : InputReciever
{
    private Vector3 clickedPosition;

    private void Update() {
        if(Input.GetMouseButtonDown(0)){
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit))
            {
                clickedPosition = hit.point;
                OnInputRecieved();
            }
        }
    }

    public override void OnInputRecieved() {
        foreach (var item in inputHandlers)
        {
            item.ProcessInput(clickedPosition, null, null);
        }
    }
}
