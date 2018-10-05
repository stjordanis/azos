
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azos.Data.Modeling
{

    /// <summary>
    /// Compiles relation schema into Ms SQL Server scripts
    /// </summary>
    public class MsSQLServerCompiler : RDBMSCompiler
    {
        #region .ctor

            public MsSQLServerCompiler(Schema schema) : base(schema)
            {

            }


        #endregion

        #region Properties
            public override TargetType Target
            {
                get { return TargetType.MsSQLServer; }
            }

            public override string Name
            {
                get { return "MsSQL"; }
            }
        #endregion


        #region Protected

            public override string GetQuotedIdentifierName(RDBMSEntityType type, string name)
            {
                if (type!=RDBMSEntityType.Domain)
                    return "[{0}]".Args(name);
                else
                    return name;
            }

            public override string GetStatementDelimiterScript(RDBMSEntityType type, bool start)
            {
                return start ? string.Empty : "\nGO\n";
            }

        #endregion

    }
}
