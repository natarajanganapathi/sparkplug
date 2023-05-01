
Module Dependency
=============================================

SparkPlug.<Module>.Domain
    SparkPlug.Persistence.Abstractions
    
SparkPlug.<Module>.Repository.Sql
    SparkPlug.Persistence.Abstractions
    SparkPlug.Persistence.EntityFramework
    SparkPlug.<Module>.Domain

SparkPlug.<Module>.Repository.Mongo
    SparkPlug.Persistence.Abstractions
    SparkPlug.Persistence.MongoDb
    SparkPlug.<Module>.Domain

SparkPlug.<Module>.Service
    SparkPlug.Contracts
    SparkPlug.Persistence
    SparkPlug.<Module>.Domain

SparkPlug.<Module>.Api
    SparkPlug.Contracts
    SparkPlug.Api
    SparkPlug.<Module>.Service


Application
===============================================
SparkPlug.<ApplicationName>.Host
    SparkPlug.<Module-1>.Api
    SparkPlug.<Module-2>.Api
    SparkPlug.<Module-2>.Api