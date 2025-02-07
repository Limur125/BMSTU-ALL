using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Demo.WindowsForms.CustomMarkers;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms.ToolTips;
using System.Reflection;
using vkr;

namespace Demo.WindowsForms
{
    public partial class MainForm : Form
    {
        // layers
        readonly GMapOverlay _top = new GMapOverlay();
        internal readonly GMapOverlay Objects = new GMapOverlay("objects");
        internal readonly GMapOverlay Routes = new GMapOverlay("routes");
        internal readonly GMapOverlay Polygons = new GMapOverlay("polygons");

        // marker
        GMapMarker _currentMarker;

        // polygons
        GMapPolygon _polygon;

        // etc
        readonly Random _rnd = new Random();
        readonly DescendingComparer _comparerIpStatus = new DescendingComparer();
        GMapMarkerRect _curentRectMarker;
        string _mobileGpsLog = string.Empty;
        bool _isMouseDown;
        PointLatLng _start;
        PointLatLng _end;

        PointLatLng[][] elemPoints;

        public MainForm()
        {
            InitializeComponent();

            if (!GMapControl.IsDesignerHosted)
            {
                // add your custom map db provider
                //MsSQLPureImageCache ch = new MsSQLPureImageCache();
                //ch.ConnectionString = @"data source = sql5040.site4now.net;User Id=DB_A3B2C9_GMapNET_admin; initial catalog = DB_A3B2C9_GMapNET; password = Usuario@2018;";                
                //MainMap.Manager.SecondaryCache = ch;

                // set your proxy here if need
                //GMapProvider.IsSocksProxy = true;
                //GMapProvider.WebProxy = new WebProxy("127.0.0.1", 1080);
                //GMapProvider.WebProxy.Credentials = new NetworkCredential("ogrenci@bilgeadam.com", "bilgeada");
                // or
                //GMapProvider.WebProxy = WebRequest.DefaultWebProxy;
                //                          

                // set cache mode only if no internet available
                if (!Stuff.PingNetwork("google.com"))
                {
                    MainMap.Manager.Mode = AccessMode.CacheOnly;
                    MessageBox.Show("No internet connection available, going to CacheOnly mode.", "GMap.NET - Demo.WindowsForms", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                //----------------------------------------
                // Config Map at Startup
                //----------------------------------------
                MainMap.MapProvider = GMapProviders.GoogleMap;

                OpenStreetMapGraphHopperProvider.Instance.ApiKey = Stuff.OpenStreetMapsGraphHopperApiKey;
                GoogleMapProvider.Instance.ApiKey = Stuff.GoogleMapsApiKey;

                // Custom Map Provider
                //MainMap.MapProvider = GMapProviders.CustomMap;
                //GMapProviders.CustomMap.CustomServerUrl = "https://{l}.tile.openstreetmap.org/{z}/{x}/{y}.png";
                //GMapProviders.CustomMap.CustomServerLetters = "abc";

                //----------------------------------------
                // Initial Position
                //----------------------------------------
                MainMap.Position = new PointLatLng(64.54302, 40.55);
                MainMap.MinZoom = 0;
                MainMap.MaxZoom = 24;
                MainMap.Zoom = 9;

                textBoxLat.Text = MainMap.Position.Lat.ToString(CultureInfo.InvariantCulture).Replace('.',',');
                textBoxLng.Text = MainMap.Position.Lng.ToString(CultureInfo.InvariantCulture).Replace('.',',');
                textBoxGeo.Text = "2";
                textBoxElemBox.Text = "0,2";

                MainMap.ScaleMode = ScaleModes.Fractional;

                //----------------------------------------
                // Map Events
                //----------------------------------------
                MainMap.OnPositionChanged += MainMap_OnPositionChanged;
                MainMap.OnTileLoadStart += MainMap_OnTileLoadStart;
                MainMap.OnTileLoadComplete += MainMap_OnTileLoadComplete;
                MainMap.OnMapClick += MainMap_OnMapClick;

                MainMap.OnMapZoomChanged += MainMap_OnMapZoomChanged;

                MainMap.OnMarkerClick += MainMap_OnMarkerClick;
                MainMap.OnMarkerDoubleClick += MainMap_OnMarkerDoubleClick;

                MainMap.OnMarkerEnter += MainMap_OnMarkerEnter;
                MainMap.OnMarkerLeave += MainMap_OnMarkerLeave;

                MainMap.OnPolygonEnter += MainMap_OnPolygonEnter;
                MainMap.OnPolygonLeave += MainMap_OnPolygonLeave;

                MainMap.OnPolygonClick += MainMap_OnPolygonClick;
                MainMap.OnPolygonDoubleClick += MainMap_OnPolygonDoubleClick;

                MainMap.OnRouteEnter += MainMap_OnRouteEnter;
                MainMap.OnRouteLeave += MainMap_OnRouteLeave;

                MainMap.OnRouteClick += MainMap_OnRouteClick;
                MainMap.OnRouteDoubleClick += MainMap_OnRouteDoubleClick;

                MainMap.MouseMove += MainMap_MouseMove;
                MainMap.MouseDown += MainMap_MouseDown;
                MainMap.MouseUp += MainMap_MouseUp;
                MainMap.MouseDoubleClick += MainMap_MouseDoubleClick;

                //----------------------------------------
                // Custom Layers
                //----------------------------------------
                MainMap.Overlays.Add(Routes);
                MainMap.Overlays.Add(Polygons);
                MainMap.Overlays.Add(Objects);
                MainMap.Overlays.Add(_top);

                //----------------------------------------
                // Other Events
                //----------------------------------------
                Routes.Routes.CollectionChanged += Routes_CollectionChanged;
                Objects.Markers.CollectionChanged += Markers_CollectionChanged;

                //----------------------------------------
                // Background Workers
                //----------------------------------------

                // vehicle demo ([jokubokla]: Doesn't seem to work anymore, to be investigated)
                _transportWorker.DoWork += transport_DoWork;
                _transportWorker.ProgressChanged += transport_ProgressChanged;
                _transportWorker.WorkerSupportsCancellation = true;
                _transportWorker.WorkerReportsProgress = true;

                // performance demo
                _timerPerf.Tick += timer_Tick;

                

                // set current marker
                _currentMarker = new GMarkerGoogle(MainMap.Position, GMarkerGoogleType.arrow);
                _currentMarker.IsHitTestVisible = false;
                _top.Markers.Add(_currentMarker);


                MainMap.MapProvider = GMapProviders.OpenStreetMap;
            }
        }

        // center markers on start
        private void MainForm_Load(object sender, EventArgs e)
        {
            //trackBarZoom.Value = (int)MainMap.Zoom * 100;
            Activate();
            TopMost = true;
            TopMost = false;
        }


        private void MainMap_OnMapClick(PointLatLng pointClick, MouseEventArgs e)
        {
            MainMap.FromLocalToLatLng(e.X, e.Y);
        }

        private void MainMap_OnRouteDoubleClick(GMapRoute item, MouseEventArgs e)
        {
            
        }

        private void MainMap_OnRouteClick(GMapRoute item, MouseEventArgs e)
        {
           
        }

        private void MainMap_OnPolygonDoubleClick(GMapPolygon item, MouseEventArgs e)
        {
            
        }

        private void MainMap_OnPolygonClick(GMapPolygon item, MouseEventArgs e)
        {
            
        }

        private void MainMap_OnMarkerDoubleClick(GMapMarker item, MouseEventArgs e)
        {
            
        }

        public T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                formatter.Serialize(ms, obj);

                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

        void Markers_CollectionChanged(object sender, GMap.NET.ObjectModel.NotifyCollectionChangedEventArgs e)
        {
            //textBoxMarkerCount.Text = Objects.Markers.Count.ToString();
        }

        void Routes_CollectionChanged(object sender, GMap.NET.ObjectModel.NotifyCollectionChangedEventArgs e)
        {
            //textBoxRouteCount.Text = Routes.Routes.Count.ToString();
        }

        #region -- performance test --

        double NextDouble(Random rng, double min, double max)
        {
            return min + (rng.NextDouble() * (max - min));
        }

        int _tt;
        void timer_Tick(object sender, EventArgs e)
        {
            var pos = new PointLatLng(NextDouble(_rnd, MainMap.ViewArea.Top, MainMap.ViewArea.Bottom), NextDouble(_rnd, MainMap.ViewArea.Left, MainMap.ViewArea.Right));
            GMapMarker m = new GMarkerGoogle(pos, GMarkerGoogleType.green_pushpin);
            {
                m.ToolTipText = (_tt++).ToString();
                m.ToolTipMode = MarkerTooltipMode.Always;
            }

            Objects.Markers.Add(m);

            if (_tt >= 333)
            {
                _timerPerf.Stop();
                _tt = 0;
            }
        }

        System.Windows.Forms.Timer _timerPerf = new System.Windows.Forms.Timer();
        #endregion


        #region -- transport demo --

        // [jokubokla]: The transport demo doesn't seem to work. Presumably because a public transportation
        // webservice in Vilnius has a new API

        BackgroundWorker _transportWorker = new BackgroundWorker();


        readonly List<VehicleData> _trolleybus = new List<VehicleData>();
        readonly Dictionary<int, GMapMarker> _trolleybusMarkers = new Dictionary<int, GMapMarker>();

        readonly List<VehicleData> _bus = new List<VehicleData>();
        readonly Dictionary<int, GMapMarker> _busMarkers = new Dictionary<int, GMapMarker>();

        bool _firstLoadTrasport = true;
        GMapMarker _currentTransport;

        void transport_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // stops immediate marker/route/polygon invalidations;
            // call Refresh to perform single refresh and reset invalidation state
            MainMap.HoldInvalidation = true;

            lock (_trolleybus)
            {
                foreach (var d in _trolleybus)
                {
                    GMapMarker marker;

                    if (!_trolleybusMarkers.TryGetValue(d.Id, out marker))
                    {
                        marker = new GMarkerGoogle(new PointLatLng(d.Lat, d.Lng), GMarkerGoogleType.red_small);
                        marker.Tag = d.Id;
                        marker.ToolTipMode = MarkerTooltipMode.OnMouseOver;

                        _trolleybusMarkers[d.Id] = marker;
                        Objects.Markers.Add(marker);
                    }
                    else
                    {
                        marker.Position = new PointLatLng(d.Lat, d.Lng);
                        //(marker as GMarkerGoogle).Bearing = (float?) d.Bearing;
                    }
                    marker.ToolTipText = "Trolley " + d.Line + (d.Bearing.HasValue ? ", bearing: " + d.Bearing.Value.ToString() : string.Empty) + ", " + d.Time;

                    if (_currentTransport != null && _currentTransport == marker)
                    {
                        MainMap.Position = marker.Position;
                        if (d.Bearing.HasValue)
                        {
                            MainMap.Bearing = (float)d.Bearing.Value;
                        }
                    }
                }
            }

            lock (_bus)
            {
                foreach (var d in _bus)
                {
                    GMapMarker marker;

                    if (!_busMarkers.TryGetValue(d.Id, out marker))
                    {
                        marker = new GMarkerGoogle(new PointLatLng(d.Lat, d.Lng), GMarkerGoogleType.green_small);
                        marker.Tag = d.Id;
                        marker.ToolTipMode = MarkerTooltipMode.OnMouseOver;

                        _busMarkers[d.Id] = marker;
                        Objects.Markers.Add(marker);
                    }
                    else
                    {
                        marker.Position = new PointLatLng(d.Lat, d.Lng);
                        //(marker as GMarkerGoogle).Bearing = (float?) d.Bearing;
                    }
                    marker.ToolTipText = "Bus " + d.Line + (d.Bearing.HasValue ? ", bearing: " + d.Bearing.Value.ToString() : string.Empty) + ", " + d.Time;

                    if (_currentTransport != null && _currentTransport == marker)
                    {
                        MainMap.Position = marker.Position;
                        if (d.Bearing.HasValue)
                        {
                            MainMap.Bearing = (float)d.Bearing.Value;
                        }
                    }
                }
            }

            if (_firstLoadTrasport)
            {
                MainMap.Zoom = 5;
                MainMap.ZoomAndCenterMarkers("objects");
                _firstLoadTrasport = false;
            }
            MainMap.Refresh();
        }

        void transport_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!_transportWorker.CancellationPending)
            {
                try
                {
                    #region -- old vehicle demo --
                    lock (_trolleybus)
                    {
                        Stuff.GetVilniusTransportData(TransportType.TrolleyBus, string.Empty, _trolleybus);
                    }

                    lock (_bus)
                    {
                        Stuff.GetVilniusTransportData(TransportType.Bus, string.Empty, _bus);
                    }
                    #endregion

                    _transportWorker.ReportProgress(100);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("transport_DoWork: " + ex.ToString());
                }
                Thread.Sleep(2 * 1000);
            }

            _trolleybusMarkers.Clear();
            _busMarkers.Clear();
        }

        #endregion


        #region -- some functions --

        void RegeneratePolygon()
        {
            var polygonPoints = new List<PointLatLng>();

            foreach (var m in Objects.Markers)
            {
                if (m is GMapMarkerRect)
                {
                    m.Tag = polygonPoints.Count;
                    polygonPoints.Add(m.Position);
                }
            }

            if (_polygon == null)
            {
                _polygon = new GMapPolygon(polygonPoints, "polygon test");
                _polygon.IsHitTestVisible = true;
                Polygons.Polygons.Add(_polygon);
            }
            else
            {
                _polygon.Points.Clear();
                _polygon.Points.AddRange(polygonPoints);

                if (Polygons.Polygons.Count == 0)
                {
                    Polygons.Polygons.Add(_polygon);
                }
                else
                {
                    MainMap.UpdatePolygonLocalPosition(_polygon);
                }
            }
        }


        /// <summary>
        /// adds marker using geocoder
        /// </summary>
        /// <param name="place"></param>
        void AddLocationLithuania(string place)
        {
            GeoCoderStatusCode status;
            var pos = MainMap.GeocodingProvider.GetPoint("Lithuania, " + place, out status);
            if (pos != null && status == GeoCoderStatusCode.OK)
            {
                var m = new GMarkerGoogle(pos.Value, GMarkerGoogleType.green);
                m.ToolTip = new GMapRoundedToolTip(m);

                var mBorders = new GMapMarkerRect(pos.Value);
                {
                    mBorders.InnerMarker = m;
                    mBorders.ToolTipText = place;
                    mBorders.ToolTipMode = MarkerTooltipMode.Always;
                }

                Objects.Markers.Add(m);
                Objects.Markers.Add(mBorders);
            }
        }

        bool TryExtractLeafletjs()
        {
            try
            {
                string launch = string.Empty;

                string[] x = Assembly.GetExecutingAssembly().GetManifestResourceNames();
                foreach (string f in x)
                {
                    if (f.Contains("leafletjs"))
                    {
                        string fName = f.Replace("Demo.WindowsForms.", string.Empty);
                        fName = fName.Replace(".", "\\");
                        int ll = fName.LastIndexOf("\\");
                        string name = fName.Substring(0, ll) + "." + fName.Substring(ll + 1, fName.Length - ll - 1);

                        //Demo.WindowsForms.leafletjs.dist.leaflet.js

                        using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(f))
                        {
                            string fileFullPath = MainMap.CacheLocation + name;

                            if (fileFullPath.Contains("gmap.html"))
                            {
                                launch = fileFullPath;
                            }

                            string dir = Path.GetDirectoryName(fileFullPath);
                            if (!Directory.Exists(dir))
                            {
                                Directory.CreateDirectory(dir);
                            }

                            using (var fileStream = File.Create(fileFullPath, (int)stream.Length))
                            {
                                // Fill the bytes[] array with the stream data
                                byte[] bytesInStream = new byte[stream.Length];
                                stream.Read(bytesInStream, 0, bytesInStream.Length);

                                // Use FileStream object to write to the specified file
                                fileStream.Write(bytesInStream, 0, bytesInStream.Length);
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(launch))
                {
                    Process.Start(launch);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("TryExtractLeafletjs: " + ex);
                return false;
            }
            return true;
        }

        #endregion


        #region -- map events --


        void MainMap_OnMarkerLeave(GMapMarker item)
        {
            if (item is GMapMarkerRect)
            {
                _curentRectMarker = null;

                var rc = item as GMapMarkerRect;
                rc.Pen.Color = Color.Blue;

                Debug.WriteLine("OnMarkerLeave: " + item.Position);
            }
        }

        void MainMap_OnMarkerEnter(GMapMarker item)
        {
            if (item is GMapMarkerRect)
            {
                var rc = item as GMapMarkerRect;
                rc.Pen.Color = Color.Red;

                _curentRectMarker = rc;
            }
            Debug.WriteLine("OnMarkerEnter: " + item.Position);
        }

        GMapPolygon _currentPolygon;
        void MainMap_OnPolygonLeave(GMapPolygon item)
        {
            _currentPolygon = null;
            item.Stroke.Color = Color.MidnightBlue;
            Debug.WriteLine("OnPolygonLeave: " + item.Name);
        }

        void MainMap_OnPolygonEnter(GMapPolygon item)
        {
            _currentPolygon = item;
            item.Stroke.Color = Color.Red;
            Debug.WriteLine("OnPolygonEnter: " + item.Name);
        }

        GMapRoute _currentRoute;
        void MainMap_OnRouteLeave(GMapRoute item)
        {
            _currentRoute = null;
            item.Stroke.Color = Color.MidnightBlue;
            Debug.WriteLine("OnRouteLeave: " + item.Name);
        }

        void MainMap_OnRouteEnter(GMapRoute item)
        {
            _currentRoute = item;
            item.Stroke.Color = Color.Red;
            Debug.WriteLine("OnRouteEnter: " + item.Name);
        }

        void MainMap_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isMouseDown = false;
            }
        }

        // add demo circle
        void MainMap_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var cc = new GMapMarkerCircle(MainMap.FromLocalToLatLng(e.X, e.Y));
            Objects.Markers.Add(cc);
        }

        void MainMap_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isMouseDown = true;

                if (_currentMarker.IsVisible)
                {
                    _currentMarker.Position = MainMap.FromLocalToLatLng(e.X, e.Y);

                    var px = MainMap.MapProvider.Projection.FromLatLngToPixel(_currentMarker.Position.Lat, _currentMarker.Position.Lng, (int)MainMap.Zoom);
                    var tile = MainMap.MapProvider.Projection.FromPixelToTileXY(px);

                    Debug.WriteLine("MouseDown: geo: " + _currentMarker.Position + " | px: " + px + " | tile: " + tile);
                }
            }
        }

        // move current marker with left holding
        void MainMap_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && _isMouseDown)
            {
                if (_curentRectMarker == null)
                {
                    if (_currentMarker.IsVisible)
                    {
                        _currentMarker.Position = MainMap.FromLocalToLatLng(e.X, e.Y);
                    }
                }
                else // move rect marker
                {
                    var pnew = MainMap.FromLocalToLatLng(e.X, e.Y);

                    int? pIndex = (int?)_curentRectMarker.Tag;
                    if (pIndex.HasValue)
                    {
                        if (pIndex < _polygon.Points.Count)
                        {
                            _polygon.Points[pIndex.Value] = pnew;
                            MainMap.UpdatePolygonLocalPosition(_polygon);
                        }
                    }

                    if (_currentMarker.IsVisible)
                    {
                        _currentMarker.Position = pnew;
                    }
                    _curentRectMarker.Position = pnew;

                    if (_curentRectMarker.InnerMarker != null)
                    {
                        _curentRectMarker.InnerMarker.Position = pnew;
                    }
                }

                MainMap.Refresh(); // force instant invalidation
            }
        }

        // MapZoomChanged
        void MainMap_OnMapZoomChanged()
        {
            //trackBarZoom.Value = (int)(MainMap.Zoom * 100.0);
            //textBoxZoomCurrent.Text = MainMap.Zoom.ToString();
        }

        // click on some marker
        void MainMap_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (item is GMapMarkerRect)
                {
                    GeoCoderStatusCode status;
                    var pos = MainMap.GeocodingProvider.GetPlacemark(item.Position, out status);
                    if (status == GeoCoderStatusCode.OK && pos != null)
                    {
                        var v = item as GMapMarkerRect;
                        {
                            v.ToolTipText = pos.Value.Address;
                        }
                        MainMap.Invalidate(false);
                    }
                }
                else
                {
                    if (item.Tag != null)
                    {
                        if (_currentTransport != null)
                        {
                            _currentTransport.ToolTipMode = MarkerTooltipMode.OnMouseOver;
                            _currentTransport = null;
                        }
                        _currentTransport = item;
                        _currentTransport.ToolTipMode = MarkerTooltipMode.Always;
                    }
                }
            }
        }

        // loader start loading tiles
        void MainMap_OnTileLoadStart()
        {
            MethodInvoker m = delegate ()
            {
                // HACK JKU CLEAN
                //panelMenu.Text = "Menu: loading tiles...";
            };
            try
            {
                BeginInvoke(m);
            }
            catch
            {
            }
        }

        // loader end loading tiles
        void MainMap_OnTileLoadComplete(long elapsedMilliseconds)
        {
            // HACK JKU CLEAN
            //MainMap.ElapsedMilliseconds = elapsedMilliseconds;

            MethodInvoker m = delegate ()
            {
                
                // HACK JKU CLEAN
                //panelMenu.Text = "Menu, last load in " + MainMap.ElapsedMilliseconds + "ms";
                //textBoxMemory.Text = string.Format(CultureInfo.InvariantCulture, "{0:0.00} MB of {1:0.00} MB", MainMap.Manager.MemoryCache.Size, MainMap.Manager.MemoryCache.Capacity);
            };
            try
            {
                BeginInvoke(m);
            }
            catch
            {
            }
        }

        // current point changed
        void MainMap_OnPositionChanged(PointLatLng point)
        {
            //textBoxLatCurrent.Text = point.Lat.ToString(CultureInfo.InvariantCulture);
            //textBoxLngCurrent.Text = point.Lng.ToString(CultureInfo.InvariantCulture);

        }

        #endregion


        #region -- ui events --

        bool _userAcceptedLicenseOnce;

        // goto by geocoder
        private void textBoxGeo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Keys)e.KeyChar == Keys.Enter)
            {
                var status = MainMap.SetPositionByKeywords(textBoxGeo.Text);

                if (status != GeoCoderStatusCode.OK)
                {
                    MessageBox.Show("Geocoder can't find: '" + textBoxGeo.Text + "', reason: " + status.ToString(), "GMap.NET", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        // go to
        private void btnGoTo_Click(object sender, EventArgs e)
        {
            if (!float.TryParse(textBoxLat.Text, out float lat))
            {
                return;
            }
            if (!float.TryParse(textBoxLng.Text, out float lng))
                return;
            if (!float.TryParse(textBoxGeo.Text, out float length))
                return;

            if (!float.TryParse(textBoxElemBox.Text, out float eleLength))
                return;
            length *= 1000;
            eleLength *= 1000;
            var boundBox = getBoundingBox(lat, lng, (int)length);

            MainMap.Position = new PointLatLng(lat, lng);

            var polygonPoints = new List<PointLatLng>
            {
                new PointLatLng(boundBox[0], boundBox[1]),
                new PointLatLng(boundBox[0], boundBox[3]),
                new PointLatLng(boundBox[2], boundBox[3]),
                new PointLatLng(boundBox[2], boundBox[1]),
            };
            _polygon = new GMapPolygon(polygonPoints, "poly");

            Polygons.Polygons.Clear();
            Polygons.Polygons.Add(_polygon);

            int n = (int)(length / eleLength);
            elemPoints = new PointLatLng[n][];
            var initPoint = getCoord(new PointLatLng(boundBox[2], boundBox[1]), eleLength, -eleLength);
            for (var i = 0; i < n; i++)
            {
                var firstPoint = getCoord(initPoint, 0, -eleLength * i * 2);
                elemPoints[i] = new PointLatLng[n];
                for (var j = 0; j < n; j++)
                {
                    var curPoint = getCoord(firstPoint, eleLength * j * 2, 0);
                    elemPoints[i][j] = curPoint;
                }
            }
        }

        private PointLatLng getCoord(PointLatLng point, float x, float y)
        {
            double latRadian = point.Lat * Math.PI / 180;

            double degLatKm = 110.574235;
            double degLongKm = 110.572833 * Math.Cos(latRadian);
            double deltaLat = y / 1000.0 / degLatKm / 2;
            double deltaLong = x / 1000.0 / degLongKm / 2;

            var newPoint = new PointLatLng(point.Lat + deltaLat, point.Lng + deltaLong);
            return newPoint;
        }

        private double[] getBoundingBox(float pLatitude, float pLongitude, int pDistanceInMeters)
        {

            double[] boundingBox = new double[4];

            double latRadian = pLatitude * Math.PI / 180;

            double degLatKm = 110.574235;
            double degLongKm = 110.572833 * Math.Cos(latRadian);
            double deltaLat = pDistanceInMeters / 1000.0 / degLatKm / 2;
            double deltaLong = pDistanceInMeters / 1000.0 / degLongKm / 2;
            
            double minLat = pLatitude - deltaLat;
            double minLong = pLongitude - deltaLong;
            double maxLat = pLatitude + deltaLat;
            double maxLong = pLongitude + deltaLong;

            boundingBox[0] = minLat;
            boundingBox[1] = minLong;
            boundingBox[2] = maxLat;
            boundingBox[3] = maxLong;

            return boundingBox;
        }

        // reload map
        private void btnReload_Click(object sender, EventArgs e)
        {
            MainMap.ReloadMap();
        }




        // add marker on current position
        private void btnAddMarker_Click(object sender, EventArgs e)
        {
            var m = new GMarkerGoogle(_currentMarker.Position, GMarkerGoogleType.green_pushpin);

            Objects.Markers.Add(m);
        }

        // clear markers, routes and polygons
        private void btnClearAll_Click(object sender, EventArgs e)
        {
            Objects.Markers.Clear();
            Routes.Routes.Clear();
            Polygons.Polygons.Clear();
        }

        // set route start
        private void btnSetStart_Click(object sender, EventArgs e)
        {
            _start = _currentMarker.Position;
        }

        // set route end
        private void btnSetEnd_Click(object sender, EventArgs e)
        {
            _end = _currentMarker.Position;
        }

        // zoom to max for markers
        private void btnZoomCenter_Click(object sender, EventArgs e)
        {
            MainMap.ZoomAndCenterMarkers("objects");
        }

        // export map data
        private void btnExport_Click(object sender, EventArgs e)
        {
            MainMap.ShowExportDialog();
        }

        // import map data
        private void btnImport_Click(object sender, EventArgs e)
        {
            MainMap.ShowImportDialog();
        }

        // prefetch
        private void btnCachePrefetch_Click(object sender, EventArgs e)
        {
            var area = MainMap.SelectedArea;
            if (!area.IsEmpty)
            {
                for (int i = (int)MainMap.Zoom; i <= MainMap.MaxZoom; i++)
                {
                    var res = MessageBox.Show("Ready ripp at Zoom = " + i + " ?", "GMap.NET", MessageBoxButtons.YesNoCancel);

                    if (res == DialogResult.Yes)
                    {
                        using (var obj = new TilePrefetcher())
                        {
                            obj.Overlay = Objects; // set overlay if you want to see cache progress on the map

                            obj.Shuffle = MainMap.Manager.Mode != AccessMode.CacheOnly;

                            obj.Owner = this;
                            obj.ShowCompleteMessage = true;
                            obj.Start(area, i, MainMap.MapProvider, MainMap.Manager.Mode == AccessMode.CacheOnly ? 0 : 100, MainMap.Manager.Mode == AccessMode.CacheOnly ? 0 : 1);
                        }
                    }
                    else if (res == DialogResult.No)
                    {
                        continue;
                    }
                    else if (res == DialogResult.Cancel)
                    {
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show("Select map area holding ALT", "GMap.NET", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        // launch static map maker
        private void btnSave_Click(object sender, EventArgs e)
        {
            var st = new StaticImage(this);
            st.Owner = this;
            st.Show();
        }

        // key-up events
        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            int offset = -22;

            if (e.KeyCode == Keys.Left)
            {
                MainMap.Offset(-offset, 0);
            }
            else if (e.KeyCode == Keys.Right)
            {
                MainMap.Offset(offset, 0);
            }
            else if (e.KeyCode == Keys.Up)
            {
                MainMap.Offset(0, -offset);
            }
            else if (e.KeyCode == Keys.Down)
            {
                MainMap.Offset(0, offset);
            }
            else if (e.KeyCode == Keys.Delete)
            {
                if (_currentPolygon != null)
                {
                    Polygons.Polygons.Remove(_currentPolygon);
                    _currentPolygon = null;
                }

                if (_currentRoute != null)
                {
                    Routes.Routes.Remove(_currentRoute);
                    _currentRoute = null;
                }

                if (_curentRectMarker != null)
                {
                    Objects.Markers.Remove(_curentRectMarker);

                    if (_curentRectMarker.InnerMarker != null)
                    {
                        Objects.Markers.Remove(_curentRectMarker.InnerMarker);
                    }
                    _curentRectMarker = null;

                    RegeneratePolygon();
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                MainMap.Bearing = 0;

                if (_currentTransport != null && !MainMap.IsMouseOverMarker)
                {
                    _currentTransport.ToolTipMode = MarkerTooltipMode.OnMouseOver;
                    _currentTransport = null;
                }
            }
        }

        // key-press events
        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (MainMap.Focused)
            {
                if (e.KeyChar == '+')
                {
                    MainMap.Zoom = ((int)MainMap.Zoom) + 1;
                }
                else if (e.KeyChar == '-')
                {
                    MainMap.Zoom = ((int)(MainMap.Zoom + 0.99)) - 1;
                }
                else if (e.KeyChar == 'a')
                {
                    MainMap.Bearing--;
                }
                else if (e.KeyChar == 'z')
                {
                    MainMap.Bearing++;
                }
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<BaseStationType> types = new List<BaseStationType>();
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++) 
            {
                BaseStationType type = new BaseStationType();
                var row = dataGridView1.Rows[i];
                type.W = double.Parse(row.Cells[0].Value.ToString());
                type.Cost = int.Parse(row.Cells[2].Value.ToString());
                type.fc = double.Parse(row.Cells[3].Value.ToString());
                type.hb = double.Parse(row.Cells[1].Value.ToString());
                types.Add(type);
            }
            List<PointLatLng> bsPlaces = new List<PointLatLng>();
            for (int i = 0; i < Objects.Markers.Count; i++) 
            {
                bsPlaces.Add(Objects.Markers[i].Position);
            }
            Stopwatch sw = Stopwatch.StartNew();
            Method method = new Method(elemPoints, bsPlaces.ToArray(), types.ToArray(), 1);
            method.Preparation(0);
            var result = method.GeneticAlgo(300, 10);
            sw.Stop();
            dataGridView2.Rows.Clear();
            Objects.Markers.Clear();
            for (int i = 0; i < bsPlaces.Count; i++) 
            {
                for (int j = 0; j < types.Count; j++) 
                {
                    if (result.Gens[i * types.Count +  j] != 0) 
                    {
                        var row = new object[3];
                        row[0] = bsPlaces[i].Lat;
                        row[1] = bsPlaces[i].Lng;
                        row[2] = j;
                        dataGridView2.Rows.Add(row);
                        Objects.Markers.Add(new GMarkerGoogle(new PointLatLng(bsPlaces[i].Lat, bsPlaces[i].Lng), GMarkerGoogleType.red_pushpin));
                    }
                }
            }
            label8.Text = sw.ElapsedMilliseconds.ToString(); 
            
            label6.Text = result.Cost.ToString();
            Console.WriteLine($"{textBoxElemBox.Text} {sw.ElapsedMilliseconds} {result.Cost}");

        }

        private void textBoxLat_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var eleSizes = new double[] { 0.5, 0.4, 0.3, 0.2, 0.1, 0.05, 0.04, 0.03, 0.02, 0.01 };
            foreach (var eleSize in eleSizes)
            {
                textBoxElemBox.Text = eleSize.ToString();
                btnGoTo_Click(sender, e);
                button1_Click(sender, e);
            }
        }
    }

        #endregion
}
