using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Base : MonoBehaviour
{
    public int _quantity;
    public int _minQuantityValue { get; private set; } = 0;
    public int _maxQuantityValue { get; private set; } = 100;
    public int _normalSize { get; private set; } = 50;

    private float _multiplyCoefficient;
    public float _playerMultiplyCoefficient = 0.2f; //for saved settings
    private float _bubblesSpeed;
    private float _playerBubblesSpeed; // for saved settings;

    public bool _isNeutral = true;
    public bool _isPlayer;
    public bool _isEnemy1;
    public bool _isEnemy2;
    private bool _isFullQuantity;
    private bool _isMultiplyingStarted = false;
    public bool _canEmitBubbles = false;

    public string _currentTag;
    public string NeutralTag { get; set; } = "Neutral";
    public string PlayerTag { get; set; } = "Player";
    public string Enemy1Tag { get; set; } = "Enemy1";
    public string Enemy2Tag { get; set; } = "Enemy2";

    public string BubbleName { get; set; } = "Bubble";
    public Sprite[] CellColor;
    public int PlayerCellSprite = 1; //for saved settings

    private Bubble _bubbleScript;
    public Enemy _enemyScript;
    public GameObject PlayerBubblePrefab;

    public Vector3 _offset;
    public Vector2 MoveToPosition;
    public string _CollisionTag;

    private void Awake()
    {
        _enemyScript = gameObject.GetComponent<Enemy>();
    }
    public void Initiate(float _multiplyRate, float _bubblesSpeedRate)
    {
        _multiplyCoefficient = _multiplyRate;
        _bubblesSpeed = _bubblesSpeedRate;
    }
    private void Start()
    {
        if (gameObject.tag == PlayerTag)
        {
            Initiate(_playerMultiplyCoefficient, _playerBubblesSpeed);
            _isPlayer = true;
            _isNeutral = false;
        }
        if(gameObject.tag == Enemy1Tag)
        {
            Initiate(_enemyScript._firstEnemyMultiplyCoefficient, _enemyScript._firstEnemyBubblesSpeed);
            _isEnemy1= true;
            _isNeutral = false;
        }
        if (gameObject.tag == Enemy2Tag)
        {
            Initiate(_enemyScript._secondEnemyMultiplyCoefficient, _enemyScript._secondEnemyBubblesSpeed);
            _isEnemy2 = true;
            _isNeutral = false;
        }
    }
    protected void FixedUpdate()
    {
        _currentTag = gameObject.tag;
        CheckQuantity();
    }
    private void CheckQuantity()
    {
        if (_quantity >= _maxQuantityValue)
        {
            _isMultiplyingStarted = false;
            _isFullQuantity = true;

        }
        else
        {
            if (!_isMultiplyingStarted)
            {
                _isMultiplyingStarted = true;
                _isFullQuantity = false;
                StartCoroutine(QuantityMultiplyCourutine());
            }
        }
    }

    private IEnumerator QuantityMultiplyCourutine()
    {
        while (!_isFullQuantity)
        {
            yield return new WaitForSeconds(1 - _multiplyCoefficient);
            _quantity++;
        }
    }
    public IEnumerator EmitBubbles(int emitNumber)
    {
        Debug.Log(this.gameObject.tag + " Emitting: " + emitNumber);
            for (int i = emitNumber; i > 0; i--)
            {
                Instantiate(PlayerBubblePrefab, this.transform.position, this.transform.rotation);
                _quantity--;
                yield return new WaitForSeconds(0.1f);
            }
    }

    private void ChangeTag(string _incomingTag, Sprite _incomingSprite, int _incomingColorIndex)
    {
        if (_incomingTag == PlayerTag)
        {
            gameObject.tag = PlayerTag;
            _isPlayer = true;
            _isNeutral = false;
            _isEnemy1 = false;
            _isEnemy2 = false;
            _multiplyCoefficient = _playerMultiplyCoefficient;
        }
        if(_incomingTag!= PlayerTag)
        {
            Debug.Log(_incomingTag + " " +  _incomingSprite + " " + _incomingColorIndex);
            _enemyScript.SetEnemyProperties(_incomingTag, _incomingSprite, _incomingColorIndex);
        }
    }

    private void HitByBubble(Collision2D collision, string _tag, Sprite _sprite, int _hitColorIndex)
    {
        if (_bubbleScript._moveToPosition == new Vector2(this.transform.position.x, this.transform.position.y))
        {
            if (_tag == _currentTag)
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
                Debug.Log(_hitColorIndex);
                ChangeTag(_tag, _sprite, _hitColorIndex);
            }
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        _bubbleScript = collision.gameObject.GetComponent<Bubble>();
        _CollisionTag = collision.gameObject.tag;
        int _colorIndex = _bubbleScript.ColorIndex;
        Sprite _bubbleSprite = collision.gameObject.GetComponent<SpriteRenderer>().sprite;
        HitByBubble(collision, _CollisionTag, _bubbleSprite, _colorIndex);
    }
    void OnMouseDown()
    {
        _offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D cellCollider = Physics2D.OverlapPoint(_offset);
        if (gameObject.tag == PlayerTag)
        {
            _canEmitBubbles = true;
        }
    }
    private void OnMouseUp()
    {
        MoveToPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + _offset;
        Collider2D cellCollider = Physics2D.OverlapPoint(MoveToPosition);
        if (cellCollider != null && cellCollider.gameObject.name != BubbleName && _canEmitBubbles)
        {
            _canEmitBubbles = false;
            MoveToPosition = cellCollider.gameObject.transform.position;
            StartCoroutine(EmitBubbles(_quantity));
        }
    }

}




