using System.IO;
using Newtonsoft.Json.Linq;
using TareEditor.Services;

namespace TareEditor.Tests
{
    public class ProjectIoServiceTests
    {
        private static string SourcePath => Path.Combine(AppContext.BaseDirectory, "TestData", "thedata.json");

        [Fact]
        public void ImportThenExport_RoundTripsGameDataUnchanged()
        {
            var project = ProjectIoService.ImportEngineJson(SourcePath);

            var exportPath = Path.GetTempFileName();
            try
            {
                ProjectIoService.ExportEngineJson(project, exportPath);

                var original = JObject.Parse(File.ReadAllText(SourcePath));
                var exported = JObject.Parse(File.ReadAllText(exportPath));

                Assert.True(JToken.DeepEquals(original, exported),
                    $"Exported JSON differs from source.\n\nOriginal:\n{original}\n\nExported:\n{exported}");
            }
            finally
            {
                File.Delete(exportPath);
            }
        }

        [Fact]
        public void Import_SynthesizesLayoutForEveryRoom()
        {
            var project = ProjectIoService.ImportEngineJson(SourcePath);

            var roomSlugs = project.GameData.rooms.rooms.Select(r => r.slug).ToHashSet();
            var layoutSlugs = project.Layout.Select(l => l.Slug).ToHashSet();

            Assert.Equal(roomSlugs, layoutSlugs);
        }

        [Fact]
        public void SaveThenLoadProject_RoundTripsLayoutAndGameData()
        {
            var project = ProjectIoService.ImportEngineJson(SourcePath);
            project.Layout[0].X = 42;
            project.Layout[0].Y = 99;

            var projectPath = Path.GetTempFileName();
            try
            {
                ProjectIoService.SaveProject(project, projectPath);
                var reloaded = ProjectIoService.LoadProject(projectPath);

                Assert.Equal(project.Layout[0].X, reloaded.Layout[0].X);
                Assert.Equal(project.Layout[0].Y, reloaded.Layout[0].Y);
                Assert.Equal(project.GameData.rooms.rooms.Length, reloaded.GameData.rooms.rooms.Length);
            }
            finally
            {
                File.Delete(projectPath);
            }
        }
    }
}
