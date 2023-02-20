using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDK.QueryAnalyser.Migrations
{
    [Migration(1)]
    public class CreateStoreQueryTable : Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            var table = Create.Table("STOREQUERY");

            table.WithColumn("Id").
                AsInt64().
                Identity();

            table.WithColumn("QueryMD5")
                 .AsString(int.MaxValue);

            table.WithColumn("TriggeredBy")
                 .AsString(int.MaxValue);

            table.WithColumn("FullQuery")
                 .AsString(int.MaxValue);
            
            table.WithColumn("BaseQuery")
                 .AsString(int.MaxValue);

            table.WithColumn("RequestJson")
                 .AsString(int.MaxValue);

            table.WithColumn("RunnedAt")
                 .AsDateTime();

            table.WithColumn("RunnedUntil")
                 .AsDateTime();


        }
    }
}
