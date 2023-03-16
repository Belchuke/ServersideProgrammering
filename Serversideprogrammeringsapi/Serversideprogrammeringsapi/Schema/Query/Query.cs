using HotChocolate.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Serversideprogrammeringsapi.Database;
using Serversideprogrammeringsapi.ExtensionMethods;
using Serversideprogrammeringsapi.Models;
using Serversideprogrammeringsapi.Schema.Mutations;
using Serversideprogrammeringsapi.Types;
using System.Security.Claims;

namespace Serversideprogrammeringsapi.Schema.Query
{
    public class Query
    {
        public TestObject RequiredQuery()
        {
            return new TestObject() { Name = "World", Child = new TestObjectChild() { ChildName = " hello" } };
        }
    }
}
