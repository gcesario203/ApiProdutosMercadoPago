﻿using System;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Conventions;
using MercadoPagoApi.Data.Collections;
using MercadoPagoApi.General;

namespace MercadoPagoApi.Data
{
    public class MongoDb
    {
        public IMongoDatabase MongoDatabaseContext { get;}

        public MongoDb(IConfiguration config)
        {
            try 
            {
                var lMongoUrl = new MongoUrl(config["ConnectionString"]);

                var configuracoes = MongoClientSettings.FromUrl(lMongoUrl);
                var client = new MongoClient(configuracoes);
                MongoDatabaseContext = client.GetDatabase(config["DatabaseName"]);
                MapClasses();
            }
            catch(Exception except)
            {
                throw new MongoException("Não foi possível conectar ao mongo", except);
            }
        }

        private void MapClasses()
        {
            var convecoes = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("camelCase", convecoes, t => true);

            Utils.JsonMapClasses<Produtos>();
            Utils.JsonMapClasses<Sequencias>();
            Utils.JsonMapClasses<Usuarios>();
            Utils.JsonMapClasses<Enderecos>();
            Utils.JsonMapClasses<Documentos>();
            Utils.JsonMapClasses<Telefones>();
        }
    }
}
