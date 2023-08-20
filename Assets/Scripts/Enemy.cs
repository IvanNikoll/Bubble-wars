using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Base _baseScript;
    protected Visual _visualScript;
    public float _firstEnemyMultiplyCoefficient = 0.2f; //for saved settings
    public float _secondEnemyMultiplyCoefficient = 0.2f; //for saved settings
    public float _firstEnemyBubblesSpeed; // for saved settings;
    public float _secondEnemyBubblesSpeed; // for saved settings;
    public float _locationUpdateInterval;
    public GameObject _minQuantityObject;
    public int[] _enemyCellSprite { get; private set; }
    public int _colorToEmit;
    public bool _isLocatingStarted;
    

    protected void Start()
    {
        _baseScript = GetComponent<Base>();
         _visualScript = GetComponent<Visual>();
        _enemyCellSprite = new int [2];  
        SetEnemycolor(_enemyCellSprite);
        if (_baseScript._isEnemy1 && !_isLocatingStarted)
        {
            _isLocatingStarted = true;
            StartCoroutine(LocateTargetCourutine());
        }
        if (_baseScript._isEnemy2 && !_isLocatingStarted)
        {
            _isLocatingStarted = true;
            StartCoroutine(LocateTargetCourutine());
        }
    }

    protected void FixedUpdate()
    {
        _isLocatingStarted = true;
    }
    
    public void SetEnemyProperties(string _enemy,Sprite _enemySprite, int _enemyIndex)
    {
        gameObject.tag = _enemy;
        _visualScript.CellSprite.sprite= _enemySprite;
        if(gameObject.tag == _baseScript.Enemy1Tag)
        {
            _enemyCellSprite[0] = _enemyIndex;
            _baseScript.Initiate(_firstEnemyBubblesSpeed, _firstEnemyBubblesSpeed);
            _baseScript._isEnemy1 = true;
            _baseScript._isEnemy2 = false;
        }
        if(gameObject.tag == _baseScript.Enemy2Tag)
        {
            _enemyCellSprite[1] = _enemyIndex;
            _baseScript.Initiate(_secondEnemyMultiplyCoefficient, _secondEnemyBubblesSpeed);
            _baseScript._isEnemy2 = true;
            _baseScript._isEnemy1 = false;
        }
        _baseScript._isPlayer = false;
        _baseScript._isNeutral = false;
        _isLocatingStarted = false;
        StartCoroutine(LocateTargetCourutine());
            }
    protected void SetEnemycolor(int[] _enemySprite)
    {
        for(int i=0; i<=_enemySprite.Length-1; i++)
        {
            _enemySprite[i] = Random.Range(0, _baseScript.CellColor.Length);
            _enemyCellSprite[i] = _enemySprite[i];           
        }
    }

    protected void EnemyBubblesEmissoion()
    {
        int _numberToEmit = _baseScript._quantity;
        if(gameObject.tag == _baseScript.Enemy1Tag)
        {
            _colorToEmit = _enemyCellSprite[0];
        }
        if (gameObject.tag == _baseScript.Enemy2Tag)
        {
            _colorToEmit = _enemyCellSprite[1];
        }
        if (_minQuantityObject != null && _minQuantityObject.GetComponent<Base>()._quantity < _baseScript._quantity - Random.Range(5, 10))
        {
            List<GameObject> _enemy1Objects;
            List<GameObject> _enemy2Objects;
            _enemy1Objects = new List<GameObject>();
            _enemy1Objects.AddRange(GameObject.FindGameObjectsWithTag(_baseScript.Enemy1Tag));
            if (_enemy1Objects.Count != 0)
            {
                int sum = 0;
                foreach (GameObject enemyObject in _enemy1Objects)
                    if (enemyObject.GetComponent<Enemy>() != null)
                    {
                        {
                            int value = enemyObject.GetComponent<Base>()._quantity;
                            sum += value;
                        }
                        if (_minQuantityObject != null)
                        {
                            if (sum > (_minQuantityObject.GetComponent<Base>()._quantity))
                            {
                                StartCoroutine(_baseScript.EmitBubbles(_numberToEmit));////////////////////////////////////////
                            }
                        }
                    }
            }
            _enemy2Objects = new List<GameObject>();
            _enemy2Objects.AddRange(GameObject.FindGameObjectsWithTag(_baseScript.Enemy2Tag));
            if (_enemy2Objects.Count != 0)
            {
                int sum = 0;
                foreach (GameObject enemyObject in _enemy2Objects)
                    if (enemyObject.GetComponent<Enemy>() != null)
                    {
                        {
                            int value = enemyObject.GetComponent<Base>()._quantity;
                            sum += value;
                        }
                        if (_minQuantityObject != null)
                        {
                            if (sum > (_minQuantityObject.GetComponent<Base>()._quantity))
                            {
                                StartCoroutine(_baseScript.EmitBubbles(_numberToEmit));
                            }
                        }
                    }
            }
        }
        
    }

    protected IEnumerator LocateTargetCourutine()
    {
        //while (_baseScript._isEnemy1 || _baseScript._isEnemy2)
        //{
        //  GenerateRandomValue();
        // EnemyBubblesEmissoion();
        // LocateEnemyTarget();
        // yield return new WaitForSeconds(_locationUpdateInterval);

        //}
        //_isLocatingStarted = false;
        while (_baseScript._isEnemy1)
        {
            yield return new WaitForSeconds(_locationUpdateInterval);
            GenerateRandomValue();
            EnemyBubblesEmissoion();
            LocateEnemyTarget();
        }
        while (_baseScript._isEnemy2)
        {
            yield return new WaitForSeconds(_locationUpdateInterval);
            GenerateRandomValue();
            EnemyBubblesEmissoion();
            LocateEnemyTarget();
        }
        _isLocatingStarted = false;
    }
    
    protected void GenerateRandomValue() 
    {
        _locationUpdateInterval = Random.Range(1, 7);
    }
    protected void LocateEnemyTarget()
    {
        if(this.gameObject.tag == _baseScript.Enemy1Tag)
        {
            List<GameObject> _enemyOneTargetObjects = new List<GameObject>();
            _enemyOneTargetObjects.AddRange(GameObject.FindGameObjectsWithTag(_baseScript.PlayerTag));
            _enemyOneTargetObjects.AddRange(GameObject.FindGameObjectsWithTag(_baseScript.NeutralTag));
            _enemyOneTargetObjects.AddRange(GameObject.FindGameObjectsWithTag(_baseScript.Enemy2Tag));
            if (_enemyOneTargetObjects.Count > 0)
            {
                _minQuantityObject = _enemyOneTargetObjects.OrderBy(obj => obj.GetComponent<Base>() != null ? obj.GetComponent<Base>()._quantity : int.MaxValue).FirstOrDefault();
                Debug.Log("1 "+_minQuantityObject);
            }
        }
        if (this.gameObject.tag == _baseScript.Enemy2Tag)
        {
            List<GameObject> _enemyTwoTargetObjects = new List<GameObject>();
            _enemyTwoTargetObjects.AddRange(GameObject.FindGameObjectsWithTag(_baseScript.PlayerTag));
            _enemyTwoTargetObjects.AddRange(GameObject.FindGameObjectsWithTag(_baseScript.NeutralTag));
            _enemyTwoTargetObjects.AddRange(GameObject.FindGameObjectsWithTag(_baseScript.Enemy1Tag));
            if (_enemyTwoTargetObjects.Count > 0)
            {
                _minQuantityObject = _enemyTwoTargetObjects.OrderBy(obj => obj.GetComponent<Base>() != null ? obj.GetComponent<Base>()._quantity : int.MaxValue).FirstOrDefault();
                Debug.Log("2 "+_minQuantityObject);
            }
        }
    }
}
