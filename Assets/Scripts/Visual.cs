using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Visual : MonoBehaviour
{
    public TextMeshProUGUI _quantityIndicator;
    private Transform _cellTransform;
    public SpriteRenderer CellSprite { get; private set; }
    private Vector3 _transformVector;
    private Base _baseScript;
    private Enemy _enemyScript;



    private void Start()
    {
        _baseScript = this.GetComponent<Base>();
        _enemyScript = _baseScript._enemyScript;
        _quantityIndicator = GetComponentInChildren<TextMeshProUGUI>();
        _cellTransform = GetComponent<Transform>();
        CellSprite = this.gameObject.GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        ShowQuantity(_baseScript._quantity);
        ShowScale(_baseScript._quantity);
        SetCellColor(_baseScript._currentTag);
    }
    public void SetCellColor(string _tag)
    {
        if (_tag == _baseScript.NeutralTag)
        {
            CellSprite.sprite = _baseScript.CellColor[0];
        }
        if (_tag == _baseScript.PlayerTag)
        {
            CellSprite.sprite = _baseScript.CellColor[_baseScript.PlayerCellSprite];
        }
        if (_tag == _baseScript.Enemy1Tag)/// when transform to enemy color changes here
        {
            CellSprite.sprite = _baseScript.CellColor[_enemyScript._enemyCellSprite[0]];
        }
        if(_tag == _baseScript.Enemy2Tag)
        {
            CellSprite.sprite = _baseScript.CellColor[_enemyScript._enemyCellSprite[1]];
        }
    }
    private int ShowQuantity(int _locQuantity)
    {
        if (_locQuantity <= _baseScript._maxQuantityValue && _locQuantity > 0)
        {
            _quantityIndicator.SetText(_locQuantity.ToString());
        }
        if (_locQuantity <= _baseScript._minQuantityValue)
        {
            _quantityIndicator.SetText(_baseScript._minQuantityValue.ToString());
            _baseScript._quantity = _baseScript._minQuantityValue;
        }
        if (_locQuantity >= _baseScript._maxQuantityValue)
        {
            _quantityIndicator.SetText(_locQuantity.ToString());
        }
        return _baseScript._quantity;
    }
    private int ShowScale(int _locQuantity)
    {
        _transformVector = new Vector3(0.2f + (_locQuantity - _baseScript._normalSize) * 0.003f, 0.2f + (_locQuantity - _baseScript._normalSize) * 0.003f, 0.2f);
        if (_locQuantity > _baseScript._normalSize && _locQuantity <=99)
        {
            _cellTransform.localScale = _transformVector;
        }
        return _locQuantity;
    }
}