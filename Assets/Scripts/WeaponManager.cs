using UnityEngine;
using System.Collections.Generic;
using System.Linq;


//Class quan trong dung de quản lý các thông tin logic của súng
public class WeaponManager : Singleton<WeaponManager>
{
   
    public WeaponDataBase weaponManager;
    public List<WeaponData> allWeapons;
    public WeaponUI selectedWeapon;
    public WeaponUI usingWeapon;
    public WeaponUI weaponPrefab;
    private List<WeaponUI> weaponUIs = new List<WeaponUI>();
    public Transform weaponListContent;

   
    private void Start()
    {
        //Tạo dữ liệu và UI
        InitWeaponUIs();
        CreateWeaponUI();
        SelectWeapon(weaponUIs[0]);
        weaponUIs[0].borderObject.SetActive(true);
    }


    //Hàm khởi tạo dữ liệu wp
    private void InitWeaponUIs()
    {
        List<WeaponData> saved = SaveManager.LoadData();
        // Nếu chưa có dữ liệu,tạo dữ liệu từ database gốc
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


    //Hàm tạo UI wp
    public void CreateWeaponUI()
    {
        //Load qua danh sach cua súng để tạo UI
        foreach (var weapon in allWeapons)
        {
            var item = Instantiate(weaponPrefab, weaponListContent);
            item.InitUIWeapon(weapon);
            weaponUIs.Add(item);
        }
        selectedWeapon = weaponUIs[0];
        usingWeapon = weaponUIs.FirstOrDefault(w => w.weapon.status == WeaponStatus.Used);
    }


    //Hàm gọi khi click chọn 1 wp
    public void SelectWeapon(WeaponUI weaponUI)
    {
        // Nếu click vào súng khác thì tắt viền súng hiện tại đi
        if (selectedWeapon != weaponUI)
        {
            selectedWeapon.borderObject.SetActive(false);
        }
        //Gán súng vừa click và selectedWP,bật viền và cập nhật UI
        selectedWeapon = weaponUI;
        selectedWeapon.borderObject.SetActive(true);
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

        //Nếu trang thái đang USE thì về NONE
        if (selectedWeapon.weapon.status == WeaponStatus.Used)
        {
            UpdateWeaponStatus(selectedWeapon, WeaponStatus.None);
            usingWeapon = null;
        }
        else
        {
            // Đưa trạng thái của súng đang used về None
            if (usingWeapon != null && usingWeapon != selectedWeapon)
            {
                UpdateWeaponStatus(usingWeapon, WeaponStatus.None);
            }
            UpdateWeaponStatus(selectedWeapon, WeaponStatus.Used);
            //Cap nhat lai usingWeapon
            usingWeapon = selectedWeapon;
        }
        SaveManager.SaveData(allWeapons);
    }


    //Cập nhật khi click RentOUT
    public void SetRentOutStatus()
    {
        //Nếu đang RENT thì chuyển thành NONE và ngược lại.
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
