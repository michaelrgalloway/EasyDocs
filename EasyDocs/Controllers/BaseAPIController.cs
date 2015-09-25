using EasyDocs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Web.Http;

namespace EasyDocs.Controllers
{
    public class BaseAPIController : ApiController
    {
        protected DocEasyContext db = new DocEasyContext();

        protected ObjectCache cache = MemoryCache.Default;

        protected T GetFromCache<T>(string key, params string[] subkeys) where T:class
        {
            return cache.Get(transformKey(key, subkeys)) as T;
        }

        protected void AddToCache<T>(string key, T obj, params string[] subkeys) where T :class
        {

            CacheItemPolicy policy = new CacheItemPolicy { AbsoluteExpiration = MemoryCache.InfiniteAbsoluteExpiration };
            cache.Add(transformKey(key, subkeys), obj, policy);
        }

        protected void RemoveFromCache(string key, params string[] subkeys)
        {
            cache.Remove(transformKey(key, subkeys));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private string transformKey(string key, string[] subkeys)
        {
            if(subkeys!=null)
            foreach(var s in subkeys)
            {
                key += "_" + s;
            }

            return key;
        }
    }
}
