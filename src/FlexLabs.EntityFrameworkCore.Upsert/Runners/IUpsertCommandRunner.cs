﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FlexLabs.EntityFrameworkCore.Upsert.Runners
{
    /// <summary>
    /// Upsert command runner base interface
    /// </summary>
    public interface IUpsertCommandRunner
    {
        /// <summary>
        /// Specifies whether this command runner supports a specific database provider
        /// </summary>
        /// <param name="providerName">Name of the database provider</param>
        /// <returns>true if this runner supports the database provider specified; otherwise false</returns>
        bool Supports(string providerName);

        /// <summary>
        /// Run the upsert command for the entities passed
        /// </summary>
        /// <typeparam name="TEntity">Entity type of the entities</typeparam>
        /// <param name="dbContext">Data context to be used</param>
        /// <param name="entityType">Metadata for the entity</param>
        /// <param name="entities">Array of entities to be upserted</param>
        /// <param name="matchExpression">Expression that represents which properties will be used as a match clause for the upsert command</param>
        /// <param name="updateExpression">Expression that represents which properties will be updated, and what values will be set</param>
        /// <param name="updateCondition">Expression that checks whether the database entry should be updated</param>
        /// <param name="queryOptions">Options for the current query that will affect it's behaviour</param>
        int Run<TEntity>(DbContext dbContext, IEntityType entityType, ICollection<TEntity> entities, Expression<Func<TEntity, object>>? matchExpression,
            Expression<Func<TEntity, TEntity, TEntity>>? updateExpression, Expression<Func<TEntity, TEntity, bool>>? updateCondition, RunnerQueryOptions queryOptions)
            where TEntity : class;

        /// <summary>
        /// Run the upsert command for the entities passed
        /// </summary>
        /// <typeparam name="TEntity">Entity type of the entities</typeparam>
        /// <param name="dbContext">Data context to be used</param>
        /// <param name="entityType">Metadata for the entity</param>
        /// <param name="entities">Array of entities to be upserted</param>
        /// <param name="matchExpression">Expression that represents which properties will be used as a match clause for the upsert command</param>
        /// <param name="updateExpression">Expression that represents which properties will be updated, and what values will be set</param>
        /// <param name="updateCondition">Expression that checks whether the database entry should be updated</param>
        /// <param name="queryOptions">Options for the current query that will affect it's behaviour</param>
        /// <param name="cancellationToken">The CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>The task that represents the asynchronous upsert operation</returns>
        Task<int> RunAsync<TEntity>(DbContext dbContext, IEntityType entityType, ICollection<TEntity> entities, Expression<Func<TEntity, object>>? matchExpression,
            Expression<Func<TEntity, TEntity, TEntity>>? updateExpression, Expression<Func<TEntity, TEntity, bool>>? updateCondition, RunnerQueryOptions queryOptions,
            CancellationToken cancellationToken) where TEntity : class;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="entityType"></param>
        /// <param name="entities"></param>
        /// <param name="matchExpression"></param>
        /// <param name="updateExpression"></param>
        /// <param name="updateCondition"></param>
        /// <param name="queryOptions"></param>
        /// <param name="dbTransaction"></param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        Task<int> RunWithTransactionAsync<TEntity>(DbContext dbContext, IEntityType entityType, ICollection<TEntity> entities, Expression<Func<TEntity, object>>? matchExpression,
            Expression<Func<TEntity, TEntity, TEntity>>? updateExpression, Expression<Func<TEntity, TEntity, bool>>? updateCondition, RunnerQueryOptions queryOptions, DbTransaction? dbTransaction,
            CancellationToken cancellationToken) where TEntity : class;
    }
}
