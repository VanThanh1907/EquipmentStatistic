using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Class để quán lý Ui 
public class UIManager : Singleton <UIManager>
{
    public Image displayImage;
    public TMP_Text nameText,damageText,dispersionText,rateText,reloadText,ammoText,btnUseText,btnRentOutText;
    public Button btnBNB,btnEWAR,btnUse,btnRentOut;
    public Color activeColorUse = new Color(0.05f, 1f, 0.05f);
    public Color activeColorRentOut = new Color(1f, 0.56f, 0.043f);
    public Color disabledColor = new Color(0.6f, 0.6f, 0.6f);

   
    private void Start()
    {
        //Add sự kiện cho các btn
        btnBNB.onClick.AddListener(() => { ApplyWeaponUpgrade(WeaponManager.Instance.selectedWeapon); });
        btnEWAR.onClick.AddListener(() => { ApplyWeaponUpgrade(WeaponManager.Instance.selectedWeapon); });
        btnUse.onClick.AddListener(WeaponManager.Instance.SetUseStatus);
        btnRentOut.onClick.AddListener(WeaponManager.Instance.SetRentOutStatus);
    }

    //Áp dụng nâng cấp,reload UI và lưu lại
    private void ApplyWeaponUpgrade(WeaponUI weaponUI)
    {
        weaponUI.weapon.Upgrade();
        UpdateWeaponUI(weaponUI);
        WeaponManager.Instance.SaveData();
    }


    // Hàm cập nhật lại UI hiển thị thông tin của weapon đang chọn
    public void UpdateWeaponUI(WeaponUI currentWeapon)
    {
        //Gan thong tin theo từng thuộc tính.
        displayImage.sprite = currentWeapon.weapon.weaponIcon;
        nameText.text = currentWeapon.weapon.weaponName;
        damageText.text = currentWeapon.weapon.damage.ToString();
        dispersionText.text = currentWeapon.weapon.dispersion.ToString();
        rateText.text = currentWeapon.weapon.rateOfFire + " RPM";
        reloadText.text = currentWeapon.weapon.reloadSpeed + "%";
        ammoText.text = currentWeapon.weapon.ammo + "/100";
        UpdateButtonUI(currentWeapon);
    }


    //CAp nhat trang thai va mau sac btn.
    public void UpdateButtonUI(WeaponUI weaponUI)
    {
        // Nút USE chỉ được bật nếu không Rent Out
        btnUse.interactable = weaponUI.weapon.status != WeaponStatus.RentedOut;
        btnUseText.color = btnUse.interactable ? activeColorUse : disabledColor;

        //Rent Out bật nếu không đang Use
        btnRentOut.interactable = weaponUI.weapon.status != WeaponStatus.Used;
        btnRentOutText.color = btnRentOut.interactable ? activeColorRentOut : disabledColor;
    }
}
