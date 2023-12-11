using System;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections;
using ScottPlot;


namespace LackmannApi
{
   
    public class MarketDocument 
    {
        public string? MRID {get; set;}
        public int? RevisionNumber{get; set;}
        public string? Type {get; set;}
        public string? SenderMRID {get; set;}
        public string? ReceiverMRID {get; set;}
        public DateTime? CreatedTime {get; set;}
        [JsonIgnore]
        public List<Point>? Points{get; set;}
        [JsonIgnore]
        public List<Point>? HourlyPoints 
        {
            get 
            {       
                return Helper.Aggregate(this, 4);
            }
            private set {}
        }

        [JsonIgnore]
        public List<Point>? DailyPoints
        {
            get
            {
               return Helper.Aggregate(this, 96);
            }
            private set {}
        }

        [JsonIgnore]
        public List<Point>? WeeklyPoints
        {
            get
            {
                return Helper.Aggregate(this, 672);
            }
            private set {}
        }

        [JsonIgnore]
        public List<Point>? MonthlyPoints
        {
            get
            {
                return Helper.Aggregate(this, 2928);
            }
            private set {}
        }

        public MarketDocument(){}

        public MarketDocument(string mRID, int revisionNumber, 
        string type, string senderMRID, string receiverMRID, 
        DateTime createdTime, List<Point> points)
        {
            this.MRID = mRID;
            this.RevisionNumber = revisionNumber;
            this.Type = type;
            this.SenderMRID = senderMRID;
            this.ReceiverMRID = receiverMRID;
            this.CreatedTime = createdTime;
            this.Points = points;
        }
        public MarketDocument(string mRID, int revisionNumber, 
        string type, string senderMRID, string receiverMRID, 
        DateTime createdTime)
        {
            this.MRID = mRID;
            this.RevisionNumber = revisionNumber;
            this.Type = type;
            this.SenderMRID = senderMRID;
            this.ReceiverMRID = receiverMRID;
            this.CreatedTime = createdTime;
        }



        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}, {3}, {4}, {5}", MRID, RevisionNumber, Type, SenderMRID, ReceiverMRID, CreatedTime);
        }

        public void DrawGraph()
        {
            int lengthOfArray = 7; //this.Points.Count;

            double[] x_axis = new double[lengthOfArray];
            double[] y_axis = new double[lengthOfArray];

            int index = 0;
            foreach (Point point in this.Points)
            {
                if(index == lengthOfArray)
                {
                    break;
                }
                x_axis[index] = point.Position;
                y_axis[index] = point.Quantity;
                Console.Write($"{point.Quantity} ");
                index = index + 1;
            }
            var plot = new ScottPlot.Plot(400, 300);
            plot.AddScatter(x_axis, y_axis);
            plot.SaveFig($"Graph {this.MRID}.png");
        }
     

    }
    public class Point 
    {
        // seems like every point of this graph is a snapshot at a 15 minute time interval
        // 365 days * 24 hours * 4 quarters = 35040 points (for the first document).

        public int Position {get; set;}
        public int Quantity {get; set;}

        public Point(){}
        public Point(int position, int quantity)
        {
            this.Position = position;
            this.Quantity = quantity;
        }
    }

    class Helper
    {
        public static List<Point> Aggregate(MarketDocument document, int step)
        {
            List<Point> points = new List<Point>();

                for ( int i = 0, positionOfPoint = 1; i < document.Points.Count - step; i = i + step, positionOfPoint++ )
                {
                    int averageInWeek = 0;
                    
                    for( int j = i; j < i + step; j++ )
                    {
                        averageInWeek = averageInWeek + document.Points[j].Quantity;
                    }
                    averageInWeek = averageInWeek / step;
                    Point point = new Point(positionOfPoint, (int) averageInWeek);
                    points.Add(point);
                }
                return points;
        }
    }
}