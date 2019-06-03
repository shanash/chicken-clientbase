namespace Common
{
    public abstract class JsonObject
    {
        public static JsonObject Empty = new EmptyJsonObject();
        public const string kEmptyBody = "{}";

        private class EmptyJsonObject : JsonObject
        {
            public override string ToJson() { return kEmptyBody; }
        }

        /// <summary>
        /// JSON 문자열을 파싱하여 객체를 생성한다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json">JSON 문자열</param>
        /// <returns>생성된 JsonObject 객체</returns>
        /// <usage>
        ///     JsonObjectModel model = JsonObject.Parse<JsonObjectModel>(json);
        /// </usage>
        public static T Parse<T>(string json) where T : JsonObject
        {
            return JsonHelper.FromJson<T>(json);
        }

        public static bool IsNullOrEmpty(JsonObject jo)
        {
            if (jo == null) return true;
            
            string json = jo.ToJson();
            if (string.IsNullOrEmpty(json)) return true;
            if (string.Equals(json, kEmptyBody)) return true;

            return false;
        }

        /// <summary>
        /// 객체의 JSON 문자열을 얻는다.
        /// </summary>
        /// <returns>JSON 문자열</returns>
        public virtual string ToJson()
        {
            return JsonHelper.ToJson(this);
        }

        public override string ToString()
        {
            return this.ToJson();
        }
    }
}
