using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using Data;

namespace GameUtils
{
    public static class Log
    {
        public static void TimeStamp(string text)
        {
            Log.Print(string.Format("[{0}]: {1}", DateTime.UtcNow, text));
        }

        public static void Print(object message)
        {
#if DEBUG
            Debug.Log(message);
#endif
        }
    }

    public static class Random
    {
        public static T RangeCollection<T>(T[] array)
        {
            int index = UnityEngine.Random.Range(0, array.Length - 1);
            return array[index];
        }

        public static T RangeCollection<T>(List<T> list)
        {
            int index = UnityEngine.Random.Range(0, list.Count - 1);
            return list[index];
        }
    }

    public static class Validation
    {
        static public bool IsInternetConnectionAvailable()
        {
            System.Net.WebRequest request = System.Net.HttpWebRequest.Create("http://www.google.com");
            request.Method = "HEAD";
            request.Timeout = 5000;
            System.Net.WebResponse response = null;
            try
            {
                response = request.GetResponse();
            }
            catch
            {
                response = null;
            }
            return response != null;
        }

        public static bool IsFileHashIdentical(FileInfo first, FileInfo second)
        {
            byte[] firstHash = MD5.Create().ComputeHash(first.OpenRead());
            byte[] secondHash = MD5.Create().ComputeHash(second.OpenRead());

            for (int i = 0; i < firstHash.Length; i++)
            {
                if (firstHash[i] != secondHash[i])
                    return false;
            }
            return true;
        }

        public static bool IsStringHashIdentical(string first, string second)
        {
            byte[] encodedFirst = new UTF8Encoding().GetBytes(first);
            byte[] firstHash = MD5.Create().ComputeHash(encodedFirst);

            byte[] encodedSecond = new UTF8Encoding().GetBytes(second);
            byte[] secondHash = MD5.Create().ComputeHash(encodedSecond);

            for (int i = 0; i < firstHash.Length; i++)
            {
                if (firstHash[i] != secondHash[i])
                    return false;
            }
            return true;
        }

        public static bool NumberInRange(float number, float min, float max)
        {
            return number >= min && number <= max;
        }

        public static bool VectorComponentsInRange(Vector3 vector, float min, float max)
        {
            return NumberInRange(vector.x, min, max) && NumberInRange(vector.y, min, max) && NumberInRange(vector.z, min, max);
        }
    }

    public static class Format
    {
        public static Rect FormatGuiTextArea(GUIText guiText, float maxAreaWidth)
        {
            string[] words = guiText.text.Split(' ');
            string result = "";
            Rect textArea = new Rect();

            for (int i = 0; i < words.Length; i++)
            {
                // set the gui text to the current string including new word
                guiText.text = (result + words[i] + " ");
                // measure it
                textArea = guiText.GetScreenRect();
                // if it didn't fit, put word onto next line, otherwise keep it
                if (textArea.width > maxAreaWidth)
                {
                    result += ("\n" + words[i] + " ");
                }
                else
                {
                    result = guiText.text;
                }
            }
            return textArea;
        }

        public static string SafeTimestampNow()
        {
            return SafeTimestamp(DateTime.UtcNow);
        }

        public static string SafeTimestamp(DateTime date)
        {
            return date.ToString(CommonConstants.TIME_FORMAT_SAFE);
        }
    }

    public static class IO
    {
        private static string[] loadedFiles;
        private static int fileCount;

        public static void ReadAllTextUbiquitus(MonoBehaviour hookBehaviour, string filePath, Action<string> callback)
        {
            loadedFiles = new string[1];
            hookBehaviour.StartCoroutine(IO.LoadFileUbiquitus(0, filePath, callback));
        }

        public static void ReadAllTextUbiquitus(MonoBehaviour hookBehaviour, string []filePaths, Action<string[]> callback)
        {
            fileCount = 0;
            loadedFiles = new string[filePaths.Length];
            for (int i = 0; i < filePaths.Length; i++)
            {
                hookBehaviour.StartCoroutine(IO.LoadFileUbiquitus(i, filePaths[i], IncreaseFileCount));
            }
            hookBehaviour.StartCoroutine(IO.CheckLoadedFiles(callback));
        }

        private static void IncreaseFileCount(string content)
        {
            fileCount++;
            Log.Print("IncreaseFileCount: " + fileCount.ToString());
        }

        private static IEnumerator CheckLoadedFiles(Action<string[]> callback)
        {
            Log.Print("CheckLoadedFiles");
            while (fileCount < loadedFiles.Length)
            {
                yield return null;
            }

            Log.Print("Calling final callback...");
            callback(loadedFiles);
        }

        private static IEnumerator LoadFileUbiquitus(int index, string filePath, Action<string> callback)
        {
            Log.Print("Try loading file at: " + filePath);

            if (filePath.Contains("://"))
            {
                WWW www = new WWW(filePath);
                yield return www;
                Log.Print("Load from url");
                loadedFiles[index] = www.text;
            }
            else
            {
                Log.Print("Load from path");
                loadedFiles[index] = ReadTextStreamingFile(filePath);
            }


            Log.Print("Calling: " + callback.ToString());
            if (callback != null)
            {
                callback(loadedFiles[index]);
            }

        }

        private static string ReadTextStreamingFile(string filePath)
        {
            try
            {
                using (FileStream fsSource = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {

                    // Read the source file into a byte array. 
                    byte[] bytes = new byte[fsSource.Length];
                    int numBytesToRead = (int)fsSource.Length;
                    int numBytesRead = 0;
                    while (numBytesToRead > 0)
                    {
                        // Read may return anything from 0 to numBytesToRead. 
                        int n = fsSource.Read(bytes, numBytesRead, numBytesToRead);

                        // Break when the end of the file is reached. 
                        if (n == 0)
                            break;

                        numBytesRead += n;
                        numBytesToRead -= n;
                    }
                    numBytesToRead = bytes.Length;
                    string result = System.Text.Encoding.Default.GetString(bytes);
                    return result;
                }
            }
            catch
            {
                Debug.LogError("An error occurred while reading " + filePath);
            }
            return "";
        }

        public static void DeserializeObjects(string filePath, SerializableInterface[] interfaces)
        {
            string fullPath = Application.persistentDataPath + "/" + filePath;
            Log.Print("Reading " + fullPath + "...");
            FileStream fileStream = new FileStream(fullPath, FileMode.OpenOrCreate);
            if (fileStream.Length == 0)
            {
                Log.Print(fullPath + " is empty");
                return;
            }

            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                string gameVersion = (string)formatter.Deserialize(fileStream);
                if(gameVersion != CommonConstants.GAME_FILE_VERSION)
                {
                    //if (File.Exists(fullPath))
                    //{
                    //    Log.Print("Removing: " + fullPath);
                    //    File.Delete(fullPath);
                    //}
                    string message = string.Format("File version mismatch: current[{0}] loaded[{1}]",CommonConstants.GAME_FILE_VERSION, gameVersion);
                    throw (new SerializationException(message));
                }

                foreach (SerializableInterface serializable in interfaces)
                {
                    int count = (int)formatter.Deserialize(fileStream);
                    object[] objects = new object[count];
                    for (int i = 0; i < count; i++)
                    {
                        objects[i] = formatter.Deserialize(fileStream);
                    }
                    serializable.Deserialize(objects);
                }
            }
            catch (SerializationException e)
            {
               Debug.LogError("Failed to deserialize. Reason: " + e.Message);
               throw;
            }
            finally
            {
                fileStream.Close();
                Log.Print("Done reading " + filePath);
            }
        }

        public static void SerializeObjects(string filePath, SerializableInterface[] interfaces)
        {
            string fullPath = Application.persistentDataPath + "/" + filePath;
            Log.Print("Writing " + fullPath + "...");
            FileStream fileStream = new FileStream(fullPath, FileMode.OpenOrCreate);

            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fileStream, CommonConstants.GAME_FILE_VERSION);
                foreach (SerializableInterface serializable in interfaces)
                {
                    object[] objects = serializable.Serialize();
                    formatter.Serialize(fileStream, objects.Length);
                    foreach (object serializableObject in objects)
                    {
                        try
                        {
                            formatter.Serialize(fileStream, serializableObject);
                        }
                        catch (SystemException e)
                        {
                            Debug.LogError(e.Message);
                        }
                    }
                }
            }
            catch (SerializationException e)
            {
                Debug.LogError("Failed to serialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fileStream.Close();
                Log.Print("Done writing " + filePath);
            }
        }

        public static void SaveTextureToFile(string filePath, Texture2D texture)
        {
            byte[] bytes;
            if (filePath.ToLower().EndsWith(".jpg"))
            {
                bytes = texture.EncodeToJPG();
            }
            else
            {
                bytes = texture.EncodeToPNG();
            }
            FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate);
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(bytes);
            writer.Close();
            stream.Close();
        }

        public static void UploadTexture(string fieldName, string extension, Texture2D texture, WWWForm uploadForm)
        {
            if (texture != null && fieldName != null && uploadForm != null)
            {
                byte[] bytes;
                string mimeType;
                if (extension.ToLower() == ("jpg"))
                {
                    bytes = texture.EncodeToJPG();
                    mimeType = "image/jpeg";
                }
                else
                {
                    bytes = texture.EncodeToPNG();
                    mimeType = "image/png";
                }
                uploadForm.AddBinaryData(fieldName, bytes, fieldName + "." + extension, mimeType);
            }
        }
    }

    public class Callbacks
    {
        public static void ReceiveSuccess(bool success)
        {
#if DEBUG
            System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
            Log.Print(stackTrace.GetFrame(1).GetMethod().Name + ": " + success);
#endif
        }
    }

    public class GraphicsUtils
    {
        public static void ResizeRendererToScreen<T>(GameObject obj, float deltaPerc = 1f) where T : Renderer
        {
            T meshRenderer = obj.GetComponent<T>();
            if (meshRenderer == null)
            {
                return;
            }

            obj.transform.localScale = Vector3.one;

            float width = meshRenderer.bounds.size.x;
            float height = meshRenderer.bounds.size.y;
            float worldScreenHeight = Camera.main.orthographicSize * 2f;
            float worldScreenWidth = (worldScreenHeight / Screen.height) * Screen.width;

            Vector3 newScale = new Vector3((worldScreenWidth / width) * deltaPerc, (worldScreenHeight / height) * deltaPerc, 1f);
            obj.transform.localScale = newScale;
        }

        public static Texture2D TakeScreenshot(int width, int height, Camera screenshotCamera)
        {
            Assert.That(width > 0 || height > 0, "Invalid size for screenshot texture");
            if (screenshotCamera == null)
            {
                screenshotCamera = Camera.main;
            }   
            Texture2D screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);
            RenderTexture renderTex = new RenderTexture(width, height, 24);
            screenshotCamera.targetTexture = renderTex;
            screenshotCamera.Render();
            RenderTexture.active = renderTex;
            screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            screenshot.Apply(false);
            screenshotCamera.targetTexture = null;
            RenderTexture.active = null;
            if (Application.isEditor)
            {
                GameObject.DestroyImmediate(renderTex);
            }
            else
            {
                GameObject.Destroy(renderTex);
            }
            return screenshot;
        }
    }
}