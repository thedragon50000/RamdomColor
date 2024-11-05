using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro;
using UniRx;
using DG.Tweening;

public class RandomColor : MonoBehaviour
{
    int R, G, B = 128;
    double _brightness;
    private int _targetBrightness = 128;

    private readonly int _tolerance = 1;
    public TMP_InputField inputField;
    public Button btn;
    ReactiveProperty<String> _inputContents = new();
    public TMP_Text text;
    public Image img;

    private void Start()
    {
        btn.onClick.AddListener(Call);
        inputField.onValidateInput = (input, charIndex, addedChar) => CheckValidateAsInt(addedChar);
        inputField.onValueChanged.AddListener(_ => { _inputContents.Value = _; });
        _inputContents.Subscribe(_ => { int.TryParse(_, out _targetBrightness); });
    }

    private async void Call()
    {
        await GetRGB();
        Color c = new Color(R / 255f, G / 255f, B / 255f);
        img.DOColor(c, 2);
        text.text = $"Gray = {_targetBrightness}\nR = {R}, G = {G}, B = {B}";
    }

    private char CheckValidateAsInt(char charToValidate)
    {
        int result;
        bool tryParse = int.TryParse(charToValidate.ToString(), out result);

        //不是數字
        if (!tryParse)
        {
            // change it to an empty character.
            charToValidate = '\0';
        }

        return charToValidate;
    }

    Task GetRGB()
    {
        if (_targetBrightness is > 255 or < 0)
        {
            _targetBrightness = 128;
        }

        do
        {
            // 隨機生成 R、G、B 值
            R = Random.Range(0, 256);
            G = Random.Range(0, 256);
            B = Random.Range(0, 256);

            // 計算亮度
            _brightness = 0.299 * R + 0.587 * G + 0.114 * B;
        } while (_brightness < _targetBrightness - _tolerance || _brightness > _targetBrightness + _tolerance);

        return Task.CompletedTask;
    }
}