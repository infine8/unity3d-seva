using System;
using System.IO;
using System.Xml;
using UnityEngine;

namespace log4net
{
    public static class LogConfigXml
    {
        public static readonly string FILE_APPENDER_NAME = "RollingFile";
        public static readonly string UNITY_CONSOLE_APPENDER_NAME = "UnityConsole";
        public static readonly string TEST_FLIGHT_APPENDER_NAME = "TestFlight";

        public static string LevelValue { get; set; }

        private static string AppenderRef { get; set; }

        private static string _rootNodeName = "log4net/root";

        private static string _xmlPath = Application.dataPath + "/Toolkits/Logging/Valion/Resources/logging.xml";

        private static XmlDocument _xmlDocument;
        private static XmlAttribute _levelValAttr;
        private static XmlAttribute _appenderRefAttr;


        public static void Load()
        {
            var xml = string.Empty;

            using (var sr = new StreamReader(_xmlPath))
            {
                xml = sr.ReadToEnd();
            }

            LoadXml(xml);
        }

        public static void LoadXml(string xml)
        {
            _xmlDocument = new XmlDocument();

            _xmlDocument.LoadXml(xml);

            var levelNode = _xmlDocument.SelectSingleNode(_rootNodeName + "/level");
            if (levelNode != null)
            {
                _levelValAttr = levelNode.Attributes["value"];
                LevelValue = _levelValAttr.Value;
            }

            var appenderNode = _xmlDocument.SelectSingleNode(_rootNodeName + "/appender-ref");
            if (appenderNode != null)
            {
                _appenderRefAttr = appenderNode.Attributes["ref"];
                AppenderRef = _appenderRefAttr.Value;
            }
        }


        private static void SaveXml()
        {
            _appenderRefAttr.Value = AppenderRef;
            _levelValAttr.Value = LevelValue;

            _xmlDocument.Save(_xmlPath);

            _xmlDocument = null;
        }

        public static void SetLogLevel(string level)
        {
            LevelValue = level;
        }

        public static string SetEditorAppender()
        {
            AppenderRef = _appenderRefAttr.Value = UNITY_CONSOLE_APPENDER_NAME;
            
            return _xmlDocument.InnerXml;
        }

        public static void SetBuildAppender(RuntimePlatform buildTarget)
        {
            if (buildTarget == RuntimePlatform.Android || buildTarget == RuntimePlatform.IPhonePlayer) AppenderRef = TEST_FLIGHT_APPENDER_NAME;
            if (buildTarget == RuntimePlatform.WindowsPlayer || buildTarget == RuntimePlatform.OSXPlayer) AppenderRef = FILE_APPENDER_NAME;

            SaveXml();
        }


    }
}

