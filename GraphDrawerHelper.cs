namespace LackmannApi
{
    // mark this for deletion
    public class GraphDrawerHelper
    {
        MarketDocument _marketDocument {get; set;}
        // this class will make it possible to draw the 35040 points (give or take)
        // in a GUI. The idea is to have methods that aggregate the reading into
        // hours, then days, and then weeks and so on.

        // Moreover, the class should also include the functionality of examining a particular
        // aggregated point.

        public GraphDrawerHelper(MarketDocument marketDocument)
        {
            _marketDocument = marketDocument;
        }
        
        

    }
}