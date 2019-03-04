using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace UtilitiesExtensionsTest
{
    public class AssemblyExtensionsTest
    {
        private System.Reflection.Assembly _assembly;
        private readonly ITestOutputHelper _output;
        public AssemblyExtensionsTest(ITestOutputHelper output)
        {
            _output = output;
            _assembly = typeof(AssemblyExtensionsTest).Assembly;
        }

        [Fact]
        public void GetFileVersionInfo()
        {
            var version = _assembly.GetFileVersionInfo();
            Assert.Equal("UtilitiesExtensions.Tests", version.ProductName);
            Assert.Equal("UtilitiesExtensions.Tests", version.CompanyName);
        }
        [Fact]
        public void GetFileVersion()
        {
            var version = _assembly.GetFileVersion();
            Assert.Equal(new Version(1, 0, 0, 0), version);
        }
        [Fact]
        public void GetProductVersion()
        {
            var version = _assembly.GetProductVersion();
            Assert.Equal(new Version(1, 0, 0), version);
        }
        [Fact]
        public void GetNameSafe()
        {
            var name = _assembly.GetNameSafe();
            Assert.Equal("UtilitiesExtensions.Tests", name.Name);
        }
    }
}
