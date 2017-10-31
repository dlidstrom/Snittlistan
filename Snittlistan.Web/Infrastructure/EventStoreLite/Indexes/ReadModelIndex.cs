using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Raven.Client.Indexes;

// ReSharper disable once CheckNamespace
namespace EventStoreLite.Indexes
{
    public class ReadModelIndex : AbstractMultiMapIndexCreationTask<IReadModel>
    {
        public ReadModelIndex(IEnumerable<Type> types)
        {
            if (types == null) throw new ArgumentNullException(nameof(types));
            AddMapForAllSub<IReadModel>(types, views => from view in views select new { view.Id });
        }

        private void AddMapForAllSub<TBase>(IEnumerable<Type> types,
            Expression<Func<IEnumerable<TBase>, IEnumerable>> expr)
        {
            // Index the base class.
            AddMap(expr);

            // Index child classes.
            var children = types.Where(x => typeof(TBase).IsAssignableFrom(x)).ToList();
            var addMapGeneric = GetType().GetMethod("AddMap", BindingFlags.Instance | BindingFlags.NonPublic);
            Debug.Assert(addMapGeneric != null, "addMapGeneric != null");

            foreach (var child in children)
            {
                var genericEnumerable = typeof(IEnumerable<>).MakeGenericType(child);
                var delegateType = typeof(Func<,>).MakeGenericType(genericEnumerable, typeof(IEnumerable));
                var lambdaExpression = Expression.Lambda(delegateType, expr.Body, Expression.Parameter(genericEnumerable, expr.Parameters[0].Name));
                addMapGeneric.MakeGenericMethod(child).Invoke(this, new object[] { lambdaExpression });
            }
        }
    }
}