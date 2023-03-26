global using System.Text;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.JsonPatch;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.Identity.Web;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.Options;

global using SparkPlug.Api.Controllers;
global using SparkPlug.Contracts;

global using SparkPlug.Persistence.Abstractions;
global using SparkPlug.Persistence.MongoDb.Models;
global using SparkPlug.Persistence.MongoDb.Attributes;
global using SparkPlug.Persistence.MongoDb.Configuration;

global using SparkPlug.Sample.WebApi.Models;
global using SparkPlug.Sample.WebApi.Repositories;
