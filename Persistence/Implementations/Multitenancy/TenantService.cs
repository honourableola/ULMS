using Domain.Multitenancy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Implementations.Multitenancy
{
    public class TenantService : ITenantService
    {
        private readonly TenantSetting _tenantSetting;
        private readonly HttpContext _httpContext;
        private Tenant _currentTenant;

        public TenantService(IOptions<TenantSetting> tenantSetting, IHttpContextAccessor contetxAccessor)
        {
            _tenantSetting = tenantSetting.Value;
            _httpContext = contetxAccessor.HttpContext;

            if (_httpContext != null)
            {
                if (_httpContext.Request.Headers.TryGetValue("tenant", out var tenantId))
                {
                    SetTenant(tenantId);
                }
                else
                {
                    throw new Exception("Invalid Tenant!");
                }
            }
        }

        private void SetTenant(string tenantId)
        {
            _currentTenant = _tenantSetting.Tenants.Where(a => a.TID == tenantId).FirstOrDefault();
            if (_currentTenant == null)
            {
                throw new Exception("Invalid tenant");
            }

            if (string.IsNullOrEmpty(_currentTenant.ConnectionString))
            {
                SetDefaultConnectionStringToCurrentTenant();
            }
        }

        private void SetDefaultConnectionStringToCurrentTenant()
        {
            _currentTenant.ConnectionString = _tenantSetting.Defaults.ConnectionString;
        }
        public string GetConnectionString()
        {
            return _currentTenant?.ConnectionString;
        }

        public string GetDatabaseProvider()
        {
            return _tenantSetting.Defaults?.DBProvider;
        }

        public Tenant GetTenant()
        {
            return _currentTenant;
        }
    }
}
