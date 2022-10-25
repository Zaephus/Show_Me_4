using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour {

    public string title;

    public Animator animator;

    public bool onPlayingField = false;

    public CardStats stats;

    [SerializeField] private TMP_Text indexText;

    private DeckManager deckManager;

    public void Initialize(DeckManager dm, CardStats cs) {
        deckManager = dm;
        stats = cs;
        indexText.text = title;
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