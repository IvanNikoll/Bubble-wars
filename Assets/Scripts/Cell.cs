using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int _quantity;
    private TextMeshProUGUI _quantityIndicator;
    private int _minQuantityValue = 0;
    private int _maxQuantityValue = 100;
    private int _normalSize = 50;
    private Transform _cellTransform;
    private Vector3 _transformVector;
    public string _currentTag;
    public float MultiplyCoefficient { get; set; }
    public bool _isFullQuantity;
    bool _isMultiplyingStarted = false;
    public Sprite[] CellColor;
    protected SpriteRenderer CellSprite;
    private Bubble _bubbleScript;
    public Player _playerScript;

    private bool _isPlayer;
    public Sprite PlayerCellSprite;
    public float _playerMultiplyCoefficient = 0.2f;
    public Vector3 _offset;
    public Vector2 MoveToPosition;
    public bool _canEmmiteBubbles = false;
    public GameObject PlayerBubblePrefab;
    void Start()//
    {
        _quantityIndicator = GetComponentInChildren<TextMeshProUGUI>(); //
        _cellTransform = GetComponent<Transform>(); //
        CellSprite = this.GetComponent<SpriteRenderer>(); //
        _playerScript= this.GetComponent<Player>(); // not required
    }
    
    private void FixedUpdate()
    {
        ShowQuantity(_quantity);//
        ShowScale(_quantity);//
        UpdateData();//
        ChangeToPlayer();//

    }
    private void UpdateData()//
    {
        _currentTag = gameObject.tag;
        SetCellColor();
        if (_quantity >= _maxQuantityValue)
        {
            _isMultiplyingStarted = false;
            _isFullQuantity = true;
        }
        else
        {
            if(!_isMultiplyingStarted)
            {
                _isMultiplyingStarted = true;
                _isFullQuantity = false;
                StartCoroutine(QuantityMultiplyCourutine());
            }
            
        }
    }

    private int ShowQuantity(int _locQuantity)//
    {
        if (_locQuantity <= _maxQuantityValue && _locQuantity >0)
        {
            _quantityIndicator.SetText(_locQuantity.ToString());
        }
        if(_locQuantity <= _minQuantityValue)
        {
            _quantityIndicator.SetText(_minQuantityValue.ToString());
            _quantity = _minQuantityValue;
        }
        if(_locQuantity >= _maxQuantityValue)
        {
            _quantityIndicator.SetText(_maxQuantityValue.ToString());
            _quantity = _maxQuantityValue;
        }
        return _quantity;
    } 
    private int ShowScale(int _locQuantity)
    {
        _transformVector = new Vector3(0.2f + (_locQuantity - _normalSize)*0.003f, 0.2f + (_locQuantity - _normalSize)*0.003f, 0.2f);
        if (_locQuantity > _normalSize)
        {
            _cellTransform.localScale = _transformVector;
         
        }
        return _locQuantity;
    }//
    private void SetCellColor() //
    {
        if (_currentTag == "Neutral")
        {
            CellSprite.sprite = CellColor[0];
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        _bubbleScript = collision.gameObject.GetComponent<Bubble>();
        if (_bubbleScript._moveToPosition == new Vector2(this.transform.position.x, this.transform.position.y))
        {
            if (collision.gameObject.tag == _currentTag)
            {
                _quantity++;
                Destroy(collision.gameObject);
            }
            else
            {
                _quantity--;
                Destroy(collision.gameObject);
            }
            if (_quantity < 1)
            {
                if (collision.gameObject.tag == "Player")
                {
                    gameObject.tag = "Player";
                   // _playerScript.enabled = true;
                }

            }
        }
    }//
    private IEnumerator QuantityMultiplyCourutine()
    {
        while (!_isFullQuantity)
        {
            yield return new WaitForSeconds(1 - MultiplyCoefficient);
            _quantity++;
        }
    }//
    private void ChangeToPlayer()//
    {
        if (this.CompareTag("Player"))
        {
            _isPlayer = true;
            //CellSprite.sprite = PlayerCellSprite;
            MultiplyCoefficient = _playerMultiplyCoefficient;
        }
    }
    IEnumerator EmmiteBubbles(int emiteNumber)//
    {
        if (_isPlayer)
        {
            for (int i = emiteNumber; i > 0; i--)
            {
                Instantiate(PlayerBubblePrefab, this.transform.position, this.transform.rotation);
                _quantity--;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
    void OnMouseDown()//
    {
        _offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D cellCollider = Physics2D.OverlapPoint(_offset);
        if (cellCollider == this.GetComponent<Collider2D>())
        {
            _canEmmiteBubbles = true;
        }

    }
    private void OnMouseUp()//
    {
        MoveToPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + _offset;
        Collider2D cellCollider = Physics2D.OverlapPoint(MoveToPosition);
        if (cellCollider != null && cellCollider.gameObject.name != "Bubble" && _canEmmiteBubbles)
        {
            _canEmmiteBubbles = false;
            MoveToPosition = cellCollider.gameObject.transform.position;
            StartCoroutine(EmmiteBubbles(_quantity));
        }
    }
}