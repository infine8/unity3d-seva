using System.Text;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System;

namespace UniKid.Core.Model.Xml
{
    public sealed class XmlResource
    {
        public static readonly string BACKUP_NAME_POSTFIX = ".bak";
        public static readonly string EDITOR_TEMP_FOLDER = "assets/temp/";

        private const string XOR_KEY = @"afhnqwuivbnqefopvnefuovbqefuovbevef14|HU(HUAhd7923dh2d2ejdnL";


        public static T LoadFromResources<T>(string path) where T : class
        {
            TextAsset textAsset = null;
            StringReader stringReader = null;
            XmlSerializer xmlSerializer = null;
            T deserializedObject = null;
            try
            {
                textAsset = Resources.Load(path, typeof(TextAsset)) as TextAsset;
                if (textAsset == null) throw new System.Exception("null resource");

                if (typeof(T) == typeof(XmlDocument))
                {
                    var doc = new XmlDocument();
                    doc.LoadXml(textAsset.text);
                    deserializedObject = doc as T;
                }
                else
                {
                    stringReader = new StringReader(textAsset.text);
                    xmlSerializer = new XmlSerializer(typeof(T));
                    deserializedObject = xmlSerializer.Deserialize(stringReader) as T;
                    stringReader.Close();
                }

            }
            catch (System.Exception e)
            {
                Debug.LogError("Unable to load file at : " + path + "\n");
                if (stringReader != null) stringReader.Close();
            }
            finally
            {
                textAsset = null;
                Resources.UnloadUnusedAssets();
            }

            return deserializedObject;
        }

        private static T LoadFromString<T>(string str) where T : class
        {
            StringReader stringReader = null;
            XmlSerializer xmlSerializer = null;
            T deserializedObject = null;

            if (typeof(T) == typeof(XmlDocument))
            {
                var doc = new XmlDocument();
                doc.LoadXml(str);
                deserializedObject = doc as T;
            }
            else
            {
                stringReader = new StringReader(str);
                xmlSerializer = new XmlSerializer(typeof(T));
                deserializedObject = xmlSerializer.Deserialize(stringReader) as T;
                stringReader.Close();
            }

            return deserializedObject;
        }

        public static string GetPathToSave()
        {
            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor) return EDITOR_TEMP_FOLDER;
            var mobilePath = Application.persistentDataPath;
            return mobilePath.EndsWith("/") ? mobilePath : mobilePath + "/";
        }


        public static T LoadFromFile<T>(string path) where T : class
        {
            var text = File.OpenText(Application.dataPath + "/" + path).ReadToEnd();

            if (string.IsNullOrEmpty(text)) throw new Exception("null resource");

            return LoadFromString<T>(text);
        }

        public static T LoadSavingFromFile<T>(string path, bool useEncryption) where T : class, new()
        {
            return LoadSavingFromFile<T>(path, useEncryption, true);
        }

        public static T LoadSavingFromFile<T>(string path, bool useEncryption, bool backup, bool notASaveFile = false) where T : class, new()
        {
            string filename;
            StreamReader streamReader = null;
            XmlSerializer xmlSerializer = null;
            T deserializedObject = null;

            // Определение имени файла

            filename = GetPathToSave() + path;
            // загрузка из файла
            try
            {
                //Debug.Log(string.Format("Loading from file with encryption set to {0}", useEncryption));

                //if (!notASaveFile && OldSaveFileExists(filename) && useEncryption)
                //{
                //    Debug.LogWarning(string.Format("Found unencrypted save file at {0}. Encrypting it now", filename));
                //    EncryptXOR(filename);
                //}

                if (useEncryption) EncryptXOR(filename);

                streamReader = new StreamReader(filename, Encoding.UTF8);

                if (typeof(T) == typeof(XmlDocument))
                {
                    var doc = new XmlDocument();
                    doc.LoadXml(streamReader.ReadToEnd());
                    deserializedObject = doc as T;
                }
                else
                {
                    xmlSerializer = new XmlSerializer(typeof(T));
                    deserializedObject = xmlSerializer.Deserialize(streamReader) as T;
                }

                streamReader.Close();
            }
            catch (Exception e)
            {
                Debug.LogWarning("Unable to load file at : " + path + "\n" + e.Message);

                // Удаляем некорректный файл
                if (streamReader != null)
                    streamReader.Close();
                if (File.Exists(filename))
                    File.Delete(filename);

                if (backup)
                {
                    Debug.LogWarning("Attempt to restore from backup");

                    string backupFilename = filename + BACKUP_NAME_POSTFIX;
                    if (File.Exists(backupFilename))
                    {
                        File.Copy(backupFilename, filename, true);
                        deserializedObject = LoadSavingFromFile<T>(path, useEncryption, false);
                    }
                    else
                    {
                        Debug.LogWarning("Unable to find backup file at : " + backupFilename);
                    }
                }
            }
            finally
            {
                //
            }

            return deserializedObject;
        }

        public static bool SaveToFile(string path, object data, bool useEncryption)
        {
            return SaveToFile(path, data, useEncryption, true, false);
        }

        public static bool SaveToFile(string path, object data, bool useEncryption, bool backup, bool isForcePath)
        {
            bool saved = false;
            string filepath;

            filepath = isForcePath ? path : GetPathToSave() + path;

            //Debug.Log("Saving to: " + filepath);

            if (backup && File.Exists(filepath))
            {
                try
                {
                    File.Copy(filepath, filepath + BACKUP_NAME_POSTFIX, true);
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Unable to create backup file at : " + path + "\n" + e.Message);
                }
            }

            //FileStream stream = null;
            //XmlSerializer xmlSerializer = null;
            //XmlWriter xmlWriter = null;
            StreamWriter streamWriter = null;


            //try
            //{
                string serializedValue;

                var serializer = new XmlSerializer(data.GetType());

                var settings = new XmlWriterSettings();
                settings.Encoding = Encoding.UTF8; // no BOM in a .NET string
                settings.Indent = false;
                settings.OmitXmlDeclaration = false;

                using (StringWriter textWriter = new StringWriter())
                {
                    using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                    {
                        serializer.Serialize(xmlWriter, data);
                    }

                    serializedValue = textWriter.ToString();

					//Debug.Log("serializedValue:" + serializedValue);
                }

                using (streamWriter = new StreamWriter(filepath))
                {
                    streamWriter.Write(serializedValue);
                }

                ////Debug.Log(string.Format("Saving to file with encryption set to {0}", useEncryption));
                //stream = new FileStream(filepath, FileMode.Create);
                //xmlSerializer = new XmlSerializer(data.GetType());
				
                //xmlSerializer.Serialize(stream, data);
                ////streamWriter.Flush();
                //stream.Close();

                if (useEncryption) EncryptXOR(filepath);

                saved = true;
                //Debug.LogWarning("Saved at: " + filepath);
			/*  }

            catch (System.Exception e)
            {
                Debug.LogError("Unable to save file at : " + path + "\n");
				Debug.Log(e.InnerException);
                if (streamWriter != null)
                    streamWriter.Close();
            }
            finally
            {
                //
            }
*/
            return saved;
        }


        /// <summary>
        /// Checks if not encrypted save file already exists(for backward compatibility)
        /// </summary>
        /// <param name="filePath">The whole path(including GetPathToSave())</param>
        /// <returns></returns>
        public static bool OldSaveFileExists(string filePath)
        {
            if (!File.Exists(filePath))
                return false;

            StreamReader sr = new StreamReader(filePath);
            string contents = sr.ReadToEnd();
            sr.Close();
            return contents.Contains("<?xml version=\"1.0\" encoding=\"utf-8\"?>") || contents.Contains(XmlUserData.ENCRYPTION_CHECK_STRING);
        }

        #region Cryptography(might help someday)
        //public static void EncryptFile(string inputFile, string outputFile)
        //{
        //	try
        //	{
        //		UnicodeEncoding UE = new UnicodeEncoding();
        //		byte[] key = UE.GetBytes(ENCRYPTION_KEY);

        //		string cryptFile = outputFile;
        //		FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

        //		RijndaelManaged RMCrypto = new RijndaelManaged();

        //		CryptoStream cs = new CryptoStream(fsCrypt,
        //			RMCrypto.CreateEncryptor(key, key),
        //			CryptoStreamMode.Write);

        //		FileStream fsIn = new FileStream(inputFile, FileMode.Open);

        //		int data;
        //		while ((data = fsIn.ReadByte()) != -1)
        //			cs.WriteByte((byte)data);

        //		fsIn.Close();
        //		cs.Close();
        //		fsCrypt.Close();
        //	}
        //	catch (Exception ex)
        //	{
        //		Debug.LogError(string.Format("Encryption failed! File: {0}", ex.Message));
        //	}
        //}

        //private static void DecryptFile(string inputFile, string outputFile)
        //{
        //	UnicodeEncoding UE = new UnicodeEncoding();
        //	byte[] key = UE.GetBytes(ENCRYPTION_KEY);

        //	FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

        //	RijndaelManaged RMCrypto = new RijndaelManaged();

        //	CryptoStream cs = new CryptoStream(fsCrypt,
        //		RMCrypto.CreateDecryptor(key, key),
        //		CryptoStreamMode.Read);

        //	FileStream fsOut = new FileStream(outputFile, FileMode.Create);

        //	int data;
        //	while ((data = cs.ReadByte()) != -1)
        //		fsOut.WriteByte((byte)data);

        //	fsOut.Close();
        //	cs.Close();
        //	fsCrypt.Close();
        //}

        public static void EncryptXOR(string inputFile)
        {
            //    try
            //    {
            //        string tempFilename = inputFile + "_tmp";
            //        string backupFileName = inputFile + "_bak";

            //        UnicodeEncoding UE = new UnicodeEncoding();
            //        byte[] key = UE.GetBytes(XOR_KEY);

            //        FileStream inputStream = new FileStream(inputFile, FileMode.Open);
            //        FileStream outputStream = new FileStream(tempFilename, FileMode.Create);

            //        int data;

            //        int i = 0;
            //        while ((data = inputStream.ReadByte()) != -1)
            //        {
            //            outputStream.WriteByte((byte)(data ^ (int)key[i]));
            //            if (i == key.Length - 1)
            //                i = 0;
            //            else
            //                i++;
            //        }

            //        outputStream.Flush();
            //        outputStream.Close();
            //        inputStream.Close();

            //        File.Replace(tempFilename, inputFile, backupFileName);

            //        if (File.Exists(backupFileName)) File.Delete(backupFileName);
            //    }
            //    catch (Exception ex)
            //    {
            //        Debug.LogWarning(string.Format("Failed to XOR-encrypt this: {0}", ex.Message));
            //    }
        }
        #endregion
    }

}
