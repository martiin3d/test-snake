using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSFX : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button _button;
    private bool _hasSoundPlayed = false;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnButtonClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.ButtonClick);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_hasSoundPlayed)
        {
            SoundManager.Instance.PlaySound(SoundManager.Sound.ButtonOver);
            _hasSoundPlayed = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _hasSoundPlayed = false;
    }
}