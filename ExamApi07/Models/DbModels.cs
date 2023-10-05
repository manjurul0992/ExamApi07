using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Data.Entity;
using System.Net.Http;

namespace ExamApi07.Models
{
    public class Device
    {
        public int DeviceId { get; set; }
        [Required, StringLength(50)]
        public string DeviceName { get; set; }
        [Required, Column(TypeName = "date")]
        public DateTime ReleaseDate { get; set; }
        [Required, Column(TypeName = "money")]

        public decimal Price { get; set; }
        public bool OnSale { get; set; }
        [Required, StringLength(200)]

        public string Picture { get; set; }
        public ICollection<Spec> Specs { get; set; } = new List<Spec>();
    }
    public class Spec
    {
        public int SpecId { get; set; }
        public string SpecName { get; set; }
        public string Value { get; set; }
        [ForeignKey("Device")]
        public int DeviceId { get; set; }
        public Device Device { get; set; }
    }
    public class DeviceDbContext : DbContext
    {
        public DbSet<Device> Devices { get; set; }
        public DbSet<Spec> Specs { get; set; }
    }
    public class DeviceInputModel
    {
        public int DeviceId { get; set; }
        [Required, StringLength(50)]
        public string DeviceName { get; set; }
        [Required, Column(TypeName = "date")]
        public DateTime ReleaseDate { get; set; } = DateTime.Now;
        [Required, Column(TypeName = "money")]

        public decimal Price { get; set; }
        public bool OnSale { get; set; }
        [Required, StringLength(200)]

        public string Picture { get; set; }
        public List<SpecInputModel> Specs { get; set; } = new List<SpecInputModel>();


    }

    public class SpecInputModel
    {

        public string SpecName { get; set; }
        public string Value { get; set; }

    }
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var identity = DecodeAuthHeader(actionContext);
            if (identity == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized, "Unauthorized request!!!");
            }
            else
            {
                if (OnAuthorizeUser(identity.Name, identity.Password, actionContext))
                {
                    var principal = new GenericPrincipal(identity, null);
                    Thread.CurrentPrincipal = principal;
                }
                else
                {
                    actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized, "Unauthorized request!!!");
                }
            }

        }
        protected virtual bool OnAuthorizeUser(string username, string password, HttpActionContext actionContext)
        {
            if (username == "manjur" && password == "manjur123")
                return true;
            return false;
        }
        protected virtual AuthIdentity DecodeAuthHeader(HttpActionContext actionContext)
        {
            string authHeader = null;
            var auth = actionContext.Request.Headers.Authorization;
            if (auth != null && auth.Scheme == "Basic")
                authHeader = auth.Parameter;

            if (string.IsNullOrEmpty(authHeader))
                return null;

            authHeader = Encoding.Default.GetString(Convert.FromBase64String(authHeader));
            var tokens = authHeader.Split(':');
            if (tokens.Length < 2)
                return null;
            return new AuthIdentity(tokens[0], tokens[1]);
        }
        public class AuthIdentity : GenericIdentity
        {
            public string Password { get; set; }
            public AuthIdentity(string name, string password) : base(name, "Basic")
            {
                this.Password = password;
            }
        }

    }
}