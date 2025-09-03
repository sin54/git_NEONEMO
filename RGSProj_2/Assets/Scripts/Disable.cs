using UnityEngine;

public class Disable : MonoBehaviour
{
    [SerializeField] private Transform disableObj;
    public void DisableGO()
    {
        disableObj.gameObject.SetActive(false);
    }
}
