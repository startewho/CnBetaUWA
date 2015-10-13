using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using Newtonsoft.Json;

namespace CnBetaUWA.Helper
{
   public  class SerializerHelper
    {

       public static string ToJson<T>(T obj)
       {
           return JsonConvert.SerializeObject(obj);
       }


       public static T FromJson<T>(string toString)
       {
           return JsonConvert.DeserializeObject<T>(toString);
       }


        public static string JsonSerializer<T>(T t)

        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof (T));

            using (MemoryStream ms = new MemoryStream())
            {
                ser.WriteObject(ms, t);
                string jsonString = Encoding.UTF8.GetString(ms.ToArray());
                return jsonString;
            }
        }



        /// <summary>

        /// JSON反序列化

        /// </summary>
        public static T JsonDeserialize<T>(string jsonString)

        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof (T));

            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
            {
                T obj = (T) ser.ReadObject(ms);

                return obj;
            }
        }
    }
}
