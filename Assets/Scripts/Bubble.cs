using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour

  {
    private Rigidbody2D _bubbleRb;
    private float moveSpeed = 0.5f;
    public Vector2 _moveToPosition;//{ get; private set; }
    private Base _parentCellScript;
    private Enemy _enemyScript;
    private bool _isDirectionReceived = false;
    public SpriteRenderer _bubbleSprite { get; private set; }
    public int ColorIndex;

    void Start()
    {
        _bubbleRb = this.GetComponent<Rigidbody2D>();
        _bubbleSprite = this.GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        StartCoroutine(MoveToTarget());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        BubbleCollision(collision);    
    }

    private void BubbleCollision(Collision2D _collision)
    {
        if (_collision.gameObject.GetComponent<Bubble>() != null)
        {
            Bubble _collisionBubble = _collision.gameObject.GetComponent<Bubble>();
            if (_collisionBubble._bubbleSprite.sprite != _bubbleSprite.sprite)
            {
                Destroy(gameObject);
            }
        }
        if (_collision.gameObject.GetComponent<Base>() != null && !_isDirectionReceived)
        {
            _parentCellScript = _collision.gameObject.GetComponent<Base>();
            _enemyScript = _parentCellScript._enemyScript;
            SetBubbleProperties();
        }
    }

    private void SetBubbleProperties()
    {
        
        gameObject.tag = _parentCellScript.gameObject.tag;
        if (_parentCellScript.gameObject.tag == _parentCellScript.PlayerTag)
        {
            ColorIndex = _parentCellScript.PlayerCellSprite;
            _bubbleSprite.sprite = _parentCellScript.CellColor[_parentCellScript.PlayerCellSprite];
            _moveToPosition = _parentCellScript.MoveToPosition;
            _isDirectionReceived = true;
        }
        if (_parentCellScript.gameObject.tag == _parentCellScript.Enemy1Tag)
        {
            SetEnemyProperties();
        }
        if (_parentCellScript.gameObject.tag == _parentCellScript.Enemy2Tag)
        {
            SetEnemyProperties();
        }
    }
    private void SetEnemyProperties()
    {
        Vector2 _targetPosition = _parentCellScript.GetComponent<Enemy>()._minQuantityObject.transform.position;
        Vector2 _nullVector = new Vector2(0, 0);
        ColorIndex = _enemyScript._colorToEmit;
        _bubbleSprite.sprite = _parentCellScript.CellColor[ColorIndex];
        if (_enemyScript._minQuantityObject != null)
        {
            if(_targetPosition != _nullVector)
            {
                _moveToPosition = _targetPosition;
                _isDirectionReceived = true;
            }
            else
            {
                StopCoroutine(_parentCellScript.EmitBubbles(0));
                Destroy(this.gameObject);
            }   
        }
        else
        {
            StopCoroutine(_parentCellScript.EmitBubbles(0));
        }
    }

    IEnumerator MoveToTarget()
    {
        while (Vector2.Distance(_bubbleRb.position, _moveToPosition) > 0.01f)
        {
            Vector2 direction = (_moveToPosition - _bubbleRb.position).normalized;
            _bubbleRb.MovePosition(_bubbleRb.position + direction * moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
    
}
