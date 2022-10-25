using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour {

    public Transform deckTransform;
    public Transform discardTransform;
    public List<Transform> playTransforms = new List<Transform>();
    public Transform viewTarget;

    public Canvas buttonCanvas;
    public Button useButton;
    public Button returnButton;

    [SerializeField] private int cardAmount = 10;

    [SerializeField] private GameObject cardPrefab;

    private List<Card> deckPool = new List<Card>();
    [SerializeField] private List<Card> playedPool = new List<Card>();
    [SerializeField] private List<Card> discardPool = new List<Card>();

    private bool isViewingCard = false;

    private void Start() {

        for(int i = 0; i < cardAmount; i++) {
            GameObject card = Instantiate(cardPrefab,Vector3.zero,cardPrefab.transform.localRotation);
            card.GetComponent<Card>().index = i+1;
            card.GetComponent<Card>().OnStart(this);
            deckPool.Add(card.GetComponent<Card>());
        }

        deckPool = ShufflePool(deckPool,deckTransform);

        buttonCanvas.enabled = false;

    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.E) && deckPool.Count != 0) {
            StartCoroutine(MoveToPlayingField(deckPool[0]));
        }
    }

    public IEnumerator MoveToPlayingField(Card card) {

        Transform targetTransform = playTransforms[playedPool.Count];

        for(int i = 0; i < playedPool.Count; i++) {
            if(playedPool[i] == null) {
                targetTransform = playTransforms[i];
                playedPool.RemoveAt(i);
                break;
            }
        }

        deckPool.Remove(card);
        playedPool.Insert(playTransforms.IndexOf(targetTransform), card);

        card.StartCoroutine(card.MoveToTarget(targetTransform.position, 5));
        yield return card.StartCoroutine(card.RotateToTarget(Quaternion.Euler(new Vector3(card.transform.eulerAngles.x + 180, card.transform.eulerAngles.y, card.transform.eulerAngles.z)), 5));
        
        card.onPlayingField = true;

    }

    public void DiscardCard(Card card) {

        Vector3 discardTarget = new Vector3(discardTransform.position.x,
                                            discardTransform.position.y + cardPrefab.transform.localScale.y * discardPool.Count,
                                            discardTransform.position.z);

        card.StartCoroutine(card.MoveToTarget(discardTarget, 5));
        card.onPlayingField = false;
        
        discardPool.Insert(0,card);
        playedPool.Insert(playedPool.IndexOf(card), null);
        playedPool.Remove(card);

    }

    public void ViewCard(Card card) {
        if(!isViewingCard) {
            StartCoroutine(StartViewCard(card));
        }
    }

    public IEnumerator StartViewCard(Card card) {

        Vector3 startPosition = card.transform.position;
        Quaternion startRotation = card.transform.rotation;

        card.StartCoroutine(card.MoveToTarget(viewTarget.position, 5));
        yield return card.StartCoroutine(card.RotateToTarget(viewTarget.rotation, 5));

        buttonCanvas.enabled = true;
        isViewingCard = true;

        WaitForUIButtons waitForButton = new WaitForUIButtons(useButton, returnButton);
        yield return waitForButton.Reset();

        if(waitForButton.PressedButton == returnButton) {
            card.StartCoroutine(card.MoveToTarget(startPosition, 5));
            card.StartCoroutine(card.RotateToTarget(startRotation, 5));
            buttonCanvas.enabled = false;
            isViewingCard = false;
        }
        else if(waitForButton.PressedButton == useButton) {
            DiscardCard(card);
            buttonCanvas.enabled = false;
            isViewingCard = false;
        }
        else {
            yield break;
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