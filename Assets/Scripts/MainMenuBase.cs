using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuBase : MonoBehaviour
{
    protected MainMenuBootstrap BootstrapScript;
    protected Data _dataScript;
    private string _cameraTag = "MainCamera";
    private string _upgradeMultiSpeedPriceTag = "UpgradeMultiSpeedPrice";
    private string _upgradeMultiSpeedLevelTag = "UpgradeMultiSpeedLevel";
    protected TextMeshProUGUI _upgradeMultiSpeedPriceText;
    protected TextMeshProUGUI _upgradeMultiSpeedLevelText;
    private string _upgradeSpeedPriceTag = "UpgradeSpeedPrice";
    private string _upgradeSpeedLevelTag = "UpgradeSpeedLevel";
    protected TextMeshProUGUI _upgradeSpeedPriceText;
    protected TextMeshProUGUI _upgradeSpeedLevelText;
    private string _moneyTag = "Money";
    protected TextMeshProUGUI _moneyText;
    [SerializeField] Sprite[] _audioImage;
    protected TextMeshProUGUI _musicText;
    protected TextMeshProUGUI _soundsText;
    protected Image _musicImage;
    protected Image _soundsImage;
    protected int _playerColor;
    protected bool[] _openedBubleColors;
    public GameObject[] _bubleColorsLocked;
    public GameObject[] _bubleLockedSprite;
    protected string[] _colorButtonsTagList;

    private string _bubleColorsTag = "Color";
    private string _bubleLockedTag = "Locked";

    public virtual void Initialize()
    {
        BootstrapScript = GameObject.FindGameObjectWithTag(_cameraTag).GetComponent<MainMenuBootstrap>();
        _dataScript = BootstrapScript.gameObject.GetComponent<Data>();
        InitializeUI();
    }

    private void InitializeUI()
    {
        _upgradeMultiSpeedPriceText = GameObject.Find(_upgradeMultiSpeedPriceTag).GetComponent<TextMeshProUGUI>();
        _upgradeMultiSpeedLevelText = GameObject.Find(_upgradeMultiSpeedLevelTag).GetComponent<TextMeshProUGUI>();
        _upgradeSpeedPriceText = GameObject.Find(_upgradeSpeedPriceTag).GetComponent<TextMeshProUGUI>();
        _upgradeSpeedLevelText = GameObject.Find(_upgradeSpeedLevelTag).GetComponent<TextMeshProUGUI>();
        _moneyText = GameObject.FindGameObjectWithTag(_moneyTag).GetComponent<TextMeshProUGUI>();
        string _musicTag = "MusicBtn";
        string _soundsTag = "SoundsBtn";
        
        _musicText = GameObject.Find(_musicTag).GetComponentInChildren<TextMeshProUGUI>();
        _musicImage = GameObject.Find("MusicImage").GetComponent<Image>();
        _soundsText = GameObject.Find(_soundsTag).GetComponentInChildren<TextMeshProUGUI>();
        _soundsImage = GameObject.Find("SoundsImage").GetComponent<Image>();
        
        _bubleColorsLocked = new GameObject[15];
        _bubleLockedSprite = new GameObject[15];
       // _colorButtonsTagList = new string[15];
       // for(int i = 1; i <15; i++)
       // {
       //     _colorButtonsTagList[i-1] = GameObject.Find
       // }


    }

    private void FixedUpdate()
    {
        UpdateUI();
        UpdateBublesColorPanel();
    }
    public virtual void UpgradeSpeed()
    {
        if(_dataScript.CoinsData >= _dataScript.SpeedUpgradePrice)
        {
            _dataScript.CoinsData -= _dataScript.SpeedUpgradePrice;
            _dataScript.SpeedUpgradeLevel++;
            _dataScript.SpeedData ++;
            _dataScript.SpeedUpgradePrice += 10;
            _dataScript.SaveData();
        }
        else
        {
            //showAD
            Debug.Log("PlayingAD");
            _dataScript.SpeedUpgradeLevel++;
            _dataScript.SpeedData += 1;
            _dataScript.SpeedUpgradePrice += 10;
            _dataScript.SaveData();
        }
    }

    public virtual void UpgradeMultiplication()
    {
        if(_dataScript.CoinsData >= _dataScript.MultiplicationUpgradePrice)
        {
            _dataScript.CoinsData -= _dataScript.MultiplicationUpgradePrice;
            _dataScript.MultiplicationUpgradeLevel++;
            _dataScript.MultiplicationData++;
            _dataScript.MultiplicationUpgradePrice += 10;
            _dataScript.SaveData();
        }
        else
        {
            //showAd
            Debug.Log("PlayingAD");
            _dataScript.MultiplicationUpgradeLevel++;
            _dataScript.MultiplicationData++;
            _dataScript.MultiplicationUpgradePrice += 10;
            _dataScript.SaveData();

        }
    }

    private void UpdateUI()
    {
        UpdateUpgradePanel();
        UpdateSettingsPanel();
    }

    private void UpdateUpgradePanel()
    {
        _upgradeMultiSpeedPriceText.SetText("Upgrade " + _dataScript.MultiplicationUpgradePrice);
        _upgradeMultiSpeedLevelText.SetText("Level " + _dataScript.MultiplicationUpgradeLevel);
        _upgradeSpeedPriceText.SetText("Upgrade " + _dataScript.SpeedUpgradePrice);
        _upgradeSpeedLevelText.SetText("Level " + _dataScript.SpeedUpgradeLevel);
        _moneyText.SetText(_dataScript.CoinsData.ToString());
    } 

    private void UpdateSettingsPanel()
    {
        if (_dataScript.IsMusicOn)
        {
            _musicImage.sprite = _audioImage[1];
            _musicText.SetText("Music On");
        }
        else
        {
            _musicImage.sprite = _audioImage[0];
            _musicText.SetText("Music Off");
        }
        if (_dataScript.IsSoundOn)
        {
            _soundsImage.sprite = _audioImage[2];
            _soundsText.SetText("Sounds On");
        }
        else
        {
            _soundsImage.sprite = _audioImage[0];
            _soundsText.SetText("Sounds Off");
        }
    }

    public virtual void UpdateBublesColorPanel()
    {
        TextMeshProUGUI[] _lockedText = new TextMeshProUGUI[15];
        for (int i = 1; i <= 15; i++)
        {
            _bubleColorsLocked[i - 1] = GameObject.Find(_bubleColorsTag + i.ToString());
            _lockedText[i-1] = _bubleColorsLocked[i-1].GetComponentInChildren<TextMeshProUGUI>();
            _bubleLockedSprite[i - 1] = GameObject.Find(_bubleLockedTag + i.ToString());
            if (_dataScript._openedColorsData[i - 1])
            {
                _lockedText[i - 1].SetText("Unlocked");
                _bubleLockedSprite[i - 1].SetActive(false);
            }
            else
            {
                _lockedText[i - 1].SetText("Locked");
                _bubleLockedSprite[i - 1].SetActive(true);
            }
        }
    }
}
