using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

//Class quan trong dung de quản lý các thông tin logic của súng
public class WeaponManager : Singleton<WeaponManager>
{
   
    public WeaponDataBase weaponManager;
    public List<WeaponData> allWeapons;
    public WeaponUI selectedWeapon;
    public WeaponUI usingWeapon;


    public WeaponUI weaponPrefab;
    public Transform weaponListContent;
    private List<WeaponUI> weaponUIs = new List<WeaponUI>();

   
    private void Start()
    {
        // Tạo dữ liệu và UI weapon
        InitWeaponUIs();
        CreateWeaponUI();
        //Chọn súng đầu tiên trong list
        SelectWeapon(weaponUIs[0]);
        weaponUIs[0].borderObject.SetActive(true);
    }


    // Hàm này dùng để khởi tạo dữ liệu vũ khí từ file đã lưu.
    private void InitWeaponUIs()
    {
        List<WeaponData> saved = SaveManager.LoadData();
        // Nếu chưa có dữ liệu ,tạo dữ liệu từ database gốc
        if (saved == null)
        {
            allWeapons = weaponManager.WeaponDatas.Select(w => w.Clone()).ToList();
            return;
        }

        Dictionary<string, WeaponData> savedDict = saved.ToDictionary(w => w.weaponName, w => w);
        // Duyệt qua danh sách vũ khí gốc để gắn lại icon cho các vũ khí từ file lưu 
        for (int i = 0; i < weaponManager.WeaponDatas.Count; i++)
        {
            if (savedDict.TryGetValue(weaponManager.WeaponDatas[i].weaponName, out WeaponData value))
            {
                // Gắn lại icon gốc từ database vào vũ khí đã lưu
                saved[i].weaponIcon = weaponManager.WeaponDatas[i].weaponIcon;
            }
        }
        allWeapons = saved;
    }

    //Hàm này để khởi tạo súng
    public void CreateWeaponUI()
    {
        //Load qua danh sach cua súng để tạo UI
        foreach (var weapon in allWeapons)
        {
            var item = Instantiate(weaponPrefab, weaponListContent);
            item.InitUIWeapon(weapon);
            weaponUIs.Add(item);
        }
        //Set súng đầu la súng dang được chọn
        selectedWeapon = weaponUIs[0];
        //Gán súng cuối cùng được Use cho usedWeapon
        usingWeapon = weaponUIs.FirstOrDefault(w => w.weapon.status == WeaponStatus.Used);
    }


    //Hàm này sẽ gọi khi click chọn 1 weapon
    public void SelectWeapon(WeaponUI weaponUI)
    {
        // Nếu click vào súng khác thì tắt viền súng hiện tại đi
        if (selectedWeapon != weaponUI)
        {
            selectedWeapon.borderObject.SetActive(false);
        }
        //Set súng hiện tại là súng mới chọn
        selectedWeapon = weaponUI;
        selectedWeapon.borderObject.SetActive(true);
        //Cập nhật thông tin
        UIManager.Instance.UpdateWeaponUI(selectedWeapon);
    }


    //Cap nhat lai trang thái UI cua vũ khí
    private void UpdateWeaponStatus(WeaponUI itemUI, WeaponStatus newStatus)
    {
        itemUI.weapon.status = newStatus;
        itemUI.SetStatusWeapon(newStatus);
    }

    public void SetUseStatus()
    {

        //Nếu trang thái đang sử dụng click vào thì sẽ về none
        if (selectedWeapon.weapon.status == WeaponStatus.Used)
        {
            UpdateWeaponStatus(selectedWeapon, WeaponStatus.None);
            // Khi về None thì ko set nó là lastUsed nữa
            usingWeapon = null;
        }
        else
        {
            //Nếu có used rồi thì tắt use chỗ đó di sau đó set used cái dang chon
            if (usingWeapon != null && usingWeapon != selectedWeapon)
            {
                UpdateWeaponStatus(usingWeapon, WeaponStatus.None);
            }
            UpdateWeaponStatus(selectedWeapon, WeaponStatus.Used);
            //Cap nhat lai usedWeapon
            usingWeapon = selectedWeapon;
        }
        SaveManager.SaveData(allWeapons);
    }


    //Cập nhật khi click RentOUT
    public void SetRentOutStatus()
    {

       
        if (selectedWeapon.weapon.status == WeaponStatus.RentedOut)
        {
            UpdateWeaponStatus(selectedWeapon, WeaponStatus.None);
        }
        else
        {
            UpdateWeaponStatus(selectedWeapon, WeaponStatus.RentedOut);
        }

        SaveManager.SaveData(allWeapons);
    }
    public void SaveData() => SaveManager.SaveData(allWeapons);
}
