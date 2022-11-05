using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour {

    public Transform deckTransform;
    public Transform discardTransform;
    public Transform passiveTransform;
    public Transform effectTransform;
    public List<Transform> playTransforms = new List<Transform>();
    public Transform cardViewTarget;
    public Transform objectiveViewTarget;

    public Button useButton;
    public Button returnButton;

    public List<Card> deckPool = new List<Card>();
    public List<Card> playedPool = new List<Card>();
    public List<Card> discardPool = new List<Card>();
    public List<Card> passivePool = new List<Card>();
    public List<Card> effectPool = new List<Card>();

    [SerializeField] private PlanetManager planet;

    [SerializeField] private List<CardStats> cardStats = new List<CardStats>();
    [SerializeField] private DeckObject deck;

    [SerializeField] private GameObject cardPrefab;

    private bool isViewingCard = false;

    public void Initialize() {

        for(int i = 0; i < deck.cards.Count; i++) {
            GameObject card = Instantiate(cardPrefab,Vector3.zero,cardPrefab.transform.localRotation);
            card.GetComponent<Card>().Initialize(this, deck.cards[i]);
            deckPool.Add(card.GetComponent<Card>());
        }

        deckPool = ShufflePool(deckPool,deckTransform);

        useButton.gameObject.SetActive(false);
        returnButton.gameObject.SetActive(false);

    }

    private void Update() {
        // if(Input.GetKeyDown(KeyCode.E)) {
        //     if(deckPool.Count != 0) {
        //         StartCoroutine(MoveToPlayingField(deckPool[0]));
        //     }
        //     else {
        //         StartCoroutine(ResetDeck(discardPool));
        //     }
        // }
    }

    public IEnumerator ResetDeck(List<Card> cardPool) {

        for(int i = 0; i < cardPool.Count; i++) {
            Vector3 targetPos = new Vector3(deckTransform.position.x, deckTransform.position.y + 2*cardPool[i].transform.localScale.z*i, deckTransform.position.z);
            cardPool[i].StartCoroutine(cardPool[i].MoveToTarget(targetPos, 2));
            cardPool[i].StartCoroutine(cardPool[i].RotateToTarget(Quaternion.Euler(new Vector3(deckTransform.eulerAngles.x + 180, deckTransform.eulerAngles.y, deckTransform.eulerAngles.z)), 2));
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(2f);
        deckPool.AddRange(cardPool);
        deckPool = ShufflePool(deckPool, deckTransform);
        cardPool.Clear();

    }

    public IEnumerator MoveToPlayingField(Card card) {

        Transform targetTransform = playTransforms[0];
        if(playedPool.Count < playTransforms.Count) {
            targetTransform = playTransforms[playedPool.Count];
        }

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

    public void UseCard(Card card) {
        switch (card.stats.modification)
        {
            case mod.Addition:
                int Aatmos = (int)card.stats.carbonModifier;
                int Atemp = (int)card.stats.tempModifier;
                int Arad = (int)card.stats.radModifier;
                int Alife = (int)card.stats.lifeModifier;
                foreach (Card eCard in effectPool)
                {
                    (int, int, int, int) effected = eCard.stats.cardFunc((Aatmos, Atemp, Arad, Alife, eCard.stats.special));
                    Aatmos = effected.Item1;
                    Atemp = effected.Item2;
                    Arad = effected.Item3;
                    Alife = effected.Item4;
                }
                planet.atmosLevel += Aatmos;
                planet.temperature += Atemp;
                planet.radiation += Arad;
                planet.lifeComplexity += Alife;

                break;
            case mod.ActiveMultiplication:
                (int, int, int, int) results = card.stats.cardFunc(((int)planet.atmosLevel, (int)planet.temperature, (int)planet.radiation, (int)planet.lifeComplexity, card.stats.special));
                planet.atmosLevel = results.Item1;
                planet.temperature = results.Item2;
                planet.radiation = results.Item3;
                planet.lifeComplexity = results.Item4;
                break;
            case mod.passiveAddition:
                break;
            case mod.effect:
                break;
            case mod.Destroypassive:
                break;
            default:
                break;
        }
        planet.atmosLevel = Mathf.Clamp(planet.atmosLevel, 0, 10);
        planet.temperature = Mathf.Clamp(planet.temperature, 0, 10);
        planet.radiation = Mathf.Clamp(planet.radiation, 0, 10);
        planet.lifeComplexity = Mathf.Clamp(planet.lifeComplexity, 0, 7);

        switch (card.stats.modification)
        {
            case mod.Addition:
                DiscardCard(card);
                break;
            case mod.ActiveMultiplication:
                DiscardCard(card);
                break;
            case mod.passiveAddition:
                PlayPassiveCard(card);
                break;
            case mod.effect:
                PlayEffectCard(card);
                break;
            case mod.Destroypassive:
                break;
            default:
                break;
        }
        planet.UpdateValues();
    }

    public void PlayPassiveCard(Card card)
    {

        Vector3 passiveTarget = new Vector3(passiveTransform.position.x - (cardPrefab.transform.localScale.x/4) * passivePool.Count,
                                            passiveTransform.position.y + 2*cardPrefab.transform.localScale.z * passivePool.Count,
                                            passiveTransform.position.z);
        card.StartCoroutine(card.MoveToTarget(passiveTarget, 5));
        card.onPlayingField = false;

        passivePool.Add(card);
        card.StartCoroutine(card.RotateToTarget(passiveTransform.rotation, 5));
        playedPool.Insert(playedPool.IndexOf(card), null);
        playedPool.Remove(card);
    }
    public void PlayEffectCard(Card card)
    {

        Vector3 effectTarget = new Vector3(effectTransform.position.x - (cardPrefab.transform.localScale.x/4) * effectPool.Count,
                                            effectTransform.position.y + 2*cardPrefab.transform.localScale.z * effectPool.Count,
                                            effectTransform.position.z);
        card.StartCoroutine(card.MoveToTarget(effectTarget, 5));
        card.onPlayingField = false;

        effectPool.Add(card);
        card.StartCoroutine(card.RotateToTarget(effectTransform.rotation, 5));
        playedPool.Insert(playedPool.IndexOf(card), null);
        playedPool.Remove(card);
    }
    public void DiscardCard(Card card) {

        Vector3 discardTarget = new Vector3(discardTransform.position.x,
                                            discardTransform.position.y + 2*cardPrefab.transform.localScale.z * discardPool.Count,
                                            discardTransform.position.z);

        card.StartCoroutine(card.MoveToTarget(discardTarget, 5));
        card.StartCoroutine(card.RotateToTarget(discardTransform.rotation, 5));
        card.onPlayingField = false;

        discardPool.Insert(0,card);
        playedPool.Insert(playedPool.IndexOf(card), null);
        playedPool.Remove(card);

    }
    public void DiscardEffectCard(Card card)
    {

        Vector3 discardTarget = new Vector3(discardTransform.position.x,
                                            discardTransform.position.y + cardPrefab.transform.localScale.z * discardPool.Count,
                                            discardTransform.position.z);

        card.StartCoroutine(card.MoveToTarget(discardTarget, 5));
        card.StartCoroutine(card.RotateToTarget(discardTransform.rotation, 5));
        card.onPlayingField = false;

        discardPool.Insert(0, card);
        effectPool.Remove(card);

    }

    public void ViewCard(Card card = null, ObjectiveCard oCard = null) {
        if(!isViewingCard) {
            if(card != null) {
                StartCoroutine(StartViewCard(card));
            }
            else if(oCard != null) {
                StartCoroutine(StartViewObjective(oCard));
            }
        }
    }

    public IEnumerator StartViewCard(Card card) {

        isViewingCard = true;

        Vector3 startPosition = card.transform.position;
        Quaternion startRotation = card.transform.rotation;

        card.StartCoroutine(card.MoveToTarget(cardViewTarget.position, 5));
        yield return card.StartCoroutine(card.RotateToTarget(cardViewTarget.rotation, 5));

        useButton.gameObject.SetActive(true);
        returnButton.gameObject.SetActive(true);

        WaitForUIButtons waitForButton = new WaitForUIButtons(useButton, returnButton);
        yield return waitForButton.Reset();

        if(waitForButton.PressedButton == returnButton) {
            card.StartCoroutine(card.MoveToTarget(startPosition, 5));
            card.StartCoroutine(card.RotateToTarget(startRotation, 5));
            useButton.gameObject.SetActive(false);
            returnButton.gameObject.SetActive(false);
            isViewingCard = false;
        }
        else if(waitForButton.PressedButton == useButton) {
            UseCard(card);
            useButton.gameObject.SetActive(false);
            returnButton.gameObject.SetActive(false);
            isViewingCard = false;
        }
        else {
            yield break;
        }
    }

    public IEnumerator StartViewObjective(ObjectiveCard oCard) {

        isViewingCard = true;

        Vector3 startPosition = oCard.transform.position;
        Quaternion startRotation = oCard.transform.rotation;

        oCard.StartCoroutine(oCard.MoveToTarget(objectiveViewTarget.position, 5));
        yield return oCard.StartCoroutine(oCard.RotateToTarget(objectiveViewTarget.rotation, 5));

        returnButton.gameObject.SetActive(true);

        WaitForUIButtons waitForButton = new WaitForUIButtons(returnButton);
        yield return waitForButton.Reset();

        if(waitForButton.PressedButton == returnButton) {
            oCard.StartCoroutine(oCard.MoveToTarget(startPosition, 5));
            oCard.StartCoroutine(oCard.RotateToTarget(startRotation, 5));
            returnButton.gameObject.SetActive(false);
            isViewingCard = false;
        }
        else {
            yield break;
        }
    }

    private List<Card> ShufflePool(List<Card> cardPool,Transform poolTransform) {
        cardPool = cardPool.OrderBy(a => Random.value).ToList();
        for(int i = 0; i < deckPool.Count; i++) {
            cardPool[i].transform.position = new Vector3(poolTransform.position.x, poolTransform.position.y + 2*cardPrefab.transform.localScale.z*i, poolTransform.position.z);
        }
        cardPool.Reverse();
        return cardPool;
    }

    public void ActivatePassives()
    {
        foreach (Card card in passivePool)
        {
            switch (card.stats.modification)
            {
                case mod.Addition:
                    break;
                case mod.ActiveMultiplication:
                    break;
                case mod.passiveAddition:
                    planet.atmosLevel += card.stats.carbonModifier;
                    planet.temperature += card.stats.tempModifier;
                    planet.radiation += card.stats.radModifier;
                    planet.lifeComplexity += card.stats.lifeModifier;
                    break;
                case mod.effect:
                    break;
                case mod.Destroypassive:
                    break;
                default:
                    break;
            }
            planet.atmosLevel = Mathf.Clamp(planet.atmosLevel, 0, 10);
            planet.temperature = Mathf.Clamp(planet.temperature, 0, 10);
            planet.radiation = Mathf.Clamp(planet.radiation, 0, 10);
            planet.lifeComplexity = Mathf.Clamp(planet.lifeComplexity, 0, 7);

        }

        planet.UpdateValues();
        
    }


}