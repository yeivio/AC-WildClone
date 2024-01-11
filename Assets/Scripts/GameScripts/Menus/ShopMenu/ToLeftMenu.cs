
using UnityEngine;
using UnityEngine.UI;


public class ToLeftMenu : MonoBehaviour
{
    [SerializeField] private SellMenu tienda;
    void Update()
    {
        if (tienda.selected == this.GetComponent<Button>() && Input.GetKeyDown(KeyCode.Space))
        {
            tienda.NextPage();

        }
    }
}
