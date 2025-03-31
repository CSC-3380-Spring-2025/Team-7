using GameDataApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace GameDataApi.Service {
    public class MongoDbService {
        private readonly IMongoCollection<PlayerData> _playerCollection;

        public MongoDbService(IOptions<DatabaseSettings> dbSettings){
            var settings = dbSettings.Value;
            var mongoClient = new MongoClient(settings.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(settings.DatabaseName);
            _playerCollection = mongoDatabase.GetCollection<PlayerData>(settings.PlayersCollectionName);
        }

        public async Task<PlayerData?> GetAsync(string playerId) =>
            await _playerCollection.Find(p => p.PlayerId == playerId).FirstOrDefaultAsync();

        public async Task SaveAsync(PlayerData playerData){
            playerData.LastUpdated = DateTime.UtcNow;
            var filter = Builders<PlayerData>.Filter.Eq(playerData => playerData.PlayerId, playerData.PlayerId);
            var options = new ReplaceOptions { IsUpsert = true };
            await _playerCollection.ReplaceOneAsync(filter, playerData, options);
        }
    } 
} 