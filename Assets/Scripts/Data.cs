using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    public int CoinsData;
    public int SpeedData;
    public int SpeedUpgradePrice;
    public int SpeedUpgradeLevel;
    public int MultiplicationData;
    public int MultiplicationUpgradePrice;
    public int MultiplicationUpgradeLevel;
    public bool IsMusicOn;
    public bool IsSoundOn;
    private string [] _saveKeys;
    public int PlayerColorData;
    public bool[] _openedColorsData;
    

    public void Initialize()
    {
        CreateStringArray();
        if (PlayerPrefs.HasKey(_saveKeys[0]))
        {
            LoadData(_saveKeys);
        }
        else SetDefaultValues();
    }
    private void CreateStringArray()
    {
        _saveKeys = new string[7];
        _saveKeys[0] = "CoinsData";
        _saveKeys[1] = "SpeedData";
        _saveKeys[2] = "SpeedUpgradePrice";
        _saveKeys[3] = "SpeedUpgradeLevel";
        _saveKeys[4] = "MultiplicationData";
        _saveKeys[5] = "MultiplicationUpgradePrice";
        _saveKeys[6] = "MultiplicationUpgradeLevel";
        _openedColorsData = new bool[15];
    }

    private void SetDefaultValues()
    {
        CoinsData = 0;
        SpeedData = 1;
        SpeedUpgradePrice = 150;
        SpeedUpgradeLevel = 1;
        MultiplicationData = 1;
        MultiplicationUpgradePrice = 150;
        MultiplicationUpgradeLevel = 1;
        IsMusicOn = true;
        IsSoundOn =true;
        int[] savedOpenedColorsData = new int[15];
        foreach(int i in savedOpenedColorsData)
        {
            _openedColorsData[i] = false;
        }
        _openedColorsData[0] = true;
        PlayerColorData = 1;
    }
    private void LoadData(string[] savedKey)
    {
        int [] loadedValue = new int[7];
        for (int i = 0; i < 7; i++)
        {
            loadedValue[i] = PlayerPrefs.GetInt(savedKey[i], loadedValue[i]);
            Debug.Log("Loaded: " + loadedValue[i]);
        }
        int[] _audioPrefs = new int[2];
        _audioPrefs[0] = PlayerPrefs.GetInt("IsMusicOn", _audioPrefs[0]);
        _audioPrefs[1] = PlayerPrefs.GetInt("IsSoundOn", _audioPrefs[1]);
        CoinsData = loadedValue[0];
        SpeedData = loadedValue[1];
        SpeedUpgradePrice = loadedValue[2];
        SpeedUpgradeLevel = loadedValue[3];
        MultiplicationData = loadedValue[4];
        MultiplicationUpgradePrice = loadedValue[5];
        MultiplicationUpgradeLevel = loadedValue[6];
        if (_audioPrefs[0] == 0)
        {
            IsMusicOn= false;
        }
        else
        {
            IsMusicOn = true;
        }
        if (_audioPrefs[1] == 0)
        {
            IsSoundOn = false;
        }
        else
        {
            IsSoundOn = true;
        }

        int[] savedOpenedColorsData = new int[15];
        foreach(int i in savedOpenedColorsData)
        {
            savedOpenedColorsData[i] = PlayerPrefs.GetInt("OpenedColor" + i);
            if (savedOpenedColorsData[i] == 0)
            {
                _openedColorsData[i] = false;
            }
            else
            {
                _openedColorsData[i] = true;
            }
        }


    }

    public void SaveData()
    {
        int[] _savingarray = new int[7];
        _savingarray[0] = CoinsData;
        _savingarray[1] = SpeedData;
        _savingarray[2] = SpeedUpgradePrice;
        _savingarray[3] = SpeedUpgradeLevel;
        _savingarray[4] = MultiplicationData;
        _savingarray[5] = MultiplicationUpgradePrice;
        _savingarray[6] = MultiplicationUpgradeLevel;
        int[] _audioPrefs = new int[2];
        if (IsMusicOn)
        {
            _audioPrefs[0] = 1;
        }
        else
        {
            _audioPrefs[0] = 0;
        }
        if (IsSoundOn)
        {
            _audioPrefs[1] = 1;
        }
        else
        {
            _audioPrefs[1] = 0;
        }
        int[] _savingOpenedColorsAray = new int[15];
        foreach (int i in _savingarray)
        {
            if (_openedColorsData[i])
            {
                _savingOpenedColorsAray[i] = 1;
            }
            else
            {
                _savingOpenedColorsAray[i] = 0;
            }
        }

        SaveToPref(_saveKeys, _savingarray, _audioPrefs, _savingOpenedColorsAray);
    }
    private void SaveToPref(string[] key, int[] valueToSave, int[] audioPrefs, int[] openedColorsToSave)
    {
        for (int i = 0; i < 7; i++)
        {
            PlayerPrefs.SetInt(key[i], valueToSave[i]);
            Debug.Log("Saved: " + valueToSave[i]);
        }
        PlayerPrefs.SetInt("IsMusicOn", audioPrefs[0]);
        PlayerPrefs.SetInt("IsSoundOn", audioPrefs[1]);
        foreach(int i in openedColorsToSave)
        {
            PlayerPrefs.SetInt("OpenedColor"+i, openedColorsToSave[i]);
        }
        PlayerPrefs.Save();
        
    }

    public void ClearData()
    {
        PlayerPrefs.DeleteAll();
    }
}

