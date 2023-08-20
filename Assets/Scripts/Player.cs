using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private bool _isPlayer;
    public Sprite PlayerCellSprite;
    public float _playerMultiplyCoefficient = 0.2f;
    public Vector3 _offset;
    public Vector2 MoveToPosition;
    public int _bubblesToEmite;
    public GameObject PlayerBubble;
    private Cell _cellScript;

    private void Start()
    {
        _cellScript= GetComponent<Cell>();
    }
    private void FixedUpdate()
    {
       
            ChangeToPlayer();
        
    }
    private void ChangeToPlayer()
    {
        if (this.CompareTag("Player"))
        {
            _isPlayer = true;
            //CellSprite.sprite = PlayerCellSprite;
            _cellScript.MultiplyCoefficient = _playerMultiplyCoefficient;
        }
    }
    IEnumerator EmmiteBubbles(int emitNumber)
    {
        if (_isPlayer)
        {
            for (int i = emitNumber; i > 0; i--)
            {
                Instantiate(PlayerBubble,this.transform.position,this.transform.rotation);
                _cellScript._quantity--;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
    void OnMouseDown()
    {
        _offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D cellCollider = Physics2D.OverlapPoint(_offset);
        if(cellCollider == this.GetComponent<Collider2D>())
        {
            _bubblesToEmite = _cellScript._quantity;
        }

    }
    private void OnMouseUp()
    {
        MoveToPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + _offset;
        Collider2D cellCollider = Physics2D.OverlapPoint(MoveToPosition);
        if (cellCollider != null && cellCollider.gameObject.name != "Bubble")
        {
            MoveToPosition = cellCollider.gameObject.transform.position;
            StartCoroutine(EmmiteBubbles(_bubblesToEmite));
        }
    }
}
