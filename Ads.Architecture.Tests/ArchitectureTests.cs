using NetArchTest.Rules;

namespace Ads.Architecture.Tests;

public class ArchitectureTests
    {
        private const string DomainNamespace = "Ads.Domain";
        private const string ApplicationNamespace = "Ads.Application";
        private const string PersistenceNamespace = "Ads.Persistence";
        private const string InfrastructureNamespace = "Ads.WebApi";

        [Fact]
        public void Domain_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange
            var assembly = typeof(Domain.AssemblyReference).Assembly;

            var otherProjects = new[]
            {
                ApplicationNamespace,
                PersistenceNamespace,
                InfrastructureNamespace
            };

            // Act
            var testResult = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAll(otherProjects)
                .GetResult();
            
            // Assert
            Assert.True(testResult.IsSuccessful);
        }
        
        [Fact]
        public void Application_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange
            var assembly = typeof(Application.AssemblyReference).Assembly;

            var otherProjects = new[]
            {
                PersistenceNamespace,
                InfrastructureNamespace
            };

            // Act
            var testResult = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAll(otherProjects)
                .GetResult();
            
            // Assert
            Assert.True(testResult.IsSuccessful);
        }
        
        [Fact]
        public void Persistence_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange
            var assembly = typeof(Persistence.AssemblyReference).Assembly;

            var otherProjects = new[]
            {
                InfrastructureNamespace
            };

            // Act
            var testResult = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAll(otherProjects)
                .GetResult();
            
            // Assert
            Assert.True(testResult.IsSuccessful);
        }
        
        [Fact]
        public void Handlers_Should_Have_DependencyOnDomain()
        {
            // Arrange
            var assembly = typeof(Ads.Application.AssemblyReference).Assembly;

            // Act
            var testResult = Types
                .InAssembly(assembly)
                .That()
                .HaveNameEndingWith("Handler")
                .Should()
                .HaveDependencyOn(DomainNamespace)
                .GetResult();
            
            // Assert
            Assert.True(testResult.IsSuccessful);
        }
        
        [Fact]
        public void Controllers_Should_Have_DependencyOnMediatR()
        {
            // Arrange
            var assembly = typeof(Ads.WebApi.AssemblyReference).Assembly;

            // Act
            var testResult = Types
                .InAssembly(assembly)
                .That()
                .HaveNameEndingWith("Controller")
                .Should()
                .HaveDependencyOn("MediatR")
                .GetResult();
            
            // Assert
            Assert.True(testResult.IsSuccessful);
        }
    }