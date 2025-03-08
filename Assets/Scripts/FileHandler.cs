using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using System.Linq;
using UnityEditor;

public static class FileHandler {
    private static readonly string SAVE_FOLDER = Application.dataPath + "/Saves/";
    public static void Init() {
        if (!Directory.Exists(SAVE_FOLDER)) {
            Directory.CreateDirectory(SAVE_FOLDER);
        }
    }
    public static void SaveToJson<T>(List<T> toSave) {
        string content = JsonHelper.ToJson<T>(toSave.ToArray());
        WriteFile(SAVE_FOLDER, content);
    }

    public static List<T> ReadFromJson<T>(string filename) {
        string content = ReadFile(SAVE_FOLDER);
        if (string.IsNullOrEmpty(content) || content == "") {
            return new List<T>();
        }
        
        List<T> res = JsonHelper.FromJson<T>(content).ToList();

        return res;
    }


    private static void WriteFile(string path, string content) {
        int saveNumber = 1;
        while (File.Exists(SAVE_FOLDER + "save_" + saveNumber + ".txt")) {
            saveNumber++;
        }
        string fileName = SAVE_FOLDER + "save_" + saveNumber + ".txt";

        FileStream fileStream = new FileStream(fileName, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(fileStream)) {
            writer.Write(content);
        }
    }

    private static string ReadFile(string path) {
        DirectoryInfo directoryInfo = new DirectoryInfo(SAVE_FOLDER);
        FileInfo[] saveFiles = directoryInfo.GetFiles("*.txt");

        FileInfo mostRecentFile = null;
        foreach (FileInfo fileInfo in saveFiles) {
            if (mostRecentFile == null)
                mostRecentFile = fileInfo;

            else if (fileInfo.LastWriteTime > mostRecentFile.LastWriteTime) {
                FileInfo temp = null;
                temp = mostRecentFile;
                mostRecentFile = fileInfo;
                File.Delete(temp.FullName);
                File.Delete(temp.FullName + ".meta");
            }
            else {
                File.Delete(fileInfo.FullName + ".meta");
                File.Delete(fileInfo.FullName);
            }

        }

        if (mostRecentFile != null) {
            string content = File.ReadAllText(mostRecentFile.FullName);
            return content;
        }
        return "";
    }

  

    public static string Load() {
        DirectoryInfo directoryInfo = new DirectoryInfo(SAVE_FOLDER);
        FileInfo[] saveFiles = directoryInfo.GetFiles("*.txt");

        FileInfo mostRecentFile = null;
        foreach (FileInfo fileInfo in saveFiles) {
            if (mostRecentFile == null)
                mostRecentFile = fileInfo;

            else if (fileInfo.LastWriteTime > mostRecentFile.LastWriteTime) {
                FileInfo temp = null;
                temp = mostRecentFile;
                mostRecentFile = fileInfo;
                File.Delete(temp.FullName);
            }
            else
                File.Delete(fileInfo.FullName);

        }

        //if there is a most recent, then return the string
        if (mostRecentFile != null) {
            string saveString = File.ReadAllText(mostRecentFile.FullName);
            return saveString;
        }
        else {
            return null;
        }
    }


    public static class JsonHelper {
        public static T[] FromJson<T>(string json) {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array) {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint) {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [Serializable]
        private class Wrapper<T> {
            public T[] Items;
        }
    }
}
