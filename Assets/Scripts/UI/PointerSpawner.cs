using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerSpawner: MonoBehaviour
{
    public static PointerSpawner Instance;

    [SerializeField] private GameObject pointerPrefab;
    [SerializeField] private Canvas canvas;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void CreateNewPointer(Vector3 objectivePosition)
    {
        Pointer newPointer = Instantiate(pointerPrefab, canvas.transform).GetComponent<Pointer>();
        newPointer.Construct(objectivePosition);
    }
}
