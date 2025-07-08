using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class WeaponUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text statusText;
    public Button selectButton;
    public GameObject borderObject;
    public WeaponData weapon; 
   
   
    [SerializeField] private Color rentedColor = new Color(1f, 0.56f, 0.043f);
    [SerializeField] private Color usedColor = new Color(0.05f, 1f, 0.05f);


   // Khởi tạo UI wp
    public void InitUIWeapon(WeaponData data)
    {
        //Gán ảnh,tắt hết boder đi,set trạng thái cho wp
        weapon = data;
        icon.sprite = data.weaponIcon;
        borderObject.SetActive(false);
        selectButton.onClick.AddListener(() =>
        {
            WeaponManager.Instance.SelectWeapon(this);
        });
        SetStatusWeapon(data.status);
    }

    
    //Set trạng thai wp
    public void SetStatusWeapon(WeaponStatus status)
    {
        switch (status)
        {
            //USE
            case WeaponStatus.Used:
                statusText.text = "Used";
                statusText.color = usedColor;
                break;
           //RENTED OUT
            case WeaponStatus.RentedOut:
                statusText.text = "Rented Out";
                statusText.color = rentedColor;
                break;
            //NONE
            default:
                statusText.text = "";
                break;
        }
        UIManager.Instance.UpdateButtonUI(this);
    }
}
