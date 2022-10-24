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
                deckManager.DiscardCard(this);
            }
        }

        // do outline thingy maybe?

    }

    public IEnumerator MoveToTarget(Vector3 target,float moveTime) {
        
        float startTime = Time.time;

        while(transform.position != target) {
            float completion = (Time.time - startTime) / (moveTime*2);
            transform.position = Vector3.Slerp(transform.position,target,completion);
            yield return new WaitForEndOfFrame();
        }
        
    }

    public IEnumerator Rotate(float moveTime) {

        Quaternion targetRotation = Quaternion.Euler(new Vector3(transform.eulerAngles.x + 180,transform.eulerAngles.y,transform.eulerAngles.z));

        float counter = 0;

        while(counter < moveTime) {
            counter += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation,counter/moveTime);
            yield return new WaitForEndOfFrame();
        }

    }

}