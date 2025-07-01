using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Xml.Linq;

public class WeaponUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text statusText;
    public Button selectButton;
    public GameObject borderObject;
    public WeaponData weapon; 
   
   
    [SerializeField] private Color rentedColor = new Color(1f, 0.56f, 0.043f);
    [SerializeField] private Color usedColor = new Color(0.05f, 1f, 0.05f);


    // Dung de khoi tao UI cua mot vu khi
    public void InitUIWeapon(WeaponData data)
    {
        weapon = data;
        icon.sprite = data.weaponIcon;
        borderObject.SetActive(false);
        selectButton.onClick.AddListener(() =>
        {
            WeaponManager.Instance.SelectWeapon(this);
        });
        SetStatusWeapon(data.status);
    }

    
    //Set trạng thai cho sung
    public void SetStatusWeapon(WeaponStatus status)
    {
        switch (status)
        {
            case WeaponStatus.Used:
                statusText.text = "Used";
                statusText.color = usedColor;
                break;
           
            case WeaponStatus.RentedOut:
                statusText.text = "Rented Out";
                statusText.color = rentedColor;
                break;
            default:
                statusText.text = "";
                break;
        }
        UIManager.Instance.InteractiveButton(this);

    }
}
