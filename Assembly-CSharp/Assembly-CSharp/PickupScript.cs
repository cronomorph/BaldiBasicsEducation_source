// PickupScript
using UnityEngine;

public class PickupScript : MonoBehaviour
{
	public GameControllerScript gc;
	public Transform player;
 
	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit raycastHit;
			if (Vector3.Distance(player.position, base.transform.position) < 10f) {
    				switch (raycastHit.transform.name.ToString()) {
	 				case "Pickup_EnergyFlavoredZestyBar":
       					raycastHit.transform.gameObject.SetActive(false);
       					gc.CollectItem(1);
       					break;
	     				case "Pickup_EnergyFlavoredZestyBar":
	   				raycastHit.transform.gameObject.SetActive(false);
	   				gc.CollectItem(2);
	   				break;
	 				case "Pickup_Key":
       					raycastHit.transform.gameObject.SetActive(false);
					gc.CollectItem(3);
       					break;
	     				case "Pickup_BSODA":
					raycastHit.transform.gameObject.SetActive(false);
 					gc.CollectItem(4);
	   				break;
	 				case "Pickup_Quarter":
       					raycastHit.transform.gameObject.SetActive(false);
	     				gc.CollectItem(5);
       					break;
     					case "Pickup_Tape":
	   				raycastHit.transform.gameObject.SetActive(false);
	 				gc.CollectItem(6);
	   				break;
	 				case "Pickup_AlarmClock":
       					raycastHit.transform.gameObject.SetActive(false);
     					gc.CollectItem(7);
       					break;
	     				case "Pickup_WD-3D":
	   				raycastHit.transform.gameObject.SetActive(false);
	 				gc.CollectItem(8);
	  				break;
	 				case "Pickup_SafetyScissors":
       					raycastHit.transform.gameObject.SetActive(false);
     					gc.CollectItem(9);
       					break;
	 			}
			}
		}
	}
}
