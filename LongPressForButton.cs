using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LongPressForButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
{
    public Button.ButtonClickedEvent onLongPress;

    public float longPressDelay = 1; // second

    bool longPressWaitStarted;
    float longPressTimeLeft;

    readonly int longPressMovesCount = 3;
    int longPressMovesLeft;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnMouseClick);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        longPressWaitStarted = true;
        longPressTimeLeft = longPressDelay;
        longPressMovesLeft = longPressMovesCount;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (Guard.Only(longPressWaitStarted)) return;
        longPressMovesLeft -= 1;
        if (longPressMovesLeft <= 0)
        {
            CancelLongPress();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        longPressWaitStarted = false;
        StartCoroutine(RestoreMouseClickAfterDelay());
    }

    IEnumerator RestoreMouseClickAfterDelay()
    {
        yield return new WaitForSeconds(0.01f);

        RestoreMouseClick();
    }

    void Update()
    {
        if (Guard.Only(longPressWaitStarted)) { return; }
        longPressTimeLeft -= Time.deltaTime;
        if (longPressTimeLeft <= 0)
        {
            LongPress();
        }
    }

    void CancelLongPress()
    {
        longPressWaitStarted = false;
    }

    void LongPress()
    {
        longPressWaitStarted = false;
        onLongPress?.Invoke();
        CancelMouseClick();
    }

    void OnMouseClick()
    {
        CancelLongPress();
    }

    void CancelMouseClick()
    {
        GetComponent<Button>().interactable = false;
    }

    void RestoreMouseClick()
    {
        GetComponent<Button>().interactable = true;
    }

}
