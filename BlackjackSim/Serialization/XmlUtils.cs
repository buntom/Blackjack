using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Globalization;
using Diagnostics.Logging;

namespace BlackjackSim.Serialization
{
    public static class XmlUtils
    {
        public static T DeserializeFromFile<T>(string fileName) where T : class
        {
            try
            {
                if (!File.Exists(fileName))
                {
                    throw new FileNotFoundException(string.Format("File not found: {0}", fileName));
                }

                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                var xs = new XmlSerializer(typeof(T));

                using (var reader = new StreamReader(fileName))
                {
                    var document = (T)xs.Deserialize(reader);
                    TraceWrapper.LogInformation("Deserialization from file {0} successful.", fileName);
                    return document;
                }
            }
            catch (Exception exception)
            {
                var message = string.Format("Deserialization from file {0} failed.", fileName);
                TraceWrapper.LogException(exception, message);
                throw;
            }
        }

        public static void SerializeToFile<T>(T item, string fileName) where T : class
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                var xs = new XmlSerializer(typeof(T));

                using (var writer = new StreamWriter(fileName))
                {
                    xs.Serialize(writer, item);
                    TraceWrapper.LogInformation("Serialization to file {0} successful.", fileName);
                }
            }
            catch (Exception exception)
            {
                var message = string.Format("Serialization to file {0} failed.", fileName);
                TraceWrapper.LogException(exception, message);
                throw;
            }
        }
    }
}