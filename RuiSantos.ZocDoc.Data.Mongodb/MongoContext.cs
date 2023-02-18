﻿using MongoDB.Driver;
using RuiSantos.ZocDoc.Core.Data;
using RuiSantos.ZocDoc.Data.Mongodb.Core;
using System.Linq.Expressions;

namespace RuiSantos.ZocDoc.Data.Mongodb;

public class MongoContext: IDataContext
{
    private readonly IMongoDatabase database;

    public MongoContext(MongoSettings settings)
    {
        var client = new MongoClient(settings.ToMongoClientSettings());
        this.database = client.GetDatabase(settings.Schema);

        Mediator.RegisterClassMaps();
    }

    public async Task StoreAsync<TModel>(TModel model)
    {
        if (Mediator.GetEntity<TModel>() is IEntity<TModel> entity)
            await entity.StoreAsync(database, model);
    }

    public async Task RemoveAsync<TModel>(Guid id)
    {
        if (Mediator.GetEntity<TModel>() is IEntity<TModel> entity)
            await entity.RemoveAsync(database, id);
    }

    public async Task<bool> ExistsAsync<TModel>(Expression<Func<TModel, bool>> expression)
    {
        if (Mediator.GetEntity<TModel>() is IEntity<TModel> entity)
            return await entity.AnyAsync(database, expression);

        return false;
    }

    public async Task<List<TModel>> ListAsync<TModel>()
    {
        if (Mediator.GetEntity<TModel>() is IEntity<TModel> entity)
            return await entity.ListAsync(database);

        return new List<TModel>();
    }

    public async Task<List<TModel>> QueryAsync<TModel>(Expression<Func<TModel, bool>> expression)
    {
        if (Mediator.GetEntity<TModel>() is IEntity<TModel> entity)
            return await entity.QueryAsync(database, expression);

        return new List<TModel>();
    }

    public async Task<TModel?> FindAsync<TModel>(Guid id)
    {
        if (Mediator.GetEntity<TModel>() is IEntity<TModel> entity)
            return await entity.FindAsync(database, id);

        return default;
    }

    public async Task<TModel?> FindAsync<TModel>(Expression<Func<TModel, bool>> expression)
    {
        if (Mediator.GetEntity<TModel>() is IEntity<TModel> entity)
            return await entity.FindAsync(database, expression);

        return default;
    }
}