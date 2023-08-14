global using System.Text;
global using System.Reflection;

global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Identity.Web;
global using Npgsql;

global using SparkPlug.Persistence.Abstractions;
global using SparkPlug.Persistence.EntityFramework.Configuration;

global using SparkPlug.Business.Tenancy.Api;
global using SparkPlug.Business.Tenancy.Repository.Sql;
global using SparkPlug.Business.Menu.Api;
global using SparkPlug.Business.Menu.Repository.Sql;
