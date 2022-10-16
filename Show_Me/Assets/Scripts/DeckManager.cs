using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckManager : MonoBehaviour {

    public Transform deckTransform;
    public Transform discardTransform;
    public List<Transform> playTransforms = new List<Transform>();

    [SerializeField] private int cardAmount = 10;

    [SerializeField] private GameObject cardPrefab;

    private List<Card> deckPool = new List<Card>();
    private List<Card> playedPool = new List<Card>();
    private List<Card> discardPool = new List<Card>();

    private void Start() {

        List<Vector3> deckOrder = SetDeckOrder(cardAmount);

        for(int i = 0; i < cardAmount; i++) {
            GameObject card = Instantiate(cardPrefab,deckOrder[i],cardPrefab.transform.localRotation);
            card.GetComponent<Card>().index = i+1;
            card.GetComponent<Card>().OnStart();
            deckPool.Add(card.GetComponent<Card>());
        }

        deckPool[0].transform.position = new Vector3(deckPool[0].transform.position.x + 1,deckPool[0].transform.position.y,deckPool[0].transform.position.z);

    }

    private void Update() {

    }

    private List<Vector3> SetDeckOrder(int amount) {

        List<Vector3> positions = new List<Vector3>();

        for(int i = 0; i < amount; i++) {
            positions.Add(new Vector3(deckTransform.position.x, deckTransform.position.y + cardPrefab.transform.localScale.y*i, deckTransform.position.z));
        }

        //return(positions);
        return(positions.OrderBy(a => Random.value).ToList());

    }

}