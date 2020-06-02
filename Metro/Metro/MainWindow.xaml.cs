using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Metro
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<string, Node> stationsdict = new Dictionary<string, Node>();
        List<Node> stations = new List<Node>();
        List<Edge> edges = new List<Edge>();

        public MainWindow()
        { 
            InitializeComponent();
            //MouseMove += OnMouseMove;
            //Closed += (object sender, EventArgs e) => { reader.Close();
                //writer.Close();
            //};
           // MouseDown += OnMouseDown;
        }

        //private void OnMouseMove(object sender, MouseEventArgs e)
        //{
          //  Start.Text = String.Format("X: {0} , Y: {1}",e.GetPosition(this).X, e.GetPosition(this).Y);
        //}

        //private void OnMouseDown(object sender, MouseEventArgs e)
        //{
        //    writer.WriteLine(e.GetPosition(this).X);
        //    writer.WriteLine(e.GetPosition(this).Y);
        //}

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ReadPoints();
        }

        private void Route_Click(object sender, RoutedEventArgs e)
        {
            CreateEdges(ref edges);

            var stats = stations.ToArray();
            var edgs = edges.ToArray();

            var start = DateTime.Now;
            Dijkstra findway = new Dijkstra(stats, edgs);
            stationsdict[Start.Text].Value = 0;
            findway.RunAlgorithm(stationsdict[Start.Text]);

            List<Node> MyWay = findway.MinAlgorithm(stationsdict[Destination.Text]);

            var end = DateTime.Now;
            var diff = end - start;

            MessageBox.Show($"Elapsed time: {diff}\n Steps taken: {findway.STEPS}");

            IEnumerable<Point> Points = from ps in MyWay select ps.SPoint;

            PathFigure geometry = new PathFigure();
            geometry.StartPoint = stationsdict[Start.Text].SPoint;
            geometry.Segments.Add(new PolyLineSegment(Points.Reverse(),false));

            PathGeometry path = FindResource("MetroPath") as PathGeometry;
            path.Figures.Add(geometry);

            Storyboard story = FindResource("MetroAnim") as Storyboard;
            story.Begin();
        }

        private void ReadPoints()    // тут повинен бути .json, але я вже мав готові .txt файли)
        {
            using (StreamReader reader = new StreamReader("Points.txt")) 
            {
                while (reader.Peek() != -1)
                {
                    int x = Convert.ToInt32(reader.ReadLine());
                    int y = Convert.ToInt32(reader.ReadLine());

                    stations.Add(new Node(999, new Point(x, y)));
                }
            }

            using (StreamReader reader2 = new StreamReader("StLines.txt"))
            {
                int count = 0;
                while (reader2.Peek() != -1)
                {
                    string stname = reader2.ReadLine();
                    stations[count].Name = stname;
                    stationsdict.Add(stname, stations[count]);
                    count++;
                }
            }

            Start.ItemsSource = stations;
            Start.DisplayMemberPath = "Name";
            Destination.ItemsSource = stations;
            Destination.DisplayMemberPath = "Name";
        }

        private void CreateEdges(ref List<Edge> edges)
        {
            for(int i = 1; i < stations.Count;i++)
            {
                if (i == 18 || i == 34) continue;
                edges.Add(new Edge(stations[i - 1], stations[i]));
            }
            edges.Add(new Edge(stationsdict["Teatralna"], stationsdict["Zoloti Vorota"]));
            edges.Add(new Edge(stationsdict["Khreshchatyk"], stationsdict["Maidan Nezalezhnosti"]));
            edges.Add(new Edge(stationsdict["Palats Sportu"],stationsdict["Ploscha Lva Tolstoho"]));
        }

    }
}
