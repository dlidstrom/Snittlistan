namespace Snittlistan.Web.Infrastructure.AutoMapper
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public static class AutoMapperExtensions
    {
        public static List<TResult> MapTo<TResult>(this IEnumerable self)
        {
            if (self == null)
                throw new ArgumentNullException("self");

            return (List<TResult>)global::AutoMapper.Mapper.Map(self, self.GetType(), typeof(List<TResult>));
        }

        public static TResult MapTo<TResult>(this object self)
        {
            if (self == null)
                throw new ArgumentNullException("self");

            return (TResult)global::AutoMapper.Mapper.Map(self, self.GetType(), typeof(TResult));
        }
    }
}