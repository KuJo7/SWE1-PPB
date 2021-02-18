using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;

namespace PPB
{
    public class RequestContext
    {
        public string Httpverb { get; } //get Httpverb
        public string Resource { get; } //get Resource
        public string Httpversion { get; } //get Httpversion
        public Dictionary<string, string> HeaderDict { get; } //get Header
        public string Authorization { get; } //get Authorization;
        public string Body { get; } //get Body

        public RequestContext(string httpverb, string resource, string httpversion, Dictionary<string, string> headerdict, string authorization, string body) // Constructor with parameters
        {
            Httpverb = httpverb;
            Resource = resource;
            Httpversion = httpversion;
            HeaderDict = headerdict;
            Authorization = authorization;
            Body = body;
        }
    }
}

// Joel KUDIYIRICKAL