using SimpleBus.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SimpleBus.Core.Test
{
    public class GenericClass<T> { }
    public class TestClass : GenericClass<string> { }

    public class TypeHelperTest
    {
        [Fact]
        public void GetTypeTest()
        {
            Assert.NotNull(TypeHelper.GetType("TypeHelperTest"));
        }


        [Fact]
        public void IsTypeDerivedFromGenericTypeTest()
        {
            Assert.True(TypeHelper.IsTypeDerivedFromGenericType(typeof(TestClass), typeof(GenericClass<>)));
            Assert.False(TypeHelper.IsTypeDerivedFromGenericType(typeof(string), typeof(GenericClass<>)));
        }
    }
}
