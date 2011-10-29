using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Json;

namespace Digipostsync.Core
{
    [DataContract, Serializable]
    public class DigipostFile
    {
        [DataMember(Name="brevUri")]
        public String BrevUri { get; set; }

        [DataMember(Name = "selvopplastet")]
        public bool Selvopplastet { get; set; }

    }
    [DataContract, Serializable]
    public class DigipostKonto
    {
        [DataMember(Name = "arkivUri")]
        public String ArkivUri { get; set; }

        [DataMember(Name = "token")]
        public String Token { get; set; }
    }

    [DataContract, Serializable]
    public class DigipostLocalFile
    {
        [DataMember]
        public String Uri { get; set; }

        [DataMember]
        public String Filename { get; set; }

        [DataMember]
        public String Hash { get; set; }

        [DataMember]
        public long Length { get; set; }

        public FileDescriptor FileDescriptor
        {
            get { return new FileDescriptor(Hash, Length, Filename); }
        }

    }

    public class JSONHelper
    {
        public static T Deserialise<T>(string json)
        {
            using (MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                return (T)serializer.ReadObject(ms); 
            } 
        }
        public static String Serialise<T>(T obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, obj);
                return Encoding.Default.GetString(ms.ToArray());
            }

        }
    }

}
