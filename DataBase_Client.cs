using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mongo_DB
{
    /// <summary>
    /// Клиент для работы с Mongo_DB.
    /// Универсальный, может использоваться в других проектах
    /// </summary>
    public class DataBase_Client
    {
        private string Mongo_Server_Path;
        public MongoClient mongoClient { get; private set; }
        public IMongoDatabase Database { get; private set; }

        /// <summary>
        /// Конструктор клиента для работы с БД
        /// </summary>
        /// <param name="Server_Path">Адрес сервера</param>
        /// <param name="DB_Name">Имя базы данных</param>
        public DataBase_Client(string Server_Path, string DB_Name)
        {
            Mongo_Server_Path = Server_Path;
            mongoClient = new MongoClient(Mongo_Server_Path);
            Database = mongoClient.GetDatabase(DB_Name);
        }        

        /// <summary>
        /// Добавление новой записи в коллекцию
        /// </summary>
        /// <typeparam name="T">Тип добавляемого объекта</typeparam>
        /// <param name="Obj">Добавляемый объект</param>
        /// <param name="CollectionName">Название коллекции</param>
        public async Task AddToCollectionAsync<T>(T Obj, string CollectionName)
        {
            IMongoCollection<T> Collection = Database.GetCollection<T>(CollectionName);
            await Collection.InsertOneAsync(Obj);
        }

        /// <summary>
        /// Получение всех элементов коллекции
        /// </summary>
        /// <typeparam name="T">Тип элементов коллекции</typeparam>
        /// <param name="CollectionName">Имя коллекции</param>
        /// <returns>Список List<T> в котором содержатся все элементы коллекции</returns>
        public async Task<List<T>> Get_AllDocs<T>(string CollectionName)
        {
            IMongoCollection<T> Collection = Database.GetCollection<T>(CollectionName);
            BsonDocument filter = new BsonDocument();
            List<T> Data = await Collection.Find(filter).ToListAsync();
            return Data;
        }
    }
}
