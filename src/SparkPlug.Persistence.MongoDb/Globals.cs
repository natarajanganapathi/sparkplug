global using System;
global using System.Linq;
global using System.Reflection;
global using System.Collections.Concurrent;
global using System.Security.Authentication;
global using System.Collections.Generic;
global using System.Threading.Tasks;
global using System.Threading;
global using System.ComponentModel.DataAnnotations;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Diagnostics.HealthChecks;
global using Microsoft.AspNetCore.JsonPatch;
global using Microsoft.Extensions.Options;
global using Newtonsoft.Json.Linq;

global using MongoDB.Driver;
global using MongoDB.Bson;
global using MongoDB.Bson.Serialization.Attributes;

global using SparkPlug.Contracts;
global using SparkPlug.Persistence.Abstractions;

global using SparkPlug.Persistence.MongoDb;
global using SparkPlug.Persistence.MongoDb.Attributes;
global using SparkPlug.Persistence.MongoDb.Context;
global using SparkPlug.Persistence.MongoDb.HealthCheck;
global using SparkPlug.Persistence.MongoDb.Configuration;
