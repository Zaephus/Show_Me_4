using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour {

    public int index;

    public Animator animator;

    [SerializeField] private TMP_Text indexText;

    [SerializeField] private Transform target;

    public void OnStart() {
        animator = GetComponent<Animator>();
        indexText.text = index.ToString();
    }

    public void OnUpdate() {

    }

    public void MoveToTarget(Transform target) {
        if(animator) {
            Debug.Log("Doing Animation");
            animator.MatchTarget(target.position,target.rotation,new AvatarTarget(),new MatchTargetWeightMask(Vector3.one,1f),0.0f,1.0f);
        }
    }

}