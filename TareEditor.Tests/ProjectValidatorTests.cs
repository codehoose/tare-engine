using System.IO;
using TareEditor.Services;

namespace TareEditor.Tests
{
    public class ProjectValidatorTests
    {
        [Fact]
        public void RealGameData_HasNoErrorSeverityIssues()
        {
            var sourcePath = Path.Combine(AppContext.BaseDirectory, "TestData", "thedata.json");
            var project = ProjectIoService.ImportEngineJson(sourcePath);

            var issues = ProjectValidator.Validate(project.GameData.rooms.rooms);

            Assert.DoesNotContain(issues, i => i.Severity == ValidationSeverity.Error);
        }
    }
}
