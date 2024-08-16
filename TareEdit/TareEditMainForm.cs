using TareEngine.Serialization;

namespace TareEdit
{
    public partial class TareEditMainForm : Form
    {
        private Rooms _rooms;

        public TareEditMainForm()
        {
            InitializeComponent();
            lstRooms.SelectedIndexChanged += LstRooms_SelectedIndexChanged;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var file = @"C:\Users\sloan\source\repos\TARDIS\TARE\Content\thedata.json";
            LoadData(file);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Open Data File";
            openFileDialog.InitialDirectory = @"C:\Users\sloan\source\repos\TARDIS\TARE\Content";
            openFileDialog.Filter = "JSON Files (*.json)|*.json";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                LoadData(openFileDialog.FileName);
            }
        }

        private void LoadData(string filename)
        {
            var data = GameDataSerializer.GetDataFullPath(filename);
            _rooms = new Rooms(data.rooms);
            FillRooms();
        }

        private void FillRooms()
        {
            lstRooms.Items.Clear();
            lstRooms.DataSource = _rooms.RoomCollection.Select(r => r.slug)
                .ToArray();
        }

        private void LstRooms_SelectedIndexChanged(object? sender, EventArgs e)
        {
            
        }
    }
}
