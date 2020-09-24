using MongoDB.Bson.Serialization;

namespace ApiDoCesao.General
{
    public static class Utils
    {
        public static void JsonMapClasses<T>()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
            {
                BsonClassMap.RegisterClassMap<T>(item =>
                {
                    item.AutoMap();
                    item.SetIgnoreExtraElements(true);
                });
            }
        }
    }
}
