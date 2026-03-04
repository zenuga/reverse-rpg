using UnityEngine;

public class ClasSelect : MonoBehaviour
{
    public GameObject crate;
    public GameObject cube;

    public Vector3 newPosition;
    public Vector3 originalPosition;

    public int item;

    public void OnButtonClick()
    {
        if (item == 1)
        {
            item = 0;
            cube.transform.position = newPosition;
            crate.transform.position = originalPosition;
        }
        else
        {
            item = 1;
            crate.transform.position = newPosition;
            cube.transform.position = originalPosition;
        }
    }
}
