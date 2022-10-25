using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour {

    public int index;

    public Animator animator;

    public bool onPlayingField = false;

    [SerializeField] private TMP_Text indexText;

    private DeckManager deckManager;

    public void OnStart(DeckManager dm) {
        deckManager = dm;
        animator = GetComponent<Animator>();
        indexText.text = index.ToString();
    }

    public void OnUpdate() {

    }

    public void OnMouseOver() {

        if(onPlayingField) {
            if(Input.GetMouseButtonDown(0)) {
                deckManager.ViewCard(this);
            }
        }

        // do outline thingy maybe?

    }

    public IEnumerator MoveToTarget(Vector3 targetPosition, float moveTime) {
        
        float counter = 0;

        while(counter <= moveTime-1) {
            counter += Time.deltaTime;
            transform.position = Vector3.Slerp(transform.position,targetPosition,counter/moveTime);
            yield return new WaitForEndOfFrame();
        }

        transform.position = targetPosition;
        
    }

    public IEnumerator RotateToTarget(Quaternion targetRotation, float moveTime) {

        float counter = 0;
        
        while(counter <= moveTime-1) {
            counter += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation,targetRotation,counter/moveTime);
            yield return new WaitForEndOfFrame();
        }

        transform.rotation = targetRotation;

    }

}