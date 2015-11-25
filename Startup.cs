using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using ThrowdownAttire.Models;

[assembly: OwinStartup(typeof(ThrowdownAttire.Startup))]

namespace ThrowdownAttire
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
