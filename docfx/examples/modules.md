
Module Dependency
=============================================

SparkPlug.Business.<Module>.Domain
    SparkPlug.Persistence.Abstractions
    
SparkPlug.Business.<Module>.Repository.Sql
    SparkPlug.Persistence.Abstractions
    SparkPlug.Persistence.EntityFramework
    SparkPlug.<Module>.Domain

SparkPlug.Business.<Module>.Repository.Mongo
    SparkPlug.Persistence.Abstractions
    SparkPlug.Persistence.MongoDb
    SparkPlug.Business.<Module>.Domain

SparkPlug.Business.<Module>.Service
    SparkPlug.Contracts
    SparkPlug.Persistence.Abstractions
    SparkPlug.Business.<Module>.Domain

SparkPlug.Business.<Module>.Api
    SparkPlug.Contracts
    SparkPlug.Api
    SparkPlug.Business.<Module>.Service


Application
===============================================
SparkPlug.<ApplicationName>.Host
    SparkPlug.Business.<Module-1>.Api
    SparkPlug.Business.<Module-2>.Api
    SparkPlug.Business.<Module-2>.Api