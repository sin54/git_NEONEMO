using UnityEngine;

public class IncenPrefab : MonoBehaviour
{
    [SerializeField] private bool isRepeating = true;
    [SerializeField]private float increasingSpeed = 0.001f;
    [SerializeField]private float maxSize = 0.3f;
    private SpriteRenderer SR;
    [SerializeField]private float currentSize = 0f;

    private void Awake()
    {
        SR = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        if (isRepeating)
        {
            transform.localScale = Vector3.zero;
            currentSize = 0f;
        }
    }

    private void Update()
    {
        if (currentSize < maxSize)
        {
            currentSize += increasingSpeed;
            transform.localScale=new Vector3(currentSize, currentSize, currentSize);
            Color spriteColor = SR.color;
            spriteColor.a = 1-currentSize/maxSize;
            SR.color = spriteColor;
        }
        else
        {
            if (!isRepeating)
            {
                transform.localScale = Vector3.zero;
                currentSize = 0f;
            }

        }
       
    }

}
