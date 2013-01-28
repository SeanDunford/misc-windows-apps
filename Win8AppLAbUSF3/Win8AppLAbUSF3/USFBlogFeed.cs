using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Syndication;

namespace Win8AppLAbUSF3
{
    class USFBlogFeed
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PubDate { get; set; }

        private List<BlogFeedItem> FeedItem;
        public List<BlogFeedItem> lFeedItem
        {
            get { return this.FeedItem; }

        }
    }
    class BlogFeedItem
    {
        public string title { get; set; }
        public string author { get; set; }
        public string content { get; set; }
        public DateTime PubDate { get; set; }
        public Uri FeedLink { get; set; }
    }
    class USFBlogDataSource
    {
        private ObservableCollection<USFBlogFeed> theFeeds;
        public ObservableCollection<USFBlogFeed> lTheFeeds
        {
            get{return this.theFeeds;}
        }
       // public 
    }
}