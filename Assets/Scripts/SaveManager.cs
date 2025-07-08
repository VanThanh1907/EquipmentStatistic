
using UnityEngine;
using System.IO;
using Newtonsoft.Json;    
using System.Collections.Generic;


//Class quán lý lưu, dọc dữ liệu.
public static class SaveManager
{
    private static string path = Application.persistentDataPath + "/save.json";

    // Hàm lưu danh sách các weapon vào file JSON 
    public static void SaveData(List<WeaponData> weapons)
    {
        string json = JsonConvert.SerializeObject(weapons, Formatting.Indented);
        File.WriteAllText(path, json);
    }


    // Hàm đọc dữ liệu từ file 
    public static List<WeaponData> LoadData()
    {
        // Nếu file khong ton tai thi return ve null
        if (!File.Exists(path))
        {
            return null;
        }
        string json = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<List<WeaponData>>(json);
    }


}