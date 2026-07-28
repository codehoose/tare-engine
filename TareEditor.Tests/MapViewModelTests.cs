using System.IO;
using TareEditor.Models;
using TareEditor.Services;
using TareEditor.ViewModels;

namespace TareEditor.Tests
{
    public class MapViewModelTests
    {
        private static string SourcePath => Path.Combine(AppContext.BaseDirectory, "TestData", "thedata.json");

        [Fact]
        public void FromProject_WiresExitTargetsAndStartRoom()
        {
            var project = ProjectIoService.ImportEngineJson(SourcePath);
            var map = MapViewModel.FromProject(project);

            var tardis = map.Rooms.Single(r => r.Slug == "tardis");
            var outsideTardis = map.Rooms.Single(r => r.Slug == "outside-tardis");

            Assert.Same(outsideTardis, tardis.GetExit(ExitDirection.South).Target);
            Assert.Same(tardis, outsideTardis.GetExit(ExitDirection.North).Target);
            Assert.Equal("tardis-key-held", outsideTardis.GetExit(ExitDirection.North).BlockedByFlag);
            Assert.Same(outsideTardis, map.StartRoom);
        }

        [Fact]
        public void ToProject_AfterNoEdits_RoundTripsGameDataUnchanged()
        {
            var project = ProjectIoService.ImportEngineJson(SourcePath);
            var map = MapViewModel.FromProject(project);

            var roundTripped = map.ToProject();

            var exportPath = Path.GetTempFileName();
            try
            {
                ProjectIoService.ExportEngineJson(roundTripped, exportPath);

                var original = Newtonsoft.Json.Linq.JObject.Parse(File.ReadAllText(SourcePath));
                var exported = Newtonsoft.Json.Linq.JObject.Parse(File.ReadAllText(exportPath));

                Assert.True(Newtonsoft.Json.Linq.JToken.DeepEquals(original, exported),
                    $"Round-tripping through the ViewModel layer with no edits changed the data.\n\nOriginal:\n{original}\n\nExported:\n{exported}");
            }
            finally
            {
                File.Delete(exportPath);
            }
        }

        [Fact]
        public void ConnectExit_ThenSync_WritesExitIntoUnderlyingRoom()
        {
            var project = ProjectIoService.ImportEngineJson(SourcePath);
            var map = MapViewModel.FromProject(project);

            var tardis = map.Rooms.Single(r => r.Slug == "tardis");
            var hallWay = map.Rooms.Single(r => r.Slug == "hall-way");

            tardis.GetExit(ExitDirection.Up).Target = hallWay;
            tardis.SyncExitsToModel();

            Assert.Equal("hall-way", tardis.Room.exits["up"]);
        }

        [Fact]
        public void AddRoom_ThenRemoveRoom_SeversExitsPointingAtIt()
        {
            var map = MapViewModel.FromProject(ProjectIoService.ImportEngineJson(SourcePath));
            var tardis = map.Rooms.Single(r => r.Slug == "tardis");

            map.AddRoomCommand.Execute(null);
            var newRoom = map.Rooms.Last();
            tardis.GetExit(ExitDirection.Up).Target = newRoom;

            map.RemoveRoomCommand.Execute(newRoom);

            Assert.DoesNotContain(newRoom, map.Rooms);
            Assert.Null(tardis.GetExit(ExitDirection.Up).Target);
        }

        [Fact]
        public void SelfConnection_IsAllowedForMazeRooms()
        {
            var map = MapViewModel.FromProject(ProjectIoService.ImportEngineJson(SourcePath));
            var tardis = map.Rooms.Single(r => r.Slug == "tardis");

            tardis.GetExit(ExitDirection.Down).Target = tardis;
            tardis.SyncExitsToModel();

            Assert.Equal("tardis", tardis.Room.exits["down"]);
        }

        [Fact]
        public void AnyEdit_RaisesChangedEvent_ButSelectionDoesNot()
        {
            var map = MapViewModel.FromProject(ProjectIoService.ImportEngineJson(SourcePath));
            var tardis = map.Rooms.Single(r => r.Slug == "tardis");

            var changedCount = 0;
            map.Changed += (_, _) => changedCount++;

            tardis.IsSelected = true;
            Assert.Equal(0, changedCount);

            tardis.Short = "Edited";
            Assert.Equal(1, changedCount);
        }
    }
}
