using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject game;

    public void StartGame() {
        mainMenu.SetActive(false);
    }
}
