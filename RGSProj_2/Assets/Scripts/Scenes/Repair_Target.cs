using UnityEngine;
using UnityEngine.Events;

public class Repair_Target : MonoBehaviour
{
    public UnityEvent onClicked; // �����Ϳ��� ���� ����
    public void SetupClickEvent(UnityAction action)
    {
        onClicked.AddListener(action);
    }
    private void OnMouseDown()
    {
        onClicked?.Invoke();
    }
}
