using System;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Collections;

namespace LackmannApi
{
   
    public class MarketDocument 
    {
        public string MRID {get; set;}
        public int RevisionNumber{get; set;}
        public string Type {get; set;}
        public string SenderMRID {get; set;}
        public string ReceiverMRID {get; set;}
        public DateTime CreatedTime {get; set;}
        public ArrayList Points{get; set;}

        public MarketDocument(){}

        public MarketDocument(string mRID, int revisionNumber, 
        string type, string senderMRID, string receiverMRID, 
        DateTime createdTime, ArrayList points)
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
     

    }
    public class Point 
    {
        public int Position {get; set;}
        public int Quantity {get; set;}

        public Point(){}
        public Point(int position, int quantity)
        {
            this.Position = position;
            this.Quantity = quantity;
        }
    }
}