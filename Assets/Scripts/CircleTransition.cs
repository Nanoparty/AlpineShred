using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CircleTransition: MonoBehaviour
{
    private Canvas _canvas;
    private Image _blackScreen;

    private bool _startGame = false;
    private bool _startMenu = false;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _blackScreen = GetComponentInChildren<Image>();
    }

    private void Start()
    {
        DrawBlackScreen();
        OpenBlackScreen();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            OpenBlackScreen();
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            CloseBlackScreen();
        }
    }

    public void OpenBlackScreen()
    {
        StartCoroutine(Transition(2, 0, 1));
    }
    public void CloseBlackScreen()
    {
        StartCoroutine(Transition(2, 1, 0));
    }

    public void StartGameTransition()
    {
        _startGame = true;
        CloseBlackScreen();
    }
    public void StartMenuTransition()
    {
        _startMenu = true;
        CloseBlackScreen();
    }

    private void DrawBlackScreen()
    {
        var canvasRect = _canvas.GetComponent<RectTransform>().rect;
        var canvasWidth = canvasRect.width;
        var canvasHeight = canvasRect.height;

        var squareValue = 0f;
        if (canvasWidth > canvasHeight) squareValue = canvasWidth;
        else squareValue = canvasHeight;

        _blackScreen.rectTransform.sizeDelta = new Vector2(squareValue, squareValue);
    }

    private IEnumerator Transition(float duration, float beginRadius, float endRadius)
    {
        var time = 0f;
        var radius = float.MaxValue;
        while (time <= duration)
        {
            time += Time.deltaTime;
            var t = time / duration;
            radius = Mathf.Lerp(beginRadius, endRadius, t);

            _blackScreen.material.SetFloat("_Radius", radius);
            yield return null;
        }

        if (_startGame && radius < 0.01f) SceneManager.LoadScene("Mountain", LoadSceneMode.Single);
        if (_startMenu && radius < 0.01f) SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}
