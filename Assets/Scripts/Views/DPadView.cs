using System;
using UnityEngine;
using UnityEngine.UI;

public class DPadView : MonoBehaviour
{
    public static event Action<Vector2Int> OnDirectionChanged;

    [SerializeField] private Button _up;
    [SerializeField] private Button _down;
    [SerializeField] private Button _left;
    [SerializeField] private Button _right;

#if !(UNITY_IOS || UNITY_ANDROID)
    private void Awake()
    {
        gameObject.SetActive(false);
    }
#endif

    private void OnEnable()
    {
        _up.onClick.AddListener(PressUp);
        _down.onClick.AddListener(PressDown);
        _left.onClick.AddListener(PressLeft);
        _right.onClick.AddListener(PressRight);
    }
    private void OnDisable()
    {
        _up.onClick.RemoveListener(PressUp);
        _down.onClick.RemoveListener(PressDown);
        _left.onClick.RemoveListener(PressLeft);
        _right.onClick.RemoveListener(PressRight);
    }

    public void PressUp() => OnDirectionChanged?.Invoke(Vector2Int.up);
    public void PressDown() => OnDirectionChanged?.Invoke(Vector2Int.down);
    public void PressLeft() => OnDirectionChanged?.Invoke(Vector2Int.left);
    public void PressRight() => OnDirectionChanged?.Invoke(Vector2Int.right);
}
