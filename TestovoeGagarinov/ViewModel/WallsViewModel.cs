using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TestovoeGagarinov.Filters;
using TestovoeGagarinov.Handlers;
using TestovoeGagarinov.Sevices;
using TestovoeGagarinov.Utils;

namespace TestovoeGagarinov.ViewModel
{
    internal class WallsViewModel : BaseViewModel
    {
        private readonly BuildWallsHandler _handler;
        private readonly ExternalEvent _event;

        private readonly UserRoomSelectionService _roomService;

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
            IList<Room> rooms = null;
            try
            {
                rooms = _roomService.PickElements();
            }
            catch (OperationCanceledException)
            {
                TaskDialog.Show("Пустота", "Вы не выбрали помещения!");
            }
            catch (Exception )
            {
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
