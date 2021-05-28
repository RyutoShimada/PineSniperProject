using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray();
    }

    /// <summary>
    /// クリックするとカバーを消す
    /// </summary>
    void Ray()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10f))
        {
            if (Input.GetMouseButtonDown(0) && hit.collider.tag == "Cover")
            {
                hit.collider.gameObject.SetActive(false);
            }

            MinesweeperGenerater minesweeperGenerater = hit.collider.GetComponent<MinesweeperGenerater>();

        }
    }
}
