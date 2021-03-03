using MercadoPagoApi.Data.Collections;
using MongoDB.Driver;

namespace MercadoPagoApi.Data.Business
{
    public class SequenciasBusiness
    {
        private readonly MongoDb _mongoDb;
        IMongoCollection<Sequencias> _sequenciasCollection;
        public int Padrao { get; set; }

        public SequenciasBusiness(MongoDb dbContext)
        {
            _mongoDb = dbContext;
            _sequenciasCollection = _mongoDb.MongoDatabaseContext.GetCollection<Sequencias>(typeof(Sequencias).Name.ToLower());
            Padrao = 0;
        }

        public int ProximoValor(string pCollectionName)
        {

            try
            {
                Padrao =  _sequenciasCollection.FindOneAndUpdate(
                        Builders<Sequencias>.Filter.Eq(seq => seq.Nome, pCollectionName),
                        Builders<Sequencias>.Update.Inc(seq => seq.Atual, 1)
                    ).Atual + Padrao;
            }
            catch
            {
                Padrao = Padrao+2;
                _sequenciasCollection.InsertOne(new Sequencias(Padrao, pCollectionName));
                Padrao--;
            }

            return Padrao;
        }
    }
}
