using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuBootstrap : MonoBehaviour
{
    [SerializeField] MainMenuNavigation MainMenuNavigationScript;
    public Data DataScript { get; private set; }
    private void Awake()
    {
        DataScript = this.gameObject.GetComponent<Data>();
        DataScript.Initialize();
        MainMenuNavigationScript = this.gameObject.GetComponent<MainMenuNavigation>();
        MainMenuNavigationScript.Initialize();
        MainMenuNavigationScript.BackButton();

    }
}
