using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.Exceptions;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using TestovoeGagarinov.Filters;
using TestovoeGagarinov.Handlers;
using TestovoeGagarinov.Services;
using TestovoeGagarinov.Utils;

namespace TestovoeGagarinov.ViewModel
{
    internal class WallsViewModel : BaseViewModel
    {
        private readonly BuildWallsHandler _handler;
        private readonly ExternalEvent _event;

        private readonly SelectionServiceBase<Room> _roomService;

        public RelayCommand PickRoomsCommand { get; }
        public RelayCommand BuildWallsCommand { get; }

        public ObservableCollection<Room> Rooms { get;  } = new ObservableCollection<Room>();

        public WallsViewModel()
        {
            PickRoomsCommand = new RelayCommand(PickRooms);
            BuildWallsCommand = new RelayCommand(BuildWalls, () => { return Rooms.Any(); });

            _handler = new BuildWallsHandler { };
            _event = ExternalEvent.Create(_handler);

            _roomService = new UserRoomSelectionService();
        }

        private void PickRooms()
        {
            Rooms.Clear();
            IList<Room> rooms;
            try
            {
                rooms = _roomService.PickElements();
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                TaskDialog.Show("Пустота", "Вы не выбрали помещения!");
                return;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Ошибка", $"Непредвиденная ошибка! {ex}");
                return;
            }

            foreach(var r in rooms)
                Rooms.Add(r);
        }

        private void BuildWalls()
        {
            _handler.Rooms = Rooms;
            _event.Raise();
        }
    }
}
