using System;
using System.Collections.Generic;

namespace Snittlistan.Web.Areas.V2.Domain
{
    public class MatchSerie
    {
        private readonly MatchTable[] tables;

        public MatchSerie(List<MatchTable> tables)
        {
            if (tables == null) throw new ArgumentNullException("tables");
            if (tables.Count != 4) throw new ArgumentException("tables");
            this.tables = tables.ToArray();
        }

        public IEnumerable<MatchTable> Tables
        {
            get
            {
                return Array.AsReadOnly(tables);
            }
        }
    }
}