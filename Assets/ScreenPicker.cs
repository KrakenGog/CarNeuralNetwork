using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScreenPicker : MonoBehaviour
{
    public event UnityAction<CarController> OnPicked;

    [SerializeField] private LayerMask _pickMask;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector3.forward, 100, _pickMask);

            if (hit)
            {
                OnPicked?.Invoke(hit.collider.GetComponent<CarController>());
            }
        }
    }
}
