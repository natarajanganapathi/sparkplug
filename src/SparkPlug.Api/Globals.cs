global using System;
global using System.ComponentModel.DataAnnotations;
global using System.Linq;
global using System.Reflection;
global using System.Text;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using System.Threading.Tasks;
global using System.Transactions;

global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Diagnostics.HealthChecks;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.JsonPatch;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.ApplicationModels;
global using Microsoft.AspNetCore.Mvc.Filters;
global using Microsoft.AspNetCore.Mvc.Infrastructure;
global using Microsoft.AspNetCore.Mvc.Routing;
global using Microsoft.AspNetCore.Routing;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;
global using Microsoft.OpenApi.Models;

global using Newtonsoft.Json;
global using Newtonsoft.Json.Linq;
global using Newtonsoft.Json.Serialization;

global using SparkPlug.Api.Configuration;
global using SparkPlug.Api.Controllers;
global using SparkPlug.Api.Filters;
global using SparkPlug.Api.Middleware;
global using SparkPlug.Contracts;
global using SparkPlug.Persistence.Abstractions;
