using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayManager : MonoBehaviour {

    public DeckManager deck;

    public int amountToPlay = 4;

    [SerializeField] private TMP_Text dayText;
    private int dayCounter = 0;

    private bool canEnd = false;

    private void Start() {
        StartCoroutine(StartGame());
    }

    private void Update() {
        if(canEnd && (Input.GetKeyDown(KeyCode.E) || deck.playedPool.Count == 0)) {
            canEnd = false;
            StartCoroutine(EndDay());
        }
    }

    private IEnumerator StartGame() {
        deck.Initialize();
        yield return new WaitForSeconds(1f);
        StartCoroutine(StartDay(1));
    }

    private IEnumerator StartDay(int day) {

        dayCounter = day;
        dayText.text = "Day: " + dayCounter.ToString();
        
        for(int i = 0; i < amountToPlay; i++) {
            StartCoroutine(deck.MoveToPlayingField(deck.deckPool[0]));
            yield return new WaitForSeconds(0.5f);
        }

        canEnd = true;
        yield return null;

    }

    private IEnumerator EndDay() {
        deck.ActivatePassives();
        for (int i = 0; i < deck.playedPool.Count; i++) {
            if(deck.playedPool[i] != null) {
                deck.DiscardCard(deck.playedPool[i]);
                yield return new WaitForSeconds(0.5f);
            }
        }
        for (int i = 0; i < deck.effectPool.Count; i++)
        {
             deck.DiscardEffectCard(deck.effectPool[i]);
             yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(3f);

        if(deck.deckPool.Count == 0) {
            yield return StartCoroutine(deck.ResetDeck(deck.discardPool));
        }

        yield return new WaitForSeconds(2f);

        dayCounter ++;
        StartCoroutine(StartDay(dayCounter));

    }

}
