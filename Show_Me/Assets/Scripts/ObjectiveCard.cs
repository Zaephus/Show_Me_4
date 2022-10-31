using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveCard : MonoBehaviour {

    [TextArea(5,10)]
    public string objectives;

    public TMP_Text objectivesText;

    public DeckManager deckManager;

    private void Start() {
        objectivesText.text = objectives;
    }

    public void OnMouseDown() {
        deckManager.ViewCard(null, this);
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
