using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayManager : MonoBehaviour {

    public DeckManager deck;

    public int amountToPlay = 4;

    public List<(string,int,int,int,int,int,int,int,int)> joblist = new List<(string, int, int, int, int, int, int, int, int)> ();

    [SerializeField] private Material dayMaterial;
    private int dayCounter = 0;

    private bool canEnd = false;

    private void Start() {
        joblist.Add(("Combat climate change", 7, 2, 4, 3, 5, 0, 5, 3));
        joblist.Add(("Energy Planet", 3, 0, 5, 7, 5, 10, 7, 0));
        joblist.Add(("Factory Planet", 2, 5, 2, 0, 10, 0, 10, 3));
        joblist.Add(("Planet Park", 10, 8, 0, 3, 5, 0, 7, 5));
        joblist.Add(("A Lost Planet", 10, 10, 10, 0, 0, 0, 0, 7));
        StartCoroutine(StartGame());
    }
    

    private void Update() {
        if(canEnd && deck.playedPool.Count == 0) {
            canEnd = false;
            StartCoroutine(EndDay());
        }
    }


    private IEnumerator StartGame() {
        deck.Initialize();
        yield return new WaitForSeconds(1f);
        StartCoroutine(StartDay(1));
    }

    public void OnEndDayClick() {
        if (canEnd) {
            canEnd = false;
            StartCoroutine(EndDay());
        }
    }
    private IEnumerator StartDay(int day) {

        dayCounter = day;
        dayMaterial.SetFloat("_DaySelection", day);
        
        for(int i = 0; i < amountToPlay; i++) {
            StartCoroutine(deck.MoveToPlayingField(deck.deckPool[0]));
            if(deck.deckPool.Count == 0) {
                break;
            }
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

        if(dayCounter >= 7) {
            FindObjectOfType<GameManager>().EndGame();
            yield break;
        }
        else {
            if(deck.deckPool.Count == 0) {
                yield return StartCoroutine(deck.ResetDeck(deck.discardPool));
            }

            yield return new WaitForSeconds(2f);

            dayCounter ++;
            StartCoroutine(StartDay(dayCounter));
        }

    }

}
