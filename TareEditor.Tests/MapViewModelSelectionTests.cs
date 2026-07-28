using System.IO;
using System.Windows;
using TareEditor.Models;
using TareEditor.Services;
using TareEditor.ViewModels;

namespace TareEditor.Tests
{
    public class MapViewModelSelectionTests
    {
        private static string SourcePath => Path.Combine(AppContext.BaseDirectory, "TestData", "thedata.json");

        [Fact]
        public void SelectRoom_ClearsOtherRoomAndExitSelection()
        {
            var map = MapViewModel.FromProject(ProjectIoService.ImportEngineJson(SourcePath));
            var tardis = map.Rooms.Single(r => r.Slug == "tardis");
            var outsideTardis = map.Rooms.Single(r => r.Slug == "outside-tardis");
            var someExit = outsideTardis.GetExit(ExitDirection.North);

            outsideTardis.IsSelected = true;
            someExit.IsSelected = true;

            map.SelectRoom(tardis);

            Assert.True(tardis.IsSelected);
            Assert.False(outsideTardis.IsSelected);
            Assert.False(someExit.IsSelected);
        }

        [Fact]
        public void SelectExit_ClearsAllRoomSelection()
        {
            var map = MapViewModel.FromProject(ProjectIoService.ImportEngineJson(SourcePath));
            var tardis = map.Rooms.Single(r => r.Slug == "tardis");
            var outsideTardis = map.Rooms.Single(r => r.Slug == "outside-tardis");
            var exit = outsideTardis.GetExit(ExitDirection.North);

            tardis.IsSelected = true;
            map.SelectExit(exit);

            Assert.True(exit.IsSelected);
            Assert.False(tardis.IsSelected);
        }

        [Fact]
        public void ClearSelection_DeselectsEverything()
        {
            var map = MapViewModel.FromProject(ProjectIoService.ImportEngineJson(SourcePath));
            var tardis = map.Rooms.Single(r => r.Slug == "tardis");
            tardis.IsSelected = true;

            map.ClearSelection();

            Assert.All(map.Rooms, r => Assert.False(r.IsSelected));
            Assert.All(map.AllExits, e => Assert.False(e.IsSelected));
        }

        [Fact]
        public void SelectionChanges_DoNotMarkProjectDirty()
        {
            var map = MapViewModel.FromProject(ProjectIoService.ImportEngineJson(SourcePath));
            var changed = 0;
            map.Changed += (_, _) => changed++;

            var room = map.Rooms.First();
            map.SelectRoom(room);
            map.SelectExit(room.Exits[0]);
            map.ClearSelection();

            Assert.Equal(0, changed);
        }

        [Fact]
        public void AllExits_TracksRoomAdditionAndRemoval()
        {
            var map = MapViewModel.FromProject(ProjectIoService.ImportEngineJson(SourcePath));
            var countBefore = map.AllExits.Count;

            map.AddRoomCommand.Execute(null);
            var newRoom = map.Rooms.Last();
            Assert.Equal(countBefore + 6, map.AllExits.Count);

            map.RemoveRoomCommand.Execute(newRoom);
            Assert.Equal(countBefore, map.AllExits.Count);
            Assert.DoesNotContain(map.AllExits, e => e.Source == newRoom);
        }

        [Fact]
        public void ConnectionDrag_CompleteOverValidTarget_ConnectsExit()
        {
            var map = MapViewModel.FromProject(ProjectIoService.ImportEngineJson(SourcePath));
            var tardis = map.Rooms.Single(r => r.Slug == "tardis");
            var hallWay = map.Rooms.Single(r => r.Slug == "hall-way");

            map.BeginConnectionDrag(tardis, ExitDirection.Up, new Point(0, 0));
            Assert.True(map.IsConnectingExit);

            map.UpdateConnectionDrag(new Point(100, 100));
            Assert.Equal(new Point(100, 100), map.PendingConnectionEnd);

            map.CompleteConnectionDrag(hallWay);

            Assert.Same(hallWay, tardis.GetExit(ExitDirection.Up).Target);
            Assert.False(map.IsConnectingExit);
            Assert.Null(map.PendingSourceRoom);
        }

        [Fact]
        public void ConnectionDrag_CompleteOverNoTarget_CancelsWithoutConnecting()
        {
            var map = MapViewModel.FromProject(ProjectIoService.ImportEngineJson(SourcePath));
            var tardis = map.Rooms.Single(r => r.Slug == "tardis");

            map.BeginConnectionDrag(tardis, ExitDirection.Up, new Point(0, 0));
            map.CompleteConnectionDrag(null);

            Assert.Null(tardis.GetExit(ExitDirection.Up).Target);
            Assert.False(map.IsConnectingExit);
        }

        [Fact]
        public void ConnectionDrag_DropOnSourceRoom_CreatesSelfLoop()
        {
            var map = MapViewModel.FromProject(ProjectIoService.ImportEngineJson(SourcePath));
            var tardis = map.Rooms.Single(r => r.Slug == "tardis");

            map.BeginConnectionDrag(tardis, ExitDirection.Down, new Point(0, 0));
            map.CompleteConnectionDrag(tardis);

            Assert.Same(tardis, tardis.GetExit(ExitDirection.Down).Target);
        }

        [Theory]
        [InlineData(ExitDirection.North, ExitDirection.South)]
        [InlineData(ExitDirection.East, ExitDirection.West)]
        [InlineData(ExitDirection.Up, ExitDirection.Down)]
        public void Opposite_IsSymmetric(ExitDirection a, ExitDirection b)
        {
            Assert.Equal(b, ExitDirectionKeys.Opposite(a));
            Assert.Equal(a, ExitDirectionKeys.Opposite(b));
        }
    }
}
