using System;
using System.Collections.Generic;
using Raven.Imports.Newtonsoft.Json;

namespace Snittlistan.Web.Areas.V2.Domain
{
    public class MatchSerie
    {
        [JsonProperty(PropertyName = "Tables")]
        private readonly List<MatchTable> tables = new List<MatchTable>();

        public MatchSerie(
            MatchTable table1,
            MatchTable table2,
            MatchTable table3,
            MatchTable table4)
        {
            if (table1 == null) throw new ArgumentNullException("table1");
            if (table2 == null) throw new ArgumentNullException("table2");
            if (table3 == null) throw new ArgumentNullException("table3");
            if (table4 == null) throw new ArgumentNullException("table4");
            this.tables = new List<MatchTable>
                        {
                            table1,
                            table2,
                            table3,
                            table4
                        };
        }

        public IEnumerable<MatchTable> Tables
        {
            get
            {
                return this.tables;
            }
        }
    }
}