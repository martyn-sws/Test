using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor_MSPath.Data
{
    public class TodoItem
    {
        public string? Title {get;set;}
        public bool IsDone {get;set;} = false;
    }
}