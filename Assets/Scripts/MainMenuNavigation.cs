using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class MainMenuNavigation : MainMenuBase
{
    [SerializeField] GameObject _upgradesPanel;
    [SerializeField] GameObject _bubbleColorPanel;
    [SerializeField] GameObject _settingsPanel;

    public void ShowUpgradePanel()
    {
        _upgradesPanel.SetActive(true);
        _bubbleColorPanel.SetActive(false);
        _settingsPanel.SetActive(false);
    }
    
    public void ShowSettingsPanel()
    {
        _settingsPanel.SetActive(true);
        _upgradesPanel.SetActive(false);
        _bubbleColorPanel.SetActive(false);
    }

    public void ShowBubbleColorPanel()
    {
        _bubbleColorPanel.SetActive(true);
        _settingsPanel.SetActive(false);
        _upgradesPanel.SetActive(false);
    }

    public void BackButton()
    {
        _upgradesPanel.SetActive(false);
        _bubbleColorPanel.SetActive(false);
        _settingsPanel.SetActive(false);
    }

    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void OnMusicButton()
    {
        if (_dataScript.IsMusicOn)
        {
            _dataScript.IsMusicOn = false;
        }
        else
        {
            _dataScript.IsMusicOn = true;
        }
    }

    public void OnSoundsButton()
    {
        if (_dataScript.IsSoundOn)
        {
            _dataScript.IsSoundOn = false;
        }
        else
        {
            _dataScript.IsSoundOn = true;
        }
    }

    public void OnColorButtonClick()
    {
        //???????????????????????????
        GameObject _onClickObject = this.gameObject;
        for(int i = 1; i<15; i++)
        {
            if(_onClickObject == _bubleColorsLocked[i-1])
            {
                Debug.Log("Changed " + _bubleColorsLocked[i-1].name);
            }
        }
        
    }

    public override void UpdateBublesColorPanel()
    {
        if (_bubbleColorPanel.activeInHierarchy ==true )
        {
            base.UpdateBublesColorPanel();
        }
    }
}
