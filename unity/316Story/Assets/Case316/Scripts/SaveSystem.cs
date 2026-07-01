using System.IO;
using UnityEngine;

namespace Case316
{
    public static class SaveSystem
    {
        const string FileName = "case316_chapter1_save.json";

        public static bool HasSave => File.Exists(SavePath);

        static string SavePath => Path.Combine(Application.persistentDataPath, FileName);

        public static void Save(SaveData data)
        {
            Directory.CreateDirectory(Application.persistentDataPath);
            File.WriteAllText(SavePath, JsonUtility.ToJson(data, true));
        }

        public static SaveData Load()
        {
            return HasSave ? JsonUtility.FromJson<SaveData>(File.ReadAllText(SavePath)) : null;
        }
    }
}
