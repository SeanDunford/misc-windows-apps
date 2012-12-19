using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Syndication;

namespace USFBlogReader
{
    class USFBlogFeed
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PubDate { get; set; }

        private List<BlogFeedItem> _FeedItem;
        public List<BlogFeedItem> FeedItem
        {
            get { return this._FeedItem; }
        }
    }

    class BlogFeedItem
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public DateTime PubDate { get; set; }
        public Uri FeedLink { get; set; }
    }

    class USFBlogDataSource
    {
        private ObservableCollection<USFBlogFeed> _theFeeds;
        public ObservableCollection<USFBlogFeed> theFeeds
        {
            get { return this._theFeeds; }
        }

        public async Task GetFeedsDataAsync()
        {
            Task<USFBlogFeed> feed = GetBlogFeedsAsync("http://feeds.wired.com/wired/index");
            Task<USFBlogFeed> feed2 = GetBlogFeedsAsync("http://windowsteamblog.com/windows/b/developers/atom.aspx");

            this.theFeeds.Add(await feed);
            this.theFeeds.Add(await feed2);
        }
        public async Task<USFBlogDataSource> GetBlogFeedsAsync(string aFeedUri)
        {
            SyndicationClient client = new SyndicationClient();
            SyndicationFeed feedObject;
            Uri FeedUri = new Uri(aFeedUri);

            try
            {
                feedObject = await client.RetrieveFeedAsync(FeedUri);

                USFBlogFeed feedData = new USFBlogFeed();
                feedData.Title = feedObject.Title.Text;
                feedData.PubDate = feedObject.Items[0].PublishedDate.DateTime;
                if (feedObject.Subtitle != null) { feedData.Description = feedObject.Subtitle.Text; }
                if (feedObject.Items != null && feedObject.Items.Count > 0)
                {
                    int currentitem = 0;
                    feedData.PubDate = feedObject.Items[0].PublishedDate.DateTime;
                    foreach (SyndicationItem feedItem in feedObject.Items)
                    {
                        currentitem++;
                        BlogFeedItem blogFeedItem = new BlogFeedItem();
                        blogFeedItem.Author = feedItem.Authors[0].Name;
                        blogFeedItem.Title = feedItem.Title.Text;
                        blogFeedItem.PubDate = feedItem.PublishedDate.DateTime;


                        if (feedObject.SourceFormat == SyndicationFormat.Rss20)
                        {
                            if (feedItem.Summary != null)
                            {
                                blogFeedItem.Content = feedItem.Summary.Text;


                            }
                            if (feedObject.Links != null) blogFeedItem.FeedLink = feedItem.Links[0].Uri;
                            if (feedObject.SourceFormat == SyndicationFormat.Atom10)
                            {
                                if (feedItem.Summary != null)
                                {
                                    blogFeedItem.Content = feedItem.Content.Text;
                                    blogFeedItem.FeedLink = new Uri(FeedUri.ToString() + currentitem.ToString());

                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            { ;}

            
        }
        public static USFBlogFeed getFeed(string feedTitle)
        {
            var currentAppData = App.Curren.Resources["USFBlogDataSource"] as USFBlogFeed;
            var feedMatch = currentAppData._theFeeds.Where((USFBlogFeed) => USFBlogFeed.Title.Equals(feedTitle)); 
    }
    }
}