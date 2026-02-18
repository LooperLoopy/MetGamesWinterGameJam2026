using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Position Points")]
    [SerializeField] private Transform right;
    [SerializeField] private Transform left;

    // Position
    private bool isRight = false;
    private Transform playerTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTransform = gameObject.GetComponent<Transform>();

        playerTransform.position = left.position;
    }

    public void switchSides()
    {
        if (isRight)
        {
            playerTransform.position = left.position;
        } 
        else
        {
            playerTransform.position = right.position;
        }
        isRight = !isRight;
    }

    public bool playerIsRight()
    {
        return isRight;
    }
}
