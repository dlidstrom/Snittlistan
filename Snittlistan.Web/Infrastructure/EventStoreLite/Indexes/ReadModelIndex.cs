#nullable enable

namespace EventStoreLite.Indexes
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Raven.Client.Indexes;

    public class ReadModelIndex : AbstractMultiMapIndexCreationTask<IReadModel>
    {
        public ReadModelIndex(IEnumerable<Type> types)
        {
            if (types == null)
            {
                throw new ArgumentNullException(nameof(types));
            }

            AddMapForAllSub<IReadModel>(types, views => from view in views select new { view.Id });
        }

        private void AddMapForAllSub<TBase>(IEnumerable<Type> types,
            Expression<Func<IEnumerable<TBase>, IEnumerable>> expr)
        {
            // Index the base class.
            AddMap(expr);

            // Index child classes.
            List<Type> children = types.Where(x => typeof(TBase).IsAssignableFrom(x)).ToList();
            MethodInfo addMapGeneric = GetType().GetMethod("AddMap", BindingFlags.Instance | BindingFlags.NonPublic);
            Debug.Assert(addMapGeneric != null, "addMapGeneric != null");

            foreach (Type child in children)
            {
                Type genericEnumerable = typeof(IEnumerable<>).MakeGenericType(child);
                Type delegateType = typeof(Func<,>).MakeGenericType(genericEnumerable, typeof(IEnumerable));
                LambdaExpression lambdaExpression = Expression.Lambda(delegateType, expr.Body, Expression.Parameter(genericEnumerable, expr.Parameters[0].Name));
                _ = addMapGeneric!.MakeGenericMethod(child).Invoke(this, new object[] { lambdaExpression });
            }
        }
    }
}
