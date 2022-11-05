using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour {

    public string title;
    public string description;

    [HideInInspector] public bool onPlayingField = false;

    [HideInInspector] public CardStats stats;

    [SerializeField] private Material thumbnailMat;

    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;

    private DeckManager deckManager;

    public void Initialize(DeckManager dm, CardStats cs) {
        deckManager = dm;
        stats = cs;
        titleText.text = title;
        descriptionText.text = description;
        thumbnailMat.SetTexture("_CardImageTexture",stats.cardTexture);
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