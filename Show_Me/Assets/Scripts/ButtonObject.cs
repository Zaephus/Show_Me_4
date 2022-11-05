using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonObject : MonoBehaviour {

    public UnityEvent buttonEvent;
    public void OnMouseDown() {
        buttonEvent.Invoke();
    }

}