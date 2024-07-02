using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Courutine : MonoBehaviour
{
    [SerializeField] private float _waitingTime;
    [SerializeField] private Transform _startPosition;
    [SerializeField] private Transform _prefab;
    [SerializeField] private Transform _parent;
    private SpriteRenderer[,] _transforms = new SpriteRenderer[20,20];
    private float _step;
    private Coroutine _courutine;
    private float _timeForChangeColor = 0.5f;

    private void Awake()
    {
        _step = _prefab.localScale.x + 0.1f;
        _waitingTime /= 100;
        StartCoroutine(SpawnCubes());
    }

    public void SetColor()
    {
        //if (_courutine != null)
        //{
        //    StopCoroutine(_courutine);
        //}

        _courutine = StartCoroutine(ChangeColor());
    }

    private IEnumerator SpawnCubes()
    {
        for (int i = 0; i < _transforms.GetLength(0); i++)
        {
            var X = _startPosition.position.x;
            var Y = _startPosition.position.y + (-i * _step);
            for (int j = 0; j < _transforms.GetLength(1); j++)
            {
                var cube = Instantiate(_prefab, _parent);
                cube.position = new Vector3(X + (j * _step),Y,_startPosition.position.z);
                _transforms[i, j] = cube.GetComponent<SpriteRenderer>();
                yield return new WaitForSeconds(_waitingTime);
            }
        }
    }

    private IEnumerator ChangeColor()
    {
        var randomColor = Random.ColorHSV();
        for (int i = 0; i < _transforms.GetLength(0); i++)
        {
            for (int j = 0; j < _transforms.GetLength(1); j++)
            {
                var currentColor = _transforms[i, j].color;
                StartCoroutine(Temp(currentColor, randomColor, _transforms[i,j]));
                yield return new WaitForSeconds(0.2f);
            }
        }
    }

    private IEnumerator Temp(Color currentColor, Color newColor, SpriteRenderer renderer)
    {
        var currentTime = 0f;
        while (currentTime < _timeForChangeColor)
        {
            renderer.color = Color.Lerp(currentColor, newColor, currentTime / _timeForChangeColor);
            currentTime += Time.deltaTime;
            yield return null;
        }
    }
}
