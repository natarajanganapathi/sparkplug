global using System.Text;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using System.ComponentModel.DataAnnotations;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.JsonPatch;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.Identity.Web;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Configuration;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.Extensions.Options;
global using Npgsql;
global using Newtonsoft.Json;

global using SparkPlug.Api.Controllers;
global using SparkPlug.Contracts;

global using SparkPlug.Persistence.Abstractions;
global using SparkPlug.Persistence.EntityFramework;
global using SparkPlug.Persistence.EntityFramework.Configuration;
global using SparkPlug.Persistence.EntityFramework.Context;
global using SparkPlug.Persistence.EntityFramework.Models;

global using SparkPlug.Sample.WebApi.Models;
