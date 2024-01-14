using Unity.VisualScripting;
using UnityEngine;

public class PlayerHouseManager : House_Manager
{
    private Vector3 oldPlayerPosition;
    [SerializeField] private GameObject houseEnterPosition;

    public override void WhenHouseEnter()
    {
        titleMenuController.PlayFadeOut();
        oldPlayerPosition = this.player.transform.position;
        this.player.transform.position = new Vector3(this.houseEnterPosition.transform.position.x, this.player.transform.position.y, this.houseEnterPosition.transform.position.z);
        houseEnterPosition.transform.parent.gameObject.SetActive(true);
        this.player.gameObject.GetComponent<PlayerInteractionsController>().DisableDig();
    }

    public void ExitHouse()
    {
        this.player.transform.position = oldPlayerPosition;
        base.PlayExitHouseAnimation();
    }
}
