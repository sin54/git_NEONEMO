using UnityEngine;
using UnityEngine.Events;

public class Repair_Target : MonoBehaviour
{
    public UnityEvent onClicked; // 에디터에서 연결 가능
    public void SetupClickEvent(UnityAction action)
    {
        onClicked.AddListener(action);
    }
    private void OnMouseDown()
    {
        onClicked?.Invoke();
    }
}
