using System;

namespace Cummulative_Assign.Controllers
{
    internal class EnableCorsAttribute : Attribute
    {
        private string origins;
        private string methods;
        private string headers;

        public EnableCorsAttribute(string origins, string methods, string headers)
        {
            this.origins = origins;
            this.methods = methods;
            this.headers = headers;
        }
    }
}