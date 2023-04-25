using System;
using System.Collections.Generic;
using System.Text;

namespace Trash.Infra.Context.Repositories.Common
{
    public class RepositoryResult
    {
        public object Object { get; set; }
        public StatusResult Result_Code { get; set; }
    }

    public enum StatusResult
    {
        Success = 1,
        BadArgument = 2,
        NullArgument = 3,
        NotFound = 4,
        DataBaseLost = 5
    }
}
