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

        for(int i = 0; i < cardAmount; i++) {
            GameObject card = Instantiate(cardPrefab,Vector3.zero,cardPrefab.transform.localRotation);
            card.GetComponent<Card>().index = i+1;
            card.GetComponent<Card>().OnStart();
            deckPool.Add(card.GetComponent<Card>());
        }

        deckPool = ShufflePool(deckPool,deckTransform);

    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.E)) {
            deckPool[0].StartCoroutine(deckPool[0].MoveToTarget(playTransforms[playedPool.Count], 5));
            deckPool[0].StartCoroutine(deckPool[0].Rotate(5));
            playedPool.Add(deckPool[0]);
            deckPool.RemoveAt(0);
        }
    }

    private List<Card> ShufflePool(List<Card> cardPool,Transform poolTransform) {
        cardPool = cardPool.OrderBy(a => Random.value).ToList();
        for(int i = 0; i < deckPool.Count; i++) {
            cardPool[i].transform.position = new Vector3(poolTransform.position.x, poolTransform.position.y + cardPrefab.transform.localScale.y*i, poolTransform.position.z);
        }
        cardPool.Reverse();
        return cardPool;
    }

}